/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  GZip.cs
 * author:    yuanjiashun
 * created:   2023/9/14
 * descrip:   GZip压缩解压缩实用函数
 ***************************************************************/
using System;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;

//using UnityEngine.Profiling;

namespace BigDream
{
    public static class GZip
    {
        /// <summary>
        /// 压缩文本为base64
        /// </summary>
        /// <param name="content">文本</param>
        /// <returns>压缩后文本</returns>
        public static string CompressToBase64(string content)
        {
            MemoryStream ms = new MemoryStream();
            GZipOutputStream gzip = new GZipOutputStream(ms);
            byte[] binary = Encoding.UTF8.GetBytes(content);
            gzip.Write(binary, 0, binary.Length);
            gzip.Close();
            var jsonString = Convert.ToBase64String(ms.ToArray());
            return jsonString;
        }

        /// <summary>
        /// 解压base64文本
        /// </summary>
        /// <param name="content">文本</param>
        /// <returns>解压后文本</returns>
        public static string UncompressFromBase64(string content)
        {
            byte[] compressedContent = Convert.FromBase64String(content);
            GZipInputStream gzip = new GZipInputStream(new MemoryStream(compressedContent));
            MemoryStream ms = new MemoryStream();
            int count = 0;
            byte[] data = new byte[256];
            while ((count = gzip.Read(data, 0, data.Length)) != 0)
            {
                ms.Write(data, 0, count);
            }
            byte[] uncompressed = ms.ToArray();
            return Encoding.UTF8.GetString(uncompressed);
        }

        /// <summary>
        /// 压缩字节流到文本
        /// </summary>
        /// <param name="content">字节流</param>
        /// <returns>压缩后字节流</returns>
        public static byte[] CompressToBytes(byte[] content)
        {
            //Profiler.BeginSample("GZip");
            MemoryStream ms = new MemoryStream();
            GZipOutputStream gzip = new GZipOutputStream(ms);
            gzip.Write(content, 0, content.Length);
            gzip.Close();
            //Profiler.EndSample();
            return ms.ToArray();
        }

        /// <summary>
        /// 解压文本到字节流
        /// </summary>
        /// <param name="content">文本</param>
        /// <returns>解压后字节流</returns>
        public static byte[] UncompressToBytes(byte[] content)
        {
            byte[] compressedContent = content;
            GZipInputStream gzip = new GZipInputStream(new MemoryStream(compressedContent));
            MemoryStream ms = new MemoryStream();
            int count = 0;
            byte[] data = new byte[256];
            while ((count = gzip.Read(data, 0, data.Length)) != 0)
            {
                ms.Write(data, 0, count);
            }
            byte[] uncompressed = ms.ToArray();
            return uncompressed;
        }

    }

}


