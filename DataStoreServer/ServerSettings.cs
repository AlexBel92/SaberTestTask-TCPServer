using System.Net;

namespace DataStoreServer
{
    class ServerSettings
    {
        public const string Position = "ServerSettings";

        public string IpAddressString { get; set; }
        public int GetPortNumber { get; set; }
        public int AddPortNumber { get; set; }
        public int UpdatePortNumber { get; set; }

        public IPAddress GetIPAddress() => IPAddress.Parse(IpAddressString);
    }
}
