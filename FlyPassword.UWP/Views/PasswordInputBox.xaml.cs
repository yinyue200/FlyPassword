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
using CorePasswordKeeper;
using Windows.ApplicationModel.DataTransfer;
using FlyPassword.UWP.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FlyPassword.UWP.Views
{
    public sealed partial class PasswordInputBox : UserControl
    {


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordInputBox), new PropertyMetadata(null, PropertyChangedCallback));


        public PasswordInputBox()
        {
            this.InitializeComponent();
        }
        static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pwdbx = (PasswordInputBox)d;
            pwdbx.pwdbox.Password = (string)e.NewValue;
        }
        public event RoutedEventHandler PasswordChanged{
            add
            {
                pwdbox.PasswordChanged += value;
            }
            remove
            {
                pwdbox.PasswordChanged -= value;
            }
        }

        private void Pwdbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = pwdbox.Password;
            var re = PasswordAdvisor.GetPasswordStrength(Password);
            probar.Value = (int)re;
            switch (re)
            {
                default:
                case PasswordStrength.Blank:
                    break;
                case PasswordStrength.VeryWeak:
                    probar.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                    break;
                case PasswordStrength.Weak:
                    probar.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
                    break;
                case PasswordStrength.Medium:
                    probar.Foreground = new SolidColorBrush(Windows.UI.Colors.Yellow);
                    break;
                case PasswordStrength.Strong:
                    probar.Foreground = new SolidColorBrush(Windows.UI.Colors.YellowGreen);
                    break;
                case PasswordStrength.VeryStrong:
                    probar.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        int length = 10;

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var slii = (int)sli1.Value;
            if (slii != length)
            {
                length = slii;
                AppBarButton_Click(null, null);
            }
        }

        private void ToggleSwitch_Toggled(object sender, RoutedEventArgs e)
        {
            AppBarButton_Click(null, null);
        }

        private void ToggleSwitch_Toggled_1(object sender, RoutedEventArgs e)
        {
            AppBarButton_Click(null, null);
        }
        CorePasswordKeeper.Generator.IPasswordGenerator generator;
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            if(ts1==null)
            {
                return;
            }
            if (sender != null&&generator!=null)
            {
                Password = generator.Generate();
            }
            else if (ts1.IsOn)
            {
                var rm = new Random();
                Password = new string((generator=new CorePasswordKeeper.Generator.PronouncablePasswordGenerator
                    (length, new CorePasswordKeeper.Generator.PronouncableWordLength(3, 8)))
                    .Generate().Select(a => a == ' ' ? (rm.Next(-1, 1) == 0 ? '_' : rm.Next(-1, 9)).ToString(System.Globalization.NumberFormatInfo.InvariantInfo)[0] : a).ToArray());
            }
            else
            {
                Password = (generator = new CorePasswordKeeper.Generator.NormalPasswordGenerator(true, true, true, ts2.IsOn, length)).Generate();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            pwdbox.PasswordRevealMode = PasswordRevealMode.Visible;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            pwdbox.PasswordRevealMode = PasswordRevealMode.Hidden;
        }

        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dataPackage = new DataPackage();
            dataPackage.SetText(Password);
            if (ApiInfo.IsUniversalApiContractV7Available)
                Clipboard.SetContentWithOptions(dataPackage, new ClipboardContentOptions() { IsAllowedInHistory = false, IsRoamable = false });
            else
                Clipboard.SetContent(dataPackage);
        }
    }
}
