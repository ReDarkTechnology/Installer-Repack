﻿using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Win32;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace Uninstaller
{
    public partial class MainForm : Form
    {
        #region Setup Config
{CONFIG_DATA}
        #endregion

        #region Variables
        [DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        #endregion

        #region Form
        public MainForm()
        {
            InitializeComponent();
            defaultDestinationPath = ResolveDirectoryPath(defaultDestinationPath);
            if(File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uninstall.inf")))
            {
                var lines = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Uninstall.inf"));
                Task.Run(() => UninstallFiles(lines));
            }
            else
            {
                MessageBox.Show("Uninstall manifest file isn't found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }

        public string ResolveDirectoryPath(string path)
        {
            path = path.Replace("%UserDocuments%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            path = path.Replace("%UserProfile%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));
            path = path.Replace("%LocalAppData%", Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            path = path.Replace("%AppData%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path = path.Replace("%ProgramFiles%", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
            path = path.Replace("%ProgramFilesX86%", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
            return path;
        }
        #endregion

        #region Uninstallation
        public Task UninstallFiles(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)))
                {
                    float progress = ((float)i) / ((float)paths.Length) * 100;
                    Invoke(new Action(() => deleteLabel.Text = $"Deleting: {path}"));
                    Invoke(new Action(() => installationbar.Value = progress > 100 ? 100 : (int)progress));
                    tryagain:
                    try
                    {
                        File.Delete(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path));
                    }
                    catch (Exception e)
                    {
                        var result = MessageBox.Show($"Unable to remove file {Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path)}: {e.Message}", "Continue?", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
                        if (result == DialogResult.Abort)
                        {
                            MessageBox.Show("Uninstallation is cancelled.");
                            Invoke(new Action(End));
                            return Task.CompletedTask;
                        }
                        else if(result == DialogResult.Retry)
                        {
                            i--;
                            goto tryagain;
                        }
                    }
                }
            }
            try
            {
                Invoke(new Action(() => deleteLabel.Text = $"Deleting shortcuts..."));
                string roamingStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.StartMenu);
                string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
                string appStartMenuPath = Path.Combine(roamingStartMenuPath, "Programs", companyName);
                string appStartMenuPath2 = Path.Combine(commonStartMenuPath, "Programs", companyName);
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + $"\\{programName}.lnk";

                if (Directory.Exists(appStartMenuPath))
                    RecursiveDelete(new DirectoryInfo(appStartMenuPath));
                if (Directory.Exists(appStartMenuPath2))
                    RecursiveDelete(new DirectoryInfo(appStartMenuPath2));
                if (Directory.Exists(appStartMenuPath2))
                    RecursiveDelete(new DirectoryInfo(appStartMenuPath2));
                if (File.Exists(shortcutAddress))
                    File.Delete(shortcutAddress);

                DeleteUninstaller();

                Invoke(new Action(() => deleteLabel.Text = $"Deleting associations..."));

                foreach (var assoc in associations)
                {
                    if (DeleteAssociation(assoc.Key, programName))
                        SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
                }
                
                foreach (var assoc in urlProtocols)
                {
                	DeleteUrlProtocol(assoc);
                }

                MessageBox.Show($"{programName} has been uninstalled from your computer!");
            }
            catch (Exception e)
            {
                
            }
            Invoke(new Action(End));
            return Task.CompletedTask;
        }

        private void DeleteUninstaller()
        {
            using (RegistryKey parent = Registry.LocalMachine.OpenSubKey(
                         @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", true))
            {
                if (parent == null)
                {
                    //throw new Exception("Uninstall registry key not found.");
                }
                try
                {
                    RegistryKey key = null;

                    try
                    {
                        string guidText = programGUID;
                        key = parent.OpenSubKey(guidText, true);
                        if(key != null)
                        {
                            parent.DeleteSubKeyTree(guidText);
                        }
                    }
                    finally
                    {
                        if (key != null)
                        {
                            key.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    /*throw new Exception(
                        "An error occurred writing uninstall information to the registry. The service is fully installed but can only be uninstalled manually through the command line.",
                        ex);*/
                }
            }
        }

        public static bool DeleteAssociation(string extension, string progId)
        {
            bool madeChanges = false;
            madeChanges |= RemoveKey(@"Software\Classes\" + extension);
            madeChanges |= RemoveKey(@"Software\Classes\" + progId);
            madeChanges |= RemoveKey($@"Software\Classes\{progId}\shell\open\command");
            return madeChanges;
        }
        
        private void DeleteUrlProtocol(string customProtocol)
        {
            Registry.ClassesRoot.DeleteSubKey(customProtocol, false);
        }

        public void End()
        {
            var proc = new ProcessStartInfo();
            proc.FileName = "cmd.exe";
            proc.Verb = "runas";
            proc.Arguments = "/C ping 99.99.99.78 -n 1 -w 3000 > Nul & RD /s /q \"" + AppDomain.CurrentDomain.BaseDirectory + "\"";
            Process.Start(proc);
            Close();
            RecursiveDelete(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory));
        }
        #endregion

        #region Utility
        public static void RecursiveDelete(DirectoryInfo baseDir)
        {
            if (!baseDir.Exists)
                return;

            foreach (var dir in baseDir.EnumerateDirectories())
                RecursiveDelete(dir);

            baseDir.Delete(true);
        }

        private static bool RemoveKey(string keyPath)
        {
            if (Registry.CurrentUser.OpenSubKey(keyPath) != null)
            {
                Registry.CurrentUser.DeleteSubKeyTree(keyPath);
                return true;
            }

            return false;
        }
        #endregion
    }
}