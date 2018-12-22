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
    }
}
