using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLauncher.Model
{
    internal class ProxyConfig
    {

        public ProxyConfig( string proxyServer, bool usehttp = false, string proxyPort = "25565")
        {
            ProxyServer = proxyServer;
            UseHttp = usehttp;
            ProxyPort = proxyPort;
        }

        public string ProxyServer { get; set; }

        public string ProxyPort { get; set; }

        public bool UseHttp { get; set; }
        public bool ProxyEnable { get; internal set; }
    }
}
