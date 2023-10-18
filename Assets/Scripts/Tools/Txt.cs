/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Txt.cs
 * author:    taoye
 * created:   2020/8/31
 * descrip:   文本实用函数
 ***************************************************************/

using System;
using System.Text;

namespace BigDream
{
    public static class Txt
    {
        [ThreadStatic]
        private static StringBuilder s_CachedStringBuilder = null;

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="arg0">字符串参数0</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string Format(string format, object arg0)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, arg0);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="arg0">字符串参数0</param>
        /// <param name="arg1">字符串参数1</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string Format(string format, object arg0, object arg1)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, arg0, arg1);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="arg0">字符串参数0</param>
        /// <param name="arg1">字符串参数1</param>
        /// <param name="arg2">字符串参数2</param>
        /// <returns>返回格式化后的字符串</returns>
        public static string Format(string format, object arg0, object arg1, object arg2)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, arg0, arg1, arg2);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 获取格式化字符串
        /// </summary>
        /// <param name="format">字符串格式</param>
        /// <param name="args">字符串参数数组</param>
        /// <returns></returns>
        public static string Format(string format, params object[] args)
        {
            if (format == null)
            {
                throw new Exception("格式无效。");
            }

            if (args == null)
            {
                throw new Exception("参数无效。");
            }

            CheckCachedStringBuilder();
            s_CachedStringBuilder.Length = 0;
            s_CachedStringBuilder.AppendFormat(format, args);
            return s_CachedStringBuilder.ToString();
        }

        /// <summary>
        /// 检查缓存StringBuilder的初始化
        /// </summary>
        private static void CheckCachedStringBuilder()
        {
            if (s_CachedStringBuilder == null)
            {
                s_CachedStringBuilder = new StringBuilder(1024);
            }
        }
    }
}


