using SimpleLauncher.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLauncher
{
    internal class Config
    {
        public static Config Instance = new Config();

        public GameInfo GameInfo;

        public ProxyConfig ProxyConfig = new ProxyConfig("localhost:8080", true);
        


        public bool ProxyOnly = false;

        
    }
}
