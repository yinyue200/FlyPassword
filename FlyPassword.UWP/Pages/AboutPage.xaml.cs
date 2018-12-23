using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using FlyPassword.UWP.Core;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyPassword.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AboutPage : Page
    {
        public AboutPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            savebtn.IsEnabled = false;
            try
            {
                var fsp = new FileSavePicker
                {
                    DefaultFileExtension = ".yfpwd"
                };
                fsp.FileTypeChoices.Add("yfpwd", new List<string> { ".yfpwd" });
                var file=await fsp.PickSaveFileAsync();
                if (file != null)
                {
                    await (await TmpData.GetPwdFileAsync()).CopyAndReplaceAsync(file);
                }
            }
            finally
            {
                savebtn.IsEnabled = true;
            }
        }
    }
}
