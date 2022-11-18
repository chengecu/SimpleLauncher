﻿
using SimpleLauncher.Common;
using SimpleLauncher.Model;
using System;
using System.Collections.Generic;
using System.IO;
using static SimpleLauncher.Common.Patch.UA.HexUtility;

namespace SimpleLauncher.Common.Patch.UA
{
    public class HexUtility
    {
        public static bool EqualsBytes(byte[] b1, params byte[] b2)
        {
            if (b1.Length != b2.Length)
                return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }
            return true;
        }

        public static byte[] Replace(byte[] sourceByteArray, List<HexReplaceEntity> replaces)
        {
            byte[] newByteArray = new byte[sourceByteArray.Length];
            Buffer.BlockCopy(sourceByteArray, 0, newByteArray, 0, sourceByteArray.Length);
            int offset = 0;
            foreach (HexReplaceEntity rep in replaces)
            {
                if (EqualsBytes(rep.oldValue, rep.newValue))
                {
                    continue;
                }

                for (; offset < sourceByteArray.Length; offset++)
                {
                    if (sourceByteArray[offset] == rep.oldValue[0])
                    {
                        if (sourceByteArray.Length - offset < rep.oldValue.Length)
                            break;

                        bool find = true;
                        for (int i = 1; i < rep.oldValue.Length - 1; i++)
                        {
                            if (sourceByteArray[offset + i] != rep.oldValue[i])
                            {
                                find = false;
                                break;
                            }
                        }
                        if (find)
                        {
                            Buffer.BlockCopy(rep.newValue, 0, newByteArray, offset, rep.newValue.Length);
                            offset += (rep.newValue.Length - 1);
                            break;
                        }
                    }
                }
            }
            return newByteArray;
        }


        public class HexReplaceEntity
        {
            public byte[] oldValue { get; set; }

            public byte[] newValue { get; set; }

        }
    }


    internal class UA_Util
    {

        //const string CN_KEY = "key/UA-CN.txt";
        //const string OS_KEY = "key/UA-OS.txt";
        //const string GC_KEY = "key/UA-key.txt";
        const string CN_KEY = "UA-CN.txt";
        const string OS_KEY = "UA-OS.txt";
        const string GC_KEY = "UA-key.txt";
        byte[] UA;
        byte[] UA_CN;
        byte[] UA_OS;
        byte[] UA_key;

        GameInfo gi;
        public UA_Util(GameInfo gi)
        {
            this.gi = gi;
            try
            {
                UA_CN = KeyHelper.GetKey(CN_KEY);
                UA_OS = KeyHelper.GetKey(OS_KEY);
                UA_key = KeyHelper.GetKey(GC_KEY);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public void Patch(string ua_file)
        {
            try
            {

                UA = File.ReadAllBytes(ua_file+".bak");
            }
            catch (Exception)
            {

                throw;
            }


            switch (gi.GetGameType())
            {
                case GameInfo.GameType.CN:
                    PatchCN(ua_file);
                    break;
                case GameInfo.GameType.OS:
                    PatchOS(ua_file);
                    break;
                //case GameInfo.GameType.UnKnown:
                //    break;
                default:
                    throw new Exception("未知版本的客户端！");
                    //break;
            }
        }

        public void PatchCN(string FilePath)
        {

            if (UA_CN.Length != UA_key.Length)
            {
                throw new Exception("key length doesn't match.");
                //return;
            }

            int Offset = 0;
            int DataLength;



            List<HexReplaceEntity> UA_CN_list = new List<HexReplaceEntity>();
            while ((DataLength = UA_CN.Length - Offset) > 0)
            {
                if (DataLength > 8)
                    DataLength = 8;
                HexReplaceEntity hexReplaceEntity = new HexReplaceEntity();
                hexReplaceEntity.oldValue = new byte[8];
                Buffer.BlockCopy(UA_CN, Offset, hexReplaceEntity.oldValue, 0, DataLength);
                hexReplaceEntity.newValue = new byte[8];
                Buffer.BlockCopy(UA_key, Offset, hexReplaceEntity.newValue, 0, DataLength);
                UA_CN_list.Add(hexReplaceEntity);
                Offset += DataLength;
            }


            byte[] UA_CN_patched = HexUtility.Replace(UA, UA_CN_list);

            if (!HexUtility.EqualsBytes(UA, UA_CN_patched))
            {
                try
                {
                    File.WriteAllBytes(FilePath, UA_CN_patched);
                }
                catch (IOException e)
                {
                    throw;
                }
                return;
            }
        }

        public void PatchOS(string FilePath)
        {


            if (UA_OS.Length != UA_key.Length)
            {
                throw new Exception("key length doesn't match.");
                //return;
            }

            int Offset = 0;
            int DataLength;

            List<HexReplaceEntity> UA_OS_list = new List<HexReplaceEntity>();
            while ((DataLength = UA_OS.Length - Offset) > 0)
            {
                if (DataLength > 8)
                    DataLength = 8;
                HexReplaceEntity hexReplaceEntity = new HexReplaceEntity();
                hexReplaceEntity.oldValue = new byte[8];
                Buffer.BlockCopy(UA_OS, Offset, hexReplaceEntity.oldValue, 0, DataLength);
                hexReplaceEntity.newValue = new byte[8];
                Buffer.BlockCopy(UA_key, Offset, hexReplaceEntity.newValue, 0, DataLength);
                UA_OS_list.Add(hexReplaceEntity);
                Offset += DataLength;
            }

            byte[] UA_OS_patched = HexUtility.Replace(UA, UA_OS_list);

            if (!HexUtility.EqualsBytes(UA, UA_OS_patched))
            {
                try
                {
                    File.WriteAllBytes(FilePath, UA_OS_patched);
                }
                catch (IOException e)
                {
                    throw;
                }
                return;
            }



        }

    }
}
