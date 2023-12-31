using System;
using System.IO;
using System.Text;

namespace Ionic.Zip
{
	/// <summary>
	///   A class for collecting the various options that can be used when
	///   Reading zip files for extraction or update.
	/// </summary>
	///
	/// <remarks>
	///   <para>
	///     When reading a zip file, there are several options an
	///     application can set, to modify how the file is read, or what
	///     the library does while reading.  This class collects those
	///     options into one container.
	///   </para>
	///
	///   <para>
	///     Pass an instance of the <c>ReadOptions</c> class into the
	///     <c>ZipFile.Read()</c> method.
	///   </para>
	///
	/// <seealso cref="M:Ionic.Zip.ZipFile.Read(System.String,Ionic.Zip.ReadOptions)" />.
	/// <seealso cref="M:Ionic.Zip.ZipFile.Read(System.IO.Stream,Ionic.Zip.ReadOptions)" />.
	/// </remarks>
	public class ReadOptions
	{
		/// <summary>
		/// An event handler for Read operations.  When opening large zip
		/// archives, you may want to display a progress bar or other
		/// indicator of status progress while reading.  This parameter
		/// allows you to specify a ReadProgress Event Handler directly.
		/// When you call <c>Read()</c>, the progress event is invoked as
		/// necessary.
		/// </summary>
		public EventHandler<ReadProgressEventArgs> ReadProgress { get; set; }

		/// <summary>
		/// The <c>System.IO.TextWriter</c> to use for writing verbose status messages
		/// during operations on the zip archive.  A console application may wish to
		/// pass <c>System.Console.Out</c> to get messages on the Console. A graphical
		/// or headless application may wish to capture the messages in a different
		/// <c>TextWriter</c>, such as a <c>System.IO.StringWriter</c>.
		/// </summary>
		public TextWriter StatusMessageWriter { get; set; }

		/// <summary>
		/// The <c>System.Text.Encoding</c> to use when reading in the zip archive. Be
		/// careful specifying the encoding.  If the value you use here is not the same
		/// as the Encoding used when the zip archive was created (possibly by a
		/// different archiver) you will get unexpected results and possibly exceptions.
		/// </summary>
		///
		/// <seealso cref="P:Ionic.Zip.ZipFile.ProvisionalAlternateEncoding" />
		public Encoding Encoding { get; set; }
	}
}
