/***************************************************************
 * (c) copyright 2021 - 2025, Solar.Game
 * All Rights Reserved.
 * -------------------------------------------------------------
 * filename:  AESEncrypt.cs
 * author:    yuanjiashun
 * created:   2021/12/9
 * descrip:   AES加密解密实用函数
 ***************************************************************/
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace BigDream
{
    public static class AESEncrypt
    {
        /// <summary>
        /// 秘钥长度
        /// 长度必须为16
        /// </summary>
        private const int SecretBytesLength = 16;

        /// <summary>
        /// 临时字节数组
        /// </summary>
        private static List<byte> m_sTempBytes = new List<byte>();

        /// <summary>
        /// AES加密为Base64文本
        /// 传入key与iv时表示使用静态加密/不传key与iv时表示使用动态加密
        /// </summary>
        /// <param name="content">明文字符串</param>
        /// <param name="specialKey">指定的加密Key，为null时使用随机生成的16位加密key</param>
        /// <param name="specialIv">指定的加密IV，为null时使用随机生成的16位加密iv</param>
        /// <returns></returns>
        public static string EncodeToBase64(string content, string specialKey = null, string specialIv = null)
        {
            byte[] keyArray = null;
            byte[] ivArray = null;
            if (specialKey != null)
            {
                keyArray = Encoding.UTF8.GetBytes(specialKey);
            }
            else
            {
                keyArray = GetRandomSecretBytes();
            }
            if (specialIv != null)
            {
                ivArray = Encoding.UTF8.GetBytes(specialIv);
            }
            else
            {
                ivArray = GetRandomSecretBytes();
            }

            byte[] toEncryptArray = Encoding.UTF8.GetBytes(content);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            cTransform.Dispose();

            m_sTempBytes.Clear();
            if (specialKey == null)
            {
                m_sTempBytes.AddRange(keyArray);
            }
            if (specialIv == null)
            {
                m_sTempBytes.AddRange(ivArray);
            }
            m_sTempBytes.AddRange(resultArray);

            return Convert.ToBase64String(m_sTempBytes.ToArray());
        }

        /// <summary>
        /// AES解密Base64文本
        /// 传入key与iv时表示使用静态解密/不传key与iv时表示使用动态解密
        /// </summary>
        /// <param name="content">密文字符串</param>
        /// <param name="specialKey">如果加密字符串使用指定的Key，解密时必须传入对应的加密key</param>
        /// <param name="specialIv">如果加密字符串使用指定的Iv，解密时必须传入对应的解密iv</param>
        /// <returns></returns>
        public static string DecodeFromBase64(string content, string specialKey = null, string specialIv = null)
        {
            // 矫正Base64文本内容，参考地址:https://github.com/dotnet/core/issues/7523
            content = content.Replace('_', '/').Replace('-', '+');
            byte[] bytes = Convert.FromBase64String(content);

            if (!Convert.TryFromBase64String(content, bytes, out int bytesWritten))
            {
                return string.Empty;
            }

            m_sTempBytes.Clear();
            m_sTempBytes.AddRange(bytes);

            if(m_sTempBytes.Count > 0)
            {
                byte[] keyArray = null;
                byte[] ivArray = null;
                int tempSecretLength = 0;

                if (specialKey != null)
                {
                    keyArray = Encoding.UTF8.GetBytes(specialKey);
                }
                else
                {
                    keyArray = m_sTempBytes.GetRange(0, SecretBytesLength).ToArray();
                    tempSecretLength = tempSecretLength + SecretBytesLength;
                }
                if (specialIv != null)
                {
                    ivArray = Encoding.UTF8.GetBytes(specialIv);
                }
                else
                {
                    ivArray = m_sTempBytes.GetRange(SecretBytesLength, SecretBytesLength).ToArray();
                    tempSecretLength = tempSecretLength + SecretBytesLength;
                }

                byte[] encodedArray = m_sTempBytes.GetRange(tempSecretLength, m_sTempBytes.Count - tempSecretLength).ToArray();
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(encodedArray, 0, encodedArray.Length);
                cTransform.Dispose();

                return Encoding.UTF8.GetString(resultArray);
            }

            return string.Empty;
        }

        /// <summary>
        /// AES加密
        /// 传入key与iv时表示使用静态加密/不传key与iv时表示使用动态加密
        /// </summary>
        /// <param name="content">字节流</param>
        /// <param name="specialKey">指定的加密Key，为null时使用随机生成的16位加密key</param>
        /// <param name="specialIv">指定的加密IV，为null时使用随机生成的16位加密iv</param>
        /// <returns>字节流</returns>
        public static byte[] EncodeToBytes(byte[] content, string specialKey = null, string specialIv = null)
        {
            byte[] keyArray = null;
            byte[] ivArray = null;
            if (specialKey != null)
            {
                keyArray = Encoding.ASCII.GetBytes(specialKey);
            }
            else
            {
                keyArray = GetRandomSecretBytes();
            }
            if (specialIv != null)
            {
                ivArray = Encoding.ASCII.GetBytes(specialIv);
            }
            else
            {
                ivArray = GetRandomSecretBytes();
            }

            byte[] toEncryptArray = content;
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.IV = ivArray;
            rDel.Mode = CipherMode.CBC;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            cTransform.Dispose();

            m_sTempBytes.Clear();
            if (specialKey == null)
            {
                m_sTempBytes.AddRange(keyArray);
            }
            if (specialIv == null)
            {
                m_sTempBytes.AddRange(ivArray);
            }
            m_sTempBytes.AddRange(resultArray);

            return m_sTempBytes.ToArray();
        }

        /// <summary>
        /// AES解密
        /// 传入key与iv时表示使用静态解密/不传key与iv时表示使用动态解密
        /// </summary>
        /// <param name="content">密文字符串</param>
        /// <param name="specialKey">如果加密字符串使用指定的Key，解密时必须传入对应的加密key</param>
        /// <param name="specialIv">如果加密字符串使用指定的Iv，解密时必须传入对应的解密iv</param>
        /// <returns></returns>
        public static byte[] DecodeToBytes(byte[] content, string specialKey = null, string specialIv = null)
        {
            byte[] bytes = content;

            m_sTempBytes.Clear();
            m_sTempBytes.AddRange(bytes);

            if (m_sTempBytes.Count > 0)
            {
                byte[] keyArray = null;
                byte[] ivArray = null;
                int tempSecretLength = 0;

                if (specialKey != null)
                {
                    keyArray = Encoding.ASCII.GetBytes(specialKey);
                }
                else
                {
                    keyArray = m_sTempBytes.GetRange(0, SecretBytesLength).ToArray();
                    tempSecretLength = tempSecretLength + SecretBytesLength;
                }
                if (specialIv != null)
                {
                    ivArray = Encoding.ASCII.GetBytes(specialIv);
                }
                else
                {
                    ivArray = m_sTempBytes.GetRange(SecretBytesLength, SecretBytesLength).ToArray();
                    tempSecretLength = tempSecretLength + SecretBytesLength;
                }

                byte[] encodedArray = m_sTempBytes.GetRange(tempSecretLength, m_sTempBytes.Count - tempSecretLength).ToArray();
                RijndaelManaged rDel = new RijndaelManaged();
                rDel.Key = keyArray;
                rDel.IV = ivArray;
                rDel.Mode = CipherMode.CBC;
                rDel.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = rDel.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(encodedArray, 0, encodedArray.Length);
                cTransform.Dispose();

                return resultArray;
            }

            return Array.Empty<byte>();
        }

        /// <summary>
        /// 获取随机秘钥16字节（Key、IV）
        /// </summary>
        /// <returns></returns>
        public static byte[] GetRandomSecretBytes()
        {
            byte[] iv = new byte[SecretBytesLength];
            for (int i = 0; i < iv.Length; i++)
            {
                iv[i] = (byte)UnityEngine.Random.Range(byte.MinValue, byte.MaxValue);
            }
            return iv;
        }

        /// <summary>
        /// 获取随机秘钥16字符（Key、IV）
        /// </summary>
        /// <returns></returns>
        public static string GetRandomSecretString()
        {
            byte[] iv = GetRandomSecretBytes();
            return Encoding.ASCII.GetString(iv);
        }

    }

}


