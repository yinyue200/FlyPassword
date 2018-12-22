using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FlyPassword.UWP.Core
{
    static class TmpData
    {
        public static DataLoader DataLoader = DataLoader.Create();
        public static CorePasswordKeeper.PasswordKeeper PasswordKeeper { get; set; }
        public static async Task<IStorageFile> GetPwdFileAsync()
        {
            return await ApplicationData.Current.LocalFolder.CreateFileAsync("records.yfpwd",CreationCollisionOption.OpenIfExists);
        }
    }
}
