using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using MS.WindowsAPICodePack.Internal;

namespace Microsoft.WindowsAPICodePack.Shell
{
	[ComImport]
	[Guid("4df0c730-df9d-4ae3-9153-aa6b82e9795a")]
	internal class KnownFolderManagerClass : IKnownFolderManager
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void FolderIdFromCsidl(int csidl, out Guid knownFolderID);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void FolderIdToCsidl([In][MarshalAs(UnmanagedType.LPStruct)] Guid id, out int csidl);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void GetFolderIds(out IntPtr folders, out uint count);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern HResult GetFolder([In][MarshalAs(UnmanagedType.LPStruct)] Guid id, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void GetFolderByName(string canonicalName, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void RegisterFolder([In][MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid, [In] ref KnownFoldersSafeNativeMethods.NativeFolderDefinition knownFolderDefinition);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void UnregisterFolder([In][MarshalAs(UnmanagedType.LPStruct)] Guid knownFolderGuid);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void FindFolderFromPath([In][MarshalAs(UnmanagedType.LPWStr)] string path, [In] int mode, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.PreserveSig | MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern HResult FindFolderFromIDList(IntPtr pidl, [MarshalAs(UnmanagedType.Interface)] out IKnownFolderNative knownFolder);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void Redirect();
	}
}
