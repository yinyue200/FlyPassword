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
