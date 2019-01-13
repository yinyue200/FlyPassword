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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using FlyPassword.UWP.Core;
using Windows.UI.Popups;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace FlyPassword.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (TmpData.PasswordKeeper != null)
            {
                System.Diagnostics.Debug.WriteLine(viewall.Parent);
                App_MainListRefresh(null, null);
            }
            else
            {
                this.Frame.Navigate(typeof(PasswordInputPage));
            }
        }
        (Type, Func<object>) lastpar = (typeof(MasterPage), ()=>null);
        private void NvSample_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                contentFrame.Navigate(typeof(AboutPage));
            }
            else
            {
                (Type, Func<object>) par = (null,()=> null);
                if (args.InvokedItemContainer == viewall)
                {
                    par = (typeof(MasterPage), ()=>null);
                }
                else if (args.InvokedItemContainer == fav)
                {
                    par = (typeof(MasterPage), ()=>TmpData.PasswordKeeper.Records.Where(a => a.Tags.Contains("_fav")).ToList());
                }
                else if (args.InvokedItemContainer == folder)
                {
                    //todo
                    _ = new MessageDialog(TmpData.loader.GetString("indevelopment")).ShowAsync();
                }


                if (par.Item1 != null)
                {
                    contentFrame.Navigate(par.Item1, par.Item2);
                    lastpar = par;
                }
                
            }
        }

        private void NvSample_Loaded(object sender, RoutedEventArgs e)
        {
            ((NavigationViewItem)nvSample.SettingsItem).Content = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView().GetString("mainpage_settings");
        }


        private void Addnewentrybt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (addnewentrybt.SelectedIndex==0)
            {
                contentFrame.Navigate(typeof(NewRecordPage));
            }
            else if (addnewentrybt.SelectedIndex == -1)
            {
                return;
            }
            else
            {
                _ = new MessageDialog(TmpData.loader.GetString("indevelopment")).ShowAsync();
            }
            addnewentrybt.SelectedIndex = -1;

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            App.MainListRefresh += App_MainListRefresh;
        }

        private void App_MainListRefresh(object sender, EventArgs e)
        {
            if (contentFrame.Content == null)
            {
                contentFrame.Navigate(lastpar.Item1, lastpar.Item2);
            }
            else if (contentFrame.Content is IRefreshable refreshable)
            {
                refreshable.Refresh();
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            App.MainListRefresh -= App_MainListRefresh;
        }
    }
}
