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
using FlyPassword.UWP.ViewModels;
using CorePasswordKeeper;
using FlyPassword.UWP.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FlyPassword.UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewRecordPage : Page
    {
        public NewRecordPage()
        {
            this.InitializeComponent();
        }

        private void Rdc_Loaded(object sender, RoutedEventArgs e)
        {
            rdc.DataContext = PasswordRecordViewModel.CreateFromRecord(AddDefaultEntry(new Record() { Id = Guid.NewGuid().ToString(), DisplayName = string.Empty }));
            rdc.Frame = Frame;
            rdc.showeditpanel();
        }
        Record AddDefaultEntry(Record record)
        {
            var ls = new List<RecordEntry>
            {
                new RecordEntry(TmpData.loader.GetString("entryname_loginname"), string.Empty, false),
                new RecordEntry(TmpData.loader.GetString("entryname_email"), string.Empty, false),
                new RecordEntry(TmpData.loader.GetString("entryname_pwd"), string.Empty, true),
                new RecordEntry(TmpData.loader.GetString("entryname_phonenum"), string.Empty, false),
                new RecordEntry(TmpData.loader.GetString("entryname_url"), string.Empty, false),
                new RecordEntry(TmpData.loader.GetString("entryname_qaq"), string.Empty, false),
                new RecordEntry(TmpData.loader.GetString("entryname_qaa"), string.Empty, false)
            };
            record.RecordEntries = ls;
            return record;
        }
    }
}
