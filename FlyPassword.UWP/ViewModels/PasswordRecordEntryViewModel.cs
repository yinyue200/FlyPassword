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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace FlyPassword.UWP.ViewModels
{
    public class PasswordRecordEntryViewModel:DependencyObject
    {
        public bool IsPwdShowed { get; set; }
        public const string hidepwd = "●●●●●●";
        public Visibility PasswordBoxVisibility { get; private set; }
        public Visibility TextBoxVisibility { get; private set; }
        public Visibility ShowPwdBtnVisibility { get; private set; }
        public string DisplayName { get; private set; }


        public string DisplayValue
        {
            get { return (string)GetValue(DisplayValueProperty); }
            set { SetValue(DisplayValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DisplayValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DisplayValueProperty =
            DependencyProperty.Register("DisplayValue", typeof(string), typeof(PasswordRecordEntryViewModel), new PropertyMetadata(null));


        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordRecordEntryViewModel), new PropertyMetadata(null));

        public static PasswordRecordEntryViewModel CreateFromRecordEntry(RecordEntry record)
        {
            var pwdbtnbool = record.IsSecret && !string.IsNullOrEmpty(record.Value);
            return new PasswordRecordEntryViewModel()
            {
                DisplayName = record.DisplayName,
                DisplayValue = pwdbtnbool ? hidepwd : record.Value,
                Password = record.Value,
                PasswordBoxVisibility = converttovisibility(record.IsSecret),
                TextBoxVisibility = converttovisibility(!record.IsSecret),
                IsPwdShowed = !record.IsSecret,
                ShowPwdBtnVisibility = converttovisibility(pwdbtnbool)
            };
        }
        static Visibility converttovisibility(bool visibility)
        {
            return visibility ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
