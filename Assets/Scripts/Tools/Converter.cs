/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  Converter.cs
 * author:    taoye
 * created:   2021/1/13
 * descrip:   类型转换实用函数
 ***************************************************************/

using System;
using System.IO;
using System.Text;
using UnityEngine;


namespace BigDream
{
    public static class Converter
    {
        private const float InchesToCentimeters = 2.54f; // 1 inch = 2.54 cm
        private const float CentimetersToInches = 1f / InchesToCentimeters; // 1 cm = 0.3937 inches

        /// <summary>
        /// 获取数据在此计算机结构中存储时的字节顺序
        /// </summary>
        public static bool IsLittleEndian
        {
            get
            {
                return BitConverter.IsLittleEndian;
            }
        }

        /// <summary>
        /// 获取或设置屏幕每英寸点数
        /// </summary>
        public static float ScreenDpi
        {
            get;
            set;
        }

        /// <summary>
        /// 将像素转换为厘米
        /// </summary>
        /// <param name="pixels">像素</param>
        /// <returns>厘米</returns>
        public static float GetCentimetersFromPixels(float pixels)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return InchesToCentimeters * pixels / ScreenDpi;
        }

        /// <summary>
        /// 将厘米转换为像素
        /// </summary>
        /// <param name="centimeters">厘米</param>
        /// <returns>像素</returns>
        public static float GetPixelsFromCentimeters(float centimeters)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return CentimetersToInches * centimeters * ScreenDpi;
        }

        /// <summary>
        /// 将像素转换为英寸
        /// </summary>
        /// <param name="pixels">像素</param>
        /// <returns>英寸</returns>
        public static float GetInchesFromPixels(float pixels)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return pixels / ScreenDpi;
        }

        /// <summary>
        /// 将英寸转换为像素
        /// </summary>
        /// <param name="inches">英寸</param>
        /// <returns>像素</returns>
        public static float GetPixelsFromInches(float inches)
        {
            if (ScreenDpi <= 0)
            {
                throw new Exception("必须先设置屏幕每英寸点数ScreenDpi。");
            }

            return inches * ScreenDpi;
        }

        /// <summary>
        /// 将指定的布尔值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的布尔值</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByBoolean(bool value)
        {
            byte[] buffer = new byte[1];
            GetBytesByBoolean(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的布尔值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的布尔值</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByBoolean(bool value, byte[] buffer)
        {
            GetBytesByBoolean(value, buffer, 0);
        }

        /// <summary>
        /// 将指定的布尔值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的布尔值</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static void GetBytesByBoolean(bool value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 1 > buffer.Length)
            {
                throw new Exception("startIndex 无效。");
            }

            buffer[startIndex] = value ? (byte)1 : (byte)0;
        }

        /// <summary>
        /// 返回由字节数组中首字节转换来的布尔值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>如果 value 中的首字节非零，则返回 true，否则返回 false</returns>
        public static bool GetBoolean(byte[] value)
        {
            return BitConverter.ToBoolean(value, 0);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的一个字节转换来的布尔值
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>如果 value 中指定位置的字节非零，则返回 true，否则返回 false</returns>
        public static bool GetBoolean(byte[] value, int startIndex)
        {
            return BitConverter.ToBoolean(value, startIndex);
        }

        /// <summary>
        /// 将指定的 Unicode 字符值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的字符</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByChar(char value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort((short)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 Unicode 字符值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的字符</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByChar(char value, byte[] buffer)
        {
            GetBytesByShort((short)value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 Unicode 字符值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的字符</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static void GetBytesByChar(char value, byte[] buffer, int startIndex)
        {
            GetBytesByShort((short)value, buffer, startIndex);
        }

        /// <summary>
        /// 将字节数组中前两个字节转换成 Unicode 字符
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>Unicode 字符</returns>
        public static char GetChar(byte[] value)
        {
            return BitConverter.ToChar(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的两个字节转换成 Unicode 字符
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>Unicode 字符</returns>
        public static char GetChar(byte[] value, int startIndex)
        {
            return BitConverter.ToChar(value, startIndex);
        }

        /// <summary>
        /// 将指定的 16 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByShort(short value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 16 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByShort(short value, byte[] buffer)
        {
            GetBytesByShort(value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 16 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static unsafe void GetBytesByShort(short value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 2 > buffer.Length)
            {
                throw new Exception("startIndex 无效。");
            }

            fixed (byte* valueRef = buffer)
            {
                *(short*)(valueRef + startIndex) = value;
            }
        }

        /// <summary>
        /// 将字节数组中前两个字节转换成 16 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>16位有符号整数</returns>
        public static short GetInt16(byte[] value)
        {
            return BitConverter.ToInt16(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的两个字节转换成 16 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>16位有符号整数</returns>
        public static short GetInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToInt16(value, startIndex);
        }

        /// <summary>
        /// 将指定的 16 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByUShort(ushort value)
        {
            byte[] buffer = new byte[2];
            GetBytesByShort((short)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 16 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByUShort(ushort value, byte[] buffer)
        {
            GetBytesByShort((short)value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 16 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static void GetBytesByUShort(ushort value, byte[] buffer, int startIndex)
        {
            GetBytesByShort((short)value, buffer, startIndex);
        }

        /// <summary>
        /// 将字节数组中前两个字节转换成 16 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>16位无符号整数</returns>
        public static ushort GetUInt16(byte[] value)
        {
            return BitConverter.ToUInt16(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的两个字节转换成 16 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>16位无符号整数</returns>
        public static ushort GetUInt16(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt16(value, startIndex);
        }

        /// <summary>
        /// 将指定的 32 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByInt(int value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 32 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByInt(int value, byte[] buffer)
        {
            GetBytesByInt(value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 32 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static unsafe void GetBytesByInt(int value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 4 > buffer.Length)
            {
                throw new Exception("Start index 无效。");
            }

            fixed (byte* valueRef = buffer)
            {
                *(int*)(valueRef + startIndex) = value;
            }
        }

        /// <summary>
        /// 将字节数组中前四个字节转换成 32 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>32位有符号整数</returns>
        public static int GetInt32(byte[] value)
        {
            return BitConverter.ToInt32(value, 0);
        }

        /// <summary>
        /// 将字节数组中前四个字节转换成 32 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>32位有符号整数</returns>
        public static int GetInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToInt32(value, startIndex);
        }

        /// <summary>
        /// 将指定的 32 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByUInt(uint value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt((int)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 32 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByUInt(uint value, byte[] buffer)
        {
            GetBytesByInt((int)value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 32 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static void GetBytesByUInt(uint value, byte[] buffer, int startIndex)
        {
            GetBytesByInt((int)value, buffer, startIndex);
        }

        /// <summary>
        /// 将字节数组中前四个字节转换成 32 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>32位无符号整数</returns>
        public static uint GetUInt32(byte[] value)
        {
            return BitConverter.ToUInt32(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的四个字节转换成 32 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>32位无符号整数</returns>
        public static uint GetUInt32(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt32(value, startIndex);
        }

        /// <summary>
        /// 将指定的 64 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByLong(long value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong(value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 64 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByLong(long value, byte[] buffer)
        {
            GetBytesByLong(value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 64 位有符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static unsafe void GetBytesByLong(long value, byte[] buffer, int startIndex)
        {
            if (buffer == null)
            {
                throw new Exception("Buffer 无效。");
            }

            if (startIndex < 0 || startIndex + 8 > buffer.Length)
            {
                throw new Exception("Start index 无效。");
            }

            fixed (byte* valueRef = buffer)
            {
                *(long*)(valueRef + startIndex) = value;
            }
        }

        /// <summary>
        /// 将字节数组中前八个字节转换成 64 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>64位有符号整数</returns>
        public static long GetInt64(byte[] value)
        {
            return BitConverter.ToInt64(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的八个字节转换成 64 位有符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>64位有符号整数</returns>
        public static long GetInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToInt64(value, startIndex);
        }

        /// <summary>
        /// 将指定的 64 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByULong(ulong value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong((long)value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的 64 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static void GetBytesByULong(ulong value, byte[] buffer)
        {
            GetBytesByLong((long)value, buffer, 0);
        }

        /// <summary>
        /// 将指定的 64 位无符号整数值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static void GetBytesByULong(ulong value, byte[] buffer, int startIndex)
        {
            GetBytesByLong((long)value, buffer, startIndex);
        }

        /// <summary>
        /// 将字节数组中前八个字节转换成 64 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>64位无符号整数</returns>
        public static ulong GetUInt64(byte[] value)
        {
            return BitConverter.ToUInt64(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的八个字节转换成 64 位无符号整数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>64位无符号整数</returns>
        public static ulong GetUInt64(byte[] value, int startIndex)
        {
            return BitConverter.ToUInt64(value, startIndex);
        }

        /// <summary>
        /// 将指定的单精度浮点值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static unsafe byte[] GetBytesByFloat(float value)
        {
            byte[] buffer = new byte[4];
            GetBytesByInt(*(int*)&value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的单精度浮点值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static unsafe void GetBytesByFloat(float value, byte[] buffer)
        {
            GetBytesByInt(*(int*)&value, buffer, 0);
        }

        /// <summary>
        /// 将指定的单精度浮点值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static unsafe void GetBytesByFloat(float value, byte[] buffer, int startIndex)
        {
            GetBytesByInt(*(int*)&value, buffer, startIndex);
        }

        /// <summary>
        /// 将字节数组中前四个字节转换成单精度浮点数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>单精度浮点数</returns>
        public static float GetSingle(byte[] value)
        {
            return BitConverter.ToSingle(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的四个字节转换成单精度浮点数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>单精度浮点数</returns>
        public static float GetSingle(byte[] value, int startIndex)
        {
            return BitConverter.ToSingle(value, startIndex);
        }

        /// <summary>
        /// 将指定的双精度浮点值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <returns>字节数组</returns>
        public static unsafe byte[] GetBytesByDouble(double value)
        {
            byte[] buffer = new byte[8];
            GetBytesByLong(*(long*)&value, buffer, 0);
            return buffer;
        }

        /// <summary>
        /// 将指定的双精度浮点值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        public static unsafe void GetBytesByDouble(double value, byte[] buffer)
        {
            GetBytesByLong(*(long*)&value, buffer, 0);
        }

        /// <summary>
        /// 将指定的双精度浮点值转换成字节数组
        /// </summary>
        /// <param name="value">要转换的数字</param>
        /// <param name="buffer">用于存放结果的字节数组</param>
        /// <param name="startIndex">buffer内的起始位置</param>
        public static unsafe void GetBytesByDouble(double value, byte[] buffer, int startIndex)
        {
            GetBytesByLong(*(long*)&value, buffer, startIndex);
        }

        /// <summary>
        /// 将字节数组中前八个字节转换成双精度浮点数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>双精度浮点数</returns>
        public static double GetDouble(byte[] value)
        {
            return BitConverter.ToDouble(value, 0);
        }

        /// <summary>
        /// 将字节数组中指定位置的八个字节转换成双精度浮点数
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <returns>双精度浮点数</returns>
        public static double GetDouble(byte[] value, int startIndex)
        {
            return BitConverter.ToDouble(value, startIndex);
        }

        /// <summary>
        /// 将指定的 UTF-8 字符串转换成字节数组
        /// </summary>
        /// <param name="value">要转换的字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] GetBytesByString(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// 将字节数组转换成 UTF-8 字符串
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <returns>UTF-8字符串</returns>
        public static string GetString(byte[] value)
        {
            if (value == null)
            {
                throw new Exception("Value 无效。");
            }
            return Encoding.UTF8.GetString(value);
        }

        /// <summary>
        /// 将字节数组转换成 UTF-8 字符串
        /// </summary>
        /// <param name="value">字节数组</param>
        /// <param name="startIndex">value内的起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>UTF-8字符串</returns>
        public static string GetString(byte[] value, int startIndex, int length)
        {
            if (value == null)
            {
                throw new Exception("Value 无效。");
            }

            return Encoding.UTF8.GetString(value, startIndex, length);
        }

        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }

        public static string ToHex(this byte[] bytes)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        public static string ToHex(this byte[] bytes, string format)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in bytes)
            {
                stringBuilder.Append(b.ToString(format));
            }
            return stringBuilder.ToString();
        }

        public static string ToHex(this byte[] bytes, int offset, int count)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = offset; i < offset + count; ++i)
            {
                stringBuilder.Append(bytes[i].ToString("X2"));
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将指定目录中指定后缀名的所有文件转换为UTF8无BOM文件
        /// </summary>
        /// <param name="dirName">目录名称</param>
        /// <param name="suffixInfos">后缀名信息（如：*.lua.txt）</param>
        public static void ToNoBOMUTF8(string dirName, string suffixInfos)
        {
            string[] fileFullPaths = Directory.GetFiles(dirName, suffixInfos, SearchOption.AllDirectories);
            for (int index = 0; index < fileFullPaths.Length; index++)
            {
                string content = File.ReadAllText(fileFullPaths[index]);
                File.WriteAllText(fileFullPaths[index], content, new System.Text.UTF8Encoding(false));
            }
        }

        /// <summary>
        /// 获取纹理的字节流
        /// </summary>
        /// <param name="texture">纹理对象</param>
        /// <param name="isJpeg">是否为jpeg</param>
        /// <returns>转换后的字节流</returns>
        public static byte[] GetTextureBytes(Texture2D texture, bool isJpeg)
        {
            try
            {
                return isJpeg ? texture.EncodeToJPG(100) : texture.EncodeToPNG();
            }
            catch (UnityException)
            {
                return GetTextureBytesFromCopy(texture, isJpeg);
            }
            catch (ArgumentException)
            {
                return GetTextureBytesFromCopy(texture, isJpeg);
            }

#pragma warning disable 0162
            return null;
#pragma warning restore 0162
        }

        /// <summary>
        /// 从副本中获取纹理字节流
        /// </summary>
        /// <param name="texture">纹理对象</param>
        /// <param name="isJpeg">是否为jpeg</param>
        /// <returns>转换后的字节流</returns>
        private static byte[] GetTextureBytesFromCopy(Texture2D texture, bool isJpeg)
        {
            // Texture is marked as non-readable, create a readable copy and save it instead
            Debug.LogWarning("Saving non-readable textures is slower than saving readable textures");

            Texture2D sourceTexReadable = null;
            RenderTexture rt = RenderTexture.GetTemporary(texture.width, texture.height);
            RenderTexture activeRT = RenderTexture.active;

            try
            {
                Graphics.Blit(texture, rt);
                RenderTexture.active = rt;

                sourceTexReadable = new Texture2D(texture.width, texture.height, isJpeg ? TextureFormat.RGB24 : TextureFormat.RGBA32, false);
                sourceTexReadable.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0, false);
                sourceTexReadable.Apply(false, false);
            }
            catch (Exception e)
            {
                Debug.LogException(e);

                UnityEngine.Object.DestroyImmediate(sourceTexReadable);
                return null;
            }
            finally
            {
                RenderTexture.active = activeRT;
                RenderTexture.ReleaseTemporary(rt);
            }

            try
            {
                return isJpeg ? sourceTexReadable.EncodeToJPG(100) : sourceTexReadable.EncodeToPNG();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return null;
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(sourceTexReadable);
            }
        }

    }
}


