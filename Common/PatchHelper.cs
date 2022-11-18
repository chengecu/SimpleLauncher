using Newtonsoft.Json;
using SimpleLauncher.Common.Patch;
using SimpleLauncher.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleLauncher.Common
{
    internal class PatchHelper
    {
        GameInfo gameInfo;
        public const string PKG_VERSION_FILE = "pkg_version";

        public PatchHelper(GameInfo info)
        {
            gameInfo = info;
        }

        public enum PatchType
        {
            None,
            UserAssemby,
            Unknown,
        }

        //public PatchType GetPatchStatue()
        //{
        //    PatchType result = PatchType.None;

        //    try
        //    {
        //        //var official = GetHashFromPkgVer("Native/UserAssembly.dll");
        //        //var current = GetHashFromFile(Path.Combine(new UA_Common(gameInfo).GetUAPatchDir(), UA_FILE_NAME));
        //        //if (current != official)
        //        //{
        //        //    if (result == PatchType.None)
        //        //    {
        //        //        result = PatchType.UserAssemby;

        //        //    }
        //        //}

        //    }
        //    catch
        //    {
        //        throw;
        //    }

        //    return result;
        //}

        public string GetHashFromPkgVer(string filepath)
        {
            var gamedir = Path.GetDirectoryName(gameInfo.GameExePath);

            if (!File.Exists(Path.Combine(gamedir, PKG_VERSION_FILE)))
            {
                throw new Exception("找不到pkg_version文件！");

            }
            var lines = File.ReadAllLines(Path.Combine(gamedir, PKG_VERSION_FILE));

            string target = null;
            foreach (var item in lines)
            {
                if (item.Contains(filepath))
                {
                    target = item;
                    break;

                }

            }
            if (target == null)
            {
                throw new Exception($"在 pkg_version 文件中找不到项目{filepath}");
            }
            var md5_s = JsonConvert.DeserializeObject<PkgVersionItem>(target).md5;
            Logger.Debug($"官方UA的md5:{md5_s}");
            return md5_s;
        }

        public string GetHashFromFile(string filepath)
        {
            try
            {
                FileStream file = new FileStream(filepath, System.IO.FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile()失败:" + ex.Message);
            }

        }

        public string GetAbsolutePath(string relative)
        {
            return Path.Combine(gameInfo.GetGameDataFolder(), relative);
        }

        public string GetBackupPath(string p)
        {
            return p+".bak";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FILE_NAME">相对于游戏Data目录的路径 例如: Plugins/locales/am.pak </param>
        /// <exception cref="Exception"></exception>
        public void BackUpFile(string FILE_NAME)
        {
            Logger.Debug($"准备备份文件:{FILE_NAME}");

            string official_md5 = string.Empty;
            string current_md5 = string.Empty;
            string backup_md5 = string.Empty;


            //路径合法性检查
            if (!File.Exists(Path.Combine(gameInfo.GetGameDataFolder(),FILE_NAME)))
            {
                throw new Exception($"在游戏文件夹中找不到文件:{FILE_NAME}");
            }

            official_md5 = GetHashFromPkgVer(FILE_NAME);
            current_md5 = GetHashFromFile(GetAbsolutePath(FILE_NAME));

            

            var backup_f = GetBackupPath(GetAbsolutePath(FILE_NAME));
            if (File.Exists(backup_f))
            {
                backup_md5 = GetHashFromFile(backup_f);
                if (backup_md5==official_md5)
                {
                    Logger.Debug("备份和官方一致，无需再次备份");
                    return;
                }
                //继续备份
            }
            //不存在备份文件
            //官方不等于现在
            if (official_md5 != current_md5)
            {
                throw new Exception("现存文件md5与 pkg_version 存在差异！");
            }

            //开始备份
            File.Copy(GetAbsolutePath(FILE_NAME), backup_f);




        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="FILE_NAME">相对于游戏Data目录的路径 例如: Plugins/locales/am.pak </param>
        /// <exception cref="Exception"></exception>
        public void RestoreFile(string FILE_NAME)
        {
            Logger.Debug($"准备恢复文件:{FILE_NAME}");
            string official_md5 = string.Empty;
            string current_md5 = string.Empty;
            string backup_md5 = string.Empty;


            var backup_f = GetBackupPath(GetAbsolutePath(FILE_NAME));

            //路径合法性检查
            if (!File.Exists(backup_f))
            {
                throw new Exception($"在游戏文件夹中找不到备份文件:{FILE_NAME}");
            }


            official_md5 = GetHashFromPkgVer(FILE_NAME);
            current_md5 = GetHashFromFile(GetAbsolutePath(FILE_NAME));

            if (official_md5==current_md5)
            {
                Logger.Debug("现存和官方一致，无需恢复");

                return;
            }

            //if (!File.Exists(backup_f))
            //{
            //    throw new Exception($"在游戏文件夹中找不到备份文件:{FILE_NAME}");
            //}

            backup_md5 = GetHashFromFile(backup_f);

            if (backup_md5!= official_md5)
            {
                throw new Exception($"备份文件和 pkg_version 不一致!{FILE_NAME}");

            }

            File.Copy(backup_f,GetAbsolutePath(FILE_NAME), true);



        }

        public void Pacth()
        {
            BackUpFile(UA_Common.UA_PKGV_FILE_PATH);

            new UA_Common(gameInfo).Pacth();

        }
        public void Restore()
        {
            RestoreFile(UA_Common.UA_PKGV_FILE_PATH);


        }




    }
}
