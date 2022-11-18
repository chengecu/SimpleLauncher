using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleLauncher
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            if (Program.proxyh!=null)
            {

                Program.proxyh.Stop();
            }
            if (Program.ph!=null)
            {
                try
                {

                    Program.ph.Restore();
                }
                catch { }
            }

            Thread.Sleep(2000);
            base.OnExit(e);


        }
    }
}
