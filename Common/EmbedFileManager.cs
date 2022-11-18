using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLauncher.Common
{
    internal class EmbedFileManager
    {
        /// <summary>
        /// 释放内嵌资源至指定位置
        /// </summary>
        /// <param name="resource">嵌入的资源，此参数写作：命名空间.文件夹名.文件名.扩展名</param>
        /// <param name="path">释放到位置</param>
        private static void ExtractFile(String resource, String path)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            BufferedStream input = new BufferedStream(assembly.GetManifestResourceStream(resource));
            FileStream output = new FileStream(path, FileMode.Create);
            byte[] data = new byte[1024];
            int lengthEachRead;
            while ((lengthEachRead = input.Read(data, 0, data.Length)) > 0)
            {
                output.Write(data, 0, lengthEachRead);
            }
            output.Flush();
            output.Close();
        }


    }
    public static class KeyHelper
    {
        public static Byte[] GetKey(string file)
        {
            Stream sr = null; ;
            try
            {
                var _assembly = Assembly.GetExecutingAssembly();//获取当前执行代码的程序集
                sr = _assembly.GetManifestResourceStream($"SimpleLauncher.key.{file}");

            }
            catch
            {
                //AConsole.e(new Spectre.Console.Markup("访问资源错误"));
                throw;
            }

            return streamToByteArray(sr);
        }

        private static byte[] streamToByteArray(Stream input)
        {
            MemoryStream ms = new MemoryStream();
            input.CopyTo(ms);
            return ms.ToArray();
        }
    }
}
