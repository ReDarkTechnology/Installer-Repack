using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.WindowsAPICodePack.Net
{
	[ComImport]
	[ComSourceInterfaces("Microsoft.Windows.NetworkList.Internal.INetworkEvents\0Microsoft.Windows.NetworkList.Internal.INetworkConnectionEvents\0Microsoft.Windows.NetworkList.Internal.INetworkListManagerEvents\0")]
	[Guid("DCB00C01-570F-4A9B-8D69-199FDBA5723B")]
	[TypeLibType(2)]
	[ClassInterface((short)0)]
	internal class NetworkListManagerClass : INetworkListManager
	{
		[DispId(6)]
		public virtual extern bool IsConnected
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(6)]
			get;
		}

		[DispId(5)]
		public virtual extern bool IsConnectedToInternet
		{
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[DispId(5)]
			get;
		}

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(7)]
		public virtual extern ConnectivityStates GetConnectivity();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(2)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern INetwork GetNetwork([In] Guid gdNetworkId);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(4)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern INetworkConnection GetNetworkConnection([In] Guid gdNetworkConnectionId);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(3)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern IEnumerable GetNetworkConnections();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[DispId(1)]
		[return: MarshalAs(UnmanagedType.Interface)]
		public virtual extern IEnumerable GetNetworks([In] NetworkConnectivityLevels Flags);
	}
}
