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
using FlyPassword.UWP.XamlCore;
using FlyPassword.UWP.Core;
using System.Threading.Tasks;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyPassword.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PasswordInputPage : Page
    {
        public PasswordInputPage()
        {
            this.InitializeComponent();
        }
        
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            pwdbox.IsEnabled = false;
            unlockbtn.IsEnabled = false;
            ProgressBarVisualHelper.SetYFHelperVisibilityForBool(ring, true);
            try
            {
                TmpData.Password = pwdbox.Password;
                await TmpData.LoadKeeperAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            catch
            {
                await new MessageDialog(TmpData.loader.GetString("passwordnotok")).ShowAsync();
            }
            finally
            {
                ProgressBarVisualHelper.SetYFHelperVisibilityForBool(ring, false);
                unlockbtn.IsEnabled = true;
                pwdbox.IsEnabled = true;
            }
        }

        private void Pwdbox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Button_Click(sender, e);
            }
        }
    }
}
