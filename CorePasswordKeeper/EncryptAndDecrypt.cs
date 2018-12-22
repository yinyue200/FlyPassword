using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CorePasswordKeeper
{
    public static class EncryptAndDecrypt
    {
        static readonly byte[] IV = new byte[16] { 0x20, 0x01, 0x09, 0x11, 0xff, 0x96, 0x0c, 0xe0, 0xf3, 0x75, 0x45, 0xee, 0xfe, 0x7e, 0xd5, 0x34 };
        public static void Encrypt(Stream data,string key,Stream output)
        {
            if (data.Length == 0)
                return;
            var bytekey = HashStr(key);
            using (var aes = GetAes())
            {
                aes.Key = bytekey;
                aes.IV = IV;               
                var enc = aes.CreateEncryptor();
                using (var cryptoStream = new CryptoStream(output, enc, CryptoStreamMode.Write))
                {
                    data.CopyTo(cryptoStream);
                }
            }
        }
        static byte[] HashStr(string str)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(Encoding.UTF8.GetBytes(str));
            }
        }

        private static Aes GetAes()
        {
            var aes= Aes.Create();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.FeedbackSize = 128;
            return aes;
        }

        public static void Decrypt(Stream data, string key, Stream output)
        {
            if (data.Length == 0)
                return;
            var bytekey = HashStr(key);
            using (var aes = GetAes())
            {
                aes.Key = bytekey;
                aes.IV = IV;
                var enc = aes.CreateDecryptor();
                using (var cryptoStream = new CryptoStream(data, enc, CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(output);
                }
            }
        }
    }
}
