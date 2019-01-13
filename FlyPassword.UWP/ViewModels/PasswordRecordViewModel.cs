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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePasswordKeeper;

namespace FlyPassword.UWP.ViewModels
{
    public class PasswordRecordViewModel
    {
        public string ItemId { get; private set; }
        public string DisplayName { get; private set; }
        public Record ORecord { get; set; }
        public string Info { get; set; }
        public static PasswordRecordViewModel CreateFromRecord(Record record)
        {
            return new PasswordRecordViewModel() { ItemId = record.Id, DisplayName = record.DisplayName,ORecord=record,Info=string.Join("\n",record.RecordEntries.Where(a=>!a.IsSecret&&string.IsNullOrEmpty(a.Value)==false).Select(a=>a.Value)) };
        }
    }
    public class PasswordRecordViewModelGroup:IGrouping<string,PasswordRecordViewModel>
    {
        public PasswordRecordViewModelGroup(string label, ObservableCollection<PasswordRecordViewModel> items)
        {
            Label = label;
            Items = items;
        }

        public string Label { get; private set; }
        public ObservableCollection<PasswordRecordViewModel> Items { get; private set; }

        string IGrouping<string, PasswordRecordViewModel>.Key => Label;

        public IEnumerator<PasswordRecordViewModel> GetEnumerator() => Items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => Items.GetEnumerator();
    }
}
