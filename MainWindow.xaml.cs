using SimpleLauncher.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Xml.Linq;

namespace SimpleLauncher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Logger.Init(logBox);
            Logger.Debug("Hello!");
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            Task.Run(() =>
            {
                Logger.Debug("调试模式已开启。");

                Program.Run();

                Logger.Debug("10s后退出程序。");

                Thread.Sleep(10000);

                Environment.Exit(0);

            });

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult vr = MessageBox.Show("确定退出？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question);
            if (vr != MessageBoxResult.OK)
            {
                e.Cancel = true;
            }
        }

        
        public void SetMinized()
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                Instance.WindowState = WindowState.Minimized;
            });
        }
        public void SetNormal()
        {
            MainWindow.Instance.Dispatcher.Invoke(() =>
            {
                Instance.WindowState = WindowState.Normal;

            });
        }
    }

}
