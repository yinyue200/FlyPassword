using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using CorePasswordKeeper;
using System.IO;

namespace FlyPassword.UWP.Core
{
    class DataLoader
    {   
        protected DataLoader()
        {

        }
        public static DataLoader Create()
        {
            return new DataLoader();
        }
        public async Task<PasswordKeeper> LoadFromFileAsync(IStorageFile storageFile,string password)
        {
            using(var stream= await storageFile.OpenStreamForReadAsync())
            {
                using(var mem=new MemoryStream())
                {
                    EncryptAndDecrypt.Decrypt(stream, password, mem);
                    var passwordKeeper = new PasswordKeeper();
                    mem.Seek(0, SeekOrigin.Begin);
                    passwordKeeper.LoadString(GetText(mem));
                    return passwordKeeper;
                }
            }
        }
        public async Task SaveToFileAsync(IStorageFile storageFile, string password,PasswordKeeper passwordKeeper)
        {
            using (var stream = await storageFile.OpenStreamForWriteAsync())
            {
                using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(passwordKeeper.SaveToJson())))
                {
                    EncryptAndDecrypt.Encrypt(mem, password, stream);
                }
            }
        }
        static string GetText(Stream stream)
        {
            using(var textreader=new StreamReader(stream))
            {
                return textreader.ReadToEnd();
            }
        }
    }
}
