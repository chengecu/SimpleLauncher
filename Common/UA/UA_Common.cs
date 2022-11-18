using SimpleLauncher.Common.Patch.UA;
using SimpleLauncher.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SimpleLauncher.Model.GameInfo;

namespace SimpleLauncher.Common.Patch
{
    internal class UA_Common
    {
        GameInfo gameInfo;


        public const string UA_FILE_NAME = "UserAssembly.dll";
        public const string UA_PKGV_FILE_PATH = "Native/UserAssembly.dll";

        public UA_Common(GameInfo gameInfo)
        {
            this.gameInfo = gameInfo;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns>绝对路径</returns>
        public string GetUAPatchFile()
        {
            var ret = "";
            if (gameInfo == null)
            {
                //MessageBox.Show("游戏路径配置不正确");
                return "";
            }
            var gamedir = Path.GetDirectoryName(gameInfo.GameExePath);

            string file_path = Path.Combine(gameInfo.GetGameDataFolder(), "Native", UA_FILE_NAME);
            string file_path_osrel = Path.Combine(gameInfo.GetGameDataFolder(), "Native", UA_FILE_NAME);

            if (gameInfo.GetGameType() == GameType.OS)
            {
                file_path = file_path_osrel;
            }
            return file_path;
        }

        public string GetUAPatchFodler()
        {
            return Path.GetDirectoryName(GetUAPatchFile());
        }


        public void Pacth()
        {
            DoPatchUserAssembly(GetUAPatchFile());
        }
        private void DoPatchUserAssembly(string p)
        {

            try
            {
                new UA_Util(gameInfo).Patch(p);
            }
            catch (Exception ex)
            {
                throw;
            }

            Logger.Info("成功补丁了 UserAssembly.dll!");
        }
    }
}
