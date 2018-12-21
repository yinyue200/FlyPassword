using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CorePasswordKeeper
{
    public static class EncryptAndDecrypt
    {
        static readonly byte[] IV = new byte[16] { 0x23, 0x7a, 0x6a, 0xff, 0xea, 0xbd, 0x4f, 0xee, 0x63, 0x70, 0x45, 0xbe, 0xae, 0xfe, 0x85, 0x99 };
        public static void Encrypt(Stream data,string key,Stream output)
        {
            var bytekey = Encoding.UTF8.GetBytes(key);
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

        private static Aes GetAes()
        {
            var aes= Aes.Create();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        public static void Decrypt(Stream data, string key, Stream output)
        {
            var bytekey = Encoding.UTF8.GetBytes(key);
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
