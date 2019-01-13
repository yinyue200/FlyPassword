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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace FlyPassword.UWP.XamlCore
{
    [Bindable]
    [MarkupExtensionReturnType(ReturnType = typeof(string))]
    //[ContentProperty(Name = nameof(PropertyName))]
    class ReswExtension: MarkupExtension
    {
        public string PropertyName { get; set; }
        protected override object ProvideValue()
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
                return PropertyName;
            try
            {
                return Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView().GetString(PropertyName);
            }
            catch
            {
                return PropertyName;
            }
        }
    }
}
