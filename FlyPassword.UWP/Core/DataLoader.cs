//FlyPassword
//Copyright(C) yinyue200.com 

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, version 3 of the License.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see https://github.com/yinyue200/FlyPassword/blob/master/LICENSE.
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
            using (var stream = await storageFile.OpenTransactedWriteAsync())
            {
                using (var mem = new MemoryStream(Encoding.UTF8.GetBytes(passwordKeeper.SaveToJson())))
                {
                    EncryptAndDecrypt.Encrypt(mem, password, stream.Stream.AsStreamForWrite());
                }
                await stream.CommitAsync();
            }
        }
        static string GetText(Stream stream)
        {
            using(var textreader=new StreamReader(stream,Encoding.UTF8))
            {
                return textreader.ReadToEnd();
            }
        }
    }
}
