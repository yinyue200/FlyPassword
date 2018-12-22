using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;
using Microsoft.VisualStudio.Threading;

namespace FlyPassword.UWP.Core
{
    static class TmpData
    {
        internal static string Password { get; set; }
        public static DataLoader DataLoader = DataLoader.Create();
        public static CorePasswordKeeper.PasswordKeeper PasswordKeeper { get; set; }
        static AsyncSemaphore semaphoreSlim = new AsyncSemaphore(1);
        public static async Task LoadKeeperAsync()
        {
            await Task.Run(async() =>
            {
                using (await semaphoreSlim.EnterAsync())
                {
                    PasswordKeeper = await DataLoader.LoadFromFileAsync(await GetPwdFileAsync(), Password);
                }
            });

        }
        public static async Task SaveKeeperAsync()
        {
            await Task.Run(async () =>
            {
                using (await semaphoreSlim.EnterAsync())
                {
                    await DataLoader.SaveToFileAsync(await GetPwdFileAsync(), Password, PasswordKeeper);
                }
            });
        }
        public static async Task<IStorageFile> GetPwdFileAsync()
        {
            return await ApplicationData.Current.LocalFolder.CreateFileAsync("records.yfpwd",CreationCollisionOption.OpenIfExists);
        }
        internal static Windows.ApplicationModel.Resources.ResourceLoader _loader;
        public static Windows.ApplicationModel.Resources.ResourceLoader loader
        {
            get
            {
                if (_loader == null)
                {
                    _loader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                }
                return _loader;
            }
        }
    }
}
