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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FlyPassword.UWP.XamlCore
{
    public enum NullableBool
    {
        Null,
        False,
        True,
    }
    //从越飞阅读里copy过来
    public static class ProgressBarVisualHelper
    {
        public static NullableBool GetYFHelperVisibility(FrameworkElement obj)
        {
            return (NullableBool)obj.GetValue(YFHelperVisibilityProperty);
        }
        public static void SetYFHelperVisibilityForBool(FrameworkElement obj, bool? value)
        {
            SetYFHelperVisibility(obj, value == null ? NullableBool.Null : (value == true ? NullableBool.True : NullableBool.False));
        }
        public static void SetYFHelperVisibility(FrameworkElement obj, NullableBool value)
        {
            obj.SetValue(YFHelperVisibilityProperty, value);
        }

        public static readonly DependencyProperty YFHelperVisibilityProperty =
            DependencyProperty.RegisterAttached("YFHelperVisibility", typeof(NullableBool), typeof(FrameworkElement), new PropertyMetadata(NullableBool.Null, OnYFHelperVisibilityChanged));
        private static void OnYFHelperVisibilityChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var value = (NullableBool)e.NewValue;
            if (value != NullableBool.Null)
            {
                bool v2 = value == NullableBool.True ? true : false;
                if (obj is ProgressBar element)
                {
                    element.Visibility = v2 ? Visibility.Visible : Visibility.Collapsed;
                    element.IsIndeterminate = v2;
                }
                else if (obj is ProgressRing element2)
                {
                    element2.Visibility = v2 ? Visibility.Visible : Visibility.Collapsed;
                    element2.IsActive = v2;
                }
            }
        }

    }
}
