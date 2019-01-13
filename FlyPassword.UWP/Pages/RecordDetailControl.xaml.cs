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
using CorePasswordKeeper;
using FlyPassword.UWP.Core;
using FlyPassword.UWP.ViewModels;
using FlyPassword.UWP.Views;
using Microsoft.VisualStudio.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FlyPassword.UWP.Pages
{
    public sealed partial class RecordDetailControl : UserControl
    {
        public Frame Frame { get; set; }
        public RecordDetailControl()
        {
            this.InitializeComponent();
        }
        bool disablecontrolevent = false;
        RangedObservableCollection<PasswordRecordEntryViewModel> passwordRecordEntryViewModels = new RangedObservableCollection<PasswordRecordEntryViewModel>();
        public PasswordRecordViewModel Record { get => (PasswordRecordViewModel)DataContext; }
        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext == null)
                return;
            if (sender != null)
            {
                hideeditpanel();
            }
            Record record = getrecord();
            disablecontrolevent = true;
            try
            {
                favappbtn.IsChecked = record.IsFav;
            }
            finally
            {
                disablecontrolevent = false;
            }
            title.Text = record.DisplayName;
            namebox.Text = record.DisplayName;
            passwordRecordEntryViewModels.Clear();
            passwordRecordEntryViewModels.AddRange(record.RecordEntries.Select(a => PasswordRecordEntryViewModel.CreateFromRecordEntry(a)));
        }

        private Record getrecord()
        {
            return Record.ORecord ?? TmpData.PasswordKeeper.Records.Where(a => a.Id == Record.ItemId).First();
        }

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var model = (PasswordRecordEntryViewModel)((FrameworkElement)sender).DataContext;
            var dataPackage = new DataPackage();
            dataPackage.SetText(model.Password);
            if(ApiInfo.IsUniversalApiContractV7Available)
                Clipboard.SetContentWithOptions(dataPackage, new ClipboardContentOptions() { IsAllowedInHistory = false, IsRoamable = false });
            else
                Clipboard.SetContent(dataPackage);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            showeditpanel();
        }

        public void showeditpanel()
        {
            infoshow.Visibility = Visibility.Collapsed;
            editpanel.Visibility = Visibility.Visible;
        }
        public void hideeditpanel()
        {
            infoshow.Visibility = Visibility.Visible;
            editpanel.Visibility = Visibility.Collapsed;
        }
        private void AppBarButton_Click_1(object sender, RoutedEventArgs e)
        {
            //save the data
            if (string.IsNullOrEmpty(namebox.Text))
            {
                _ = new MessageDialog(TmpData.loader.GetString("namecannotbeempty")).ShowAsync();
                return;
            }
            var record = TmpData.PasswordKeeper.Records.Where(a => a.Id == Record.ItemId).FirstOrDefault();
            if(record==null)
            {
                TmpData.PasswordKeeper.Records.Add(record = (Record.ORecord??new Record() { Id = Record.ItemId }));
            }
            record.DisplayName = namebox.Text;
            List<RecordEntry> ls = new List<RecordEntry>();
            foreach (var one in passwordRecordEntryViewModels)
            {
                ls.Add(new RecordEntry(one.DisplayName, one.Password, one.PasswordBoxVisibility == Visibility.Visible ? true : false));
                System.Diagnostics.Debug.WriteLine(one.DisplayName + " " + one.Password + "added"+one.DisplayValue);
            }
            record.RecordEntries = ls;
            TmpData.SaveKeeperAsync().Forget();
            UserControl_DataContextChanged(null, null);
            hideeditpanel();
            App.CallMainListRefresh(this,EventArgs.Empty);
        }

        private void AppBarButton_Click_2(object sender, RoutedEventArgs e)
        {
            UserControl_DataContextChanged(null, null);
            hideeditpanel();
            if (!TmpData.PasswordKeeper.Records.Any(a=> a.Id == Record.ItemId))
            {
                Back();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            var model = (PasswordRecordEntryViewModel)((FrameworkElement)sender).DataContext;
            if (model == null)
                return;
            model.DisplayValue = model.Password;
            model.IsPwdShowed = false;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            var model = (PasswordRecordEntryViewModel)((FrameworkElement)sender).DataContext;
            if (model == null)
                return;
            model.DisplayValue = PasswordRecordEntryViewModel.hidepwd;
            model.IsPwdShowed = true;
        }

        private void AppBarButton_Click_3(object sender, RoutedEventArgs e)
        {
            TmpData.PasswordKeeper.Records.Remove(Record.ORecord ?? TmpData.PasswordKeeper.Records.Where(a => a.Id == Record.ItemId).First());
            TmpData.SaveKeeperAsync().Forget();
            App.CallMainListRefresh(this, EventArgs.Empty);
            Back();
        }

        private void Back()
        {
            if (Frame != null)
                Frame.GoBack();
            editpanel.Visibility = Visibility.Collapsed;
            infoshow.Visibility = Visibility.Collapsed;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var ss= (TextBox)sender;
            var model = (PasswordRecordEntryViewModel)ss.DataContext;
            if (model == null)
                return;
            model.Password = ss.Text;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var ss = (PasswordBox)sender;
            var model = (PasswordRecordEntryViewModel)ss.DataContext;
            if (model == null)
                return;
            model.Password = ss.Password;
        }

        private void AppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if (disablecontrolevent)
                return;
            getrecord().IsFav = true;
            TmpData.SaveKeeperAsync().Forget();
        }

        private void AppBarToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (disablecontrolevent)
                return;
            getrecord().IsFav = false;
            TmpData.SaveKeeperAsync().Forget();
        }

        private async void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            var model = (PasswordRecordEntryViewModel)((FrameworkElement)sender).DataContext;
            if (model == null)
                return;
            var rd = new RecordEntry(model.DisplayName, model.Password, model.PasswordBoxVisibility == Visibility.Visible ? true : false);
            await NewMethod(model, rd,true);
        }

        private async System.Threading.Tasks.Task NewMethod(PasswordRecordEntryViewModel model, RecordEntry rd,bool sd)
        {
            var editLabelDialog = new EditLabelDialog(rd,sd);
            editLabelDialog.Deleted += (s, args) =>
            {
                passwordRecordEntryViewModels.Remove(model);
            };
            var result = await editLabelDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var index = passwordRecordEntryViewModels.IndexOf(model);
                if (index < 0)
                    index = passwordRecordEntryViewModels.Count;
                else
                    passwordRecordEntryViewModels.RemoveAt(index);
                passwordRecordEntryViewModels.Insert(index, PasswordRecordEntryViewModel.CreateFromRecordEntry(rd));
            }
        }

        private async void AppBarButton_Click_4(object sender, RoutedEventArgs e)
        {
            var rd = new RecordEntry(string.Empty, string.Empty, false);
            await NewMethod(null, rd,false);
        }
    }
}
