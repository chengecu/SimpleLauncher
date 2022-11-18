using SimpleLauncher.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleLauncher
{
    internal class Program
    {

        static Config cfg = Config.Instance;
        public static ProxyHelper.ProxyController proxyh;
        public static PatchHelper ph;
        public static void Run()
        {

            Logger.Debug("调试模式已开启。");

           
            #region 游戏文件获取
            var exePath = string.Empty;

            try
            {
                Logger.Info("尝试通过自身查找游戏文件...");

                exePath = GameHelper.GameLocalReader.GetGameExePath();
            }
            catch (Exception ex)
            {

                Logger.Debug("失败:\r\n\t" + ex.Message);

                try
                {
                    Logger.Debug("尝试通过注册表查找游戏文件...");

                    exePath = GameHelper.GameRegReader.GetGameExePath();
                }
                catch (Exception ex1)
                {
                    Logger.Debug("失败:\r\n" + ex1.Message);
                }
            }
            Logger.Debug($"目的可执行文件:{exePath}");

            if (!File.Exists(exePath))
            {
                Logger.Error("找不到游戏文件！");

                return;

            }
            else
            {
                Logger.Info($"找到了可执行文件:{exePath}");
            }
            cfg.GameInfo = new Model.GameInfo(exePath);




            #endregion

            ph = new PatchHelper(cfg.GameInfo);
            //var ps = ph.GetPatchStatue();

            try
            {
                Logger.Info("尝试进行补丁....");

                ph.Pacth();

            }
            catch (Exception ex)
            {
                Logger.Error("补丁失败！");
                Logger.Error(ex.Message+"\r\n" + ex.StackTrace);
                return;
            }


            try
            {
                Logger.Info("尝试启动代理....");

                proxyh = new ProxyHelper.ProxyController(cfg.ProxyConfig.ProxyPort, cfg.ProxyConfig.ProxyServer, cfg.ProxyConfig.UseHttp);

                proxyh.Start();
            }
            catch (Exception ex)
            {
                Logger.Error("启动代理失败！");
                Logger.Error(ex.Message + "\r\n" + ex.StackTrace);
                return;

            }



            Process p = new Process();
            try
            {
                Logger.Info("尝试启动游戏....");
                p = GameHelper.StartGame(cfg.GameInfo.GameExePath);
                //p = GameHelper.StartGame("C:\\Windows\\System32\\cmd.exe");


                MainWindow.Instance.SetMinized();

                

            }
            catch (Exception ex)
            {

                Logger.Error("启动游戏失败！");
                Logger.Error(ex.Message + "\r\n" + ex.StackTrace);


            }

            p.WaitForExit();

            MainWindow.Instance.SetNormal();

            Logger.Info("检测到游戏退出");

            Logger.Info("关闭代理....");
            try
            {
                proxyh.Stop();
            }
            catch { }


           

            try
            {
                Logger.Info("尝试恢复游戏文件....");

                ph.Restore();

                Logger.Info("恢复游戏文件成功！");

            }
            catch (Exception ex)
            {
                Logger.Error("恢复游戏文件失败！");
                Logger.Error(ex.Message + "\r\n" + ex.StackTrace);
                return;

            }

           
        }
    }
}
