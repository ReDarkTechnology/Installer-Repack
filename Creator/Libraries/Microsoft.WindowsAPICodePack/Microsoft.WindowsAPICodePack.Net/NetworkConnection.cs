using System;

namespace Microsoft.WindowsAPICodePack.Net
{
	public class NetworkConnection
	{
		private INetworkConnection networkConnection;

		public Network Network => new Network(networkConnection.GetNetwork());

		public Guid AdapterId => networkConnection.GetAdapterId();

		public Guid ConnectionId => networkConnection.GetConnectionId();

		public ConnectivityStates Connectivity => networkConnection.GetConnectivity();

		public DomainType DomainType => networkConnection.GetDomainType();

		public bool IsConnectedToInternet => networkConnection.IsConnectedToInternet;

		public bool IsConnected => networkConnection.IsConnected;

		internal NetworkConnection(INetworkConnection networkConnection)
		{
			this.networkConnection = networkConnection;
		}
	}
}
