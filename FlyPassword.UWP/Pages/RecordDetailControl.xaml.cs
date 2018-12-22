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
using FlyPassword.UWP.ViewModels;
using FlyPassword.UWP.Core;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace FlyPassword.UWP.Pages
{
    public sealed partial class RecordDetailControl : UserControl
    {
        public RecordDetailControl()
        {
            this.InitializeComponent();
        }
        public PasswordRecordViewModel Record { get => (PasswordRecordViewModel)DataContext; }
        private void UserControl_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext == null)
                return;
            var record = TmpData.PasswordKeeper.Records.Where(a => a.Id == Record.ItemId).First();
            entryls.ItemsSource = record.RecordEntries.Select(a=>PasswordRecordEntryViewModel.CreateFromRecordEntry(a));
        }
    }
}
