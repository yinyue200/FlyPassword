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
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FlyPassword.UWP.Views;
using FlyPassword.UWP.Core;
using Windows.Storage.Pickers;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyPassword.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new CreateNewPorfile();
            if(await window.ShowAsync() == ContentDialogResult.Primary)
            {
                if (window.Password == window.ConformPassword)
                {
                    if (!string.IsNullOrWhiteSpace(window.Password))
                    {
                        TmpData.Password = window.Password;
                        await TmpData.LoadKeeperAsync();
                        await TmpData.SaveKeeperAsync();
                        Frame.Navigate(typeof(MainPage));
                    }
                    else
                    {
                        await new MessageDialog(TmpData.loader.GetString("Passwordcannotbeempty")).ShowAsync();
                    }
                }
                else
                {
                    await new MessageDialog(TmpData.loader.GetString("Pleasekeepasswordsamewithconformpassword")).ShowAsync();
                }
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await new MessageDialog(TmpData.loader.GetString("indevelopment")).ShowAsync();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var fop = new FileOpenPicker();
            fop.FileTypeFilter.Add(".yfpwd");
            var file = await fop.PickSingleFileAsync();
            if (file != null)
            {
                await file.CopyAndReplaceAsync(await TmpData.GetPwdFileAsync());
                Frame.Navigate(typeof(PasswordInputPage));
            }
        }
    }
}
