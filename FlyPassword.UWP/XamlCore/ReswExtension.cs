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
