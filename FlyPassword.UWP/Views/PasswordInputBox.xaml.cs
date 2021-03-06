﻿//FlyPassword
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
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordInputBox), new PropertyMetadata(string.Empty, PropertyChangedCallback));


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
        bool isnotfirsttrue = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            isnotfirsttrue = pwdck.IsChecked==true;
            pwdck.IsChecked = true;
            AppBarButton_Click(null, null);
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
            const string  sp= "0123456789!@#$%^&*?_~-,()";
            if (ts1==null||ts2==null)
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
                    .Generate().Select(a => a == ' ' ? sp[rm.Next(0, ts2.IsOn==false?10:sp.Length-1)] : a).ToArray());
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

        private void Flyout_Closed(object sender, object e)
        {
            pwdck.IsChecked = isnotfirsttrue;
        }
    }
}
