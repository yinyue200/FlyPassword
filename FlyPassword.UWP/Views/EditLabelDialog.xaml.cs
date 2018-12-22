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
using Windows.UI.Popups;
using FlyPassword.UWP.Core;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyPassword.UWP.Views
{
    public sealed partial class EditLabelDialog : ContentDialog
    {
  //        <data name = "ret_email" xml:space="preserve">
  //  <value>email</value>
  //</data>
  //<data name = "ret_phonenum" xml:space="preserve">
  //  <value>PhoneNumber</value>
  //</data>
  //<data name = "ret_pin" xml:space="preserve">
  //  <value>PIN</value>
  //</data>
  //<data name = "ret_pwd" xml:space="preserve">
  //  <value>Password</value>
  //</data>
  //<data name = "ret_text" xml:space="preserve">
  //  <value>Text</value>
  //</data>
  //<data name = "ret_uns" xml:space="preserve">
  //  <value>Unspecified</value>
  //</data>
  //<data name = "ret_url" xml:space="preserve">
  //  <value>Url</value>
  //</data>
        RecordEntry RecordEntry { get; }
        Visibility showdel;
        public EditLabelDialog(RecordEntry recordEntry,bool showdel)
        {
            RecordEntry = recordEntry;
            this.InitializeComponent();
            this.showdel = showdel ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if(string.IsNullOrEmpty(namebox.Text))
            {
                _ = new MessageDialog(TmpData.loader.GetString("namecannotbeempty")).ShowAsync();
            }
            RecordEntry.DisplayName = namebox.Text;
            RecordEntry.IsSecret = issbox.IsChecked == true;
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            namebox.Text = RecordEntry.DisplayName;
            issbox.IsChecked = RecordEntry.IsSecret;
        }
        public event EventHandler<RecordEntry> Deleted;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Deleted?.Invoke(this, RecordEntry);
            Hide();
        }
    }
}
