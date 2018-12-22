using CorePasswordKeeper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlyPassword.UWP.ViewModels
{
    class PasswordRecordEntryViewModel
    {
        public string DisplayName { get; private set; }
        public string DisplayValue { get; private set; }
        public static PasswordRecordEntryViewModel CreateFromRecordEntry(RecordEntry record)
        {
            return new PasswordRecordEntryViewModel() { DisplayName = record.DisplayName, DisplayValue = record.Value };
        }
    }
}
