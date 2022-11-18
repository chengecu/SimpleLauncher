using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLauncher.Model
{
    internal class GameInfo
    {
        public string GameExeFolder { get; set; }

        public string GetGameDataFolder()
        {
            var dataDirName = string.Empty;
            switch (GetGameType())
            {
                case GameType.CN:
                    dataDirName = "YuanShen_Data";
                    break;
                case GameType.OS:
                    dataDirName = "GenshinImpact_Data";

                    break;
                default:
                    break;
            }
            return Path.Combine(GameExeFolder,dataDirName);
        }


        public GameInfo(string gameExePath)
        {
            GameExePath = gameExePath;

            GameExeFolder = Path.GetDirectoryName(gameExePath);


        }

        public enum GameType
        {
            CN,
            OS,
            UnKnown,
        }
        public GameType GetGameType()
        {
            GameType gameType = GameType.UnKnown;

            if (Directory.Exists(Path.Combine(GameExeFolder, "YuanShen_Data")))
            {
                gameType = GameType.CN;
            }
            else if (Directory.Exists(Path.Combine(GameExeFolder, "GenshinImpact_Data")))
            {
                gameType = GameType.OS;
            }

            return gameType;
        }

        public string GameExePath { get; set; }

    }
}
