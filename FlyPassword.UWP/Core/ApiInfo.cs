using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace FlyPassword.UWP.Core
{
    static class ApiInfo
    {
        /// <summary>
        /// Windows 10.0.17134 (1803) 引入
        /// </summary>
        public readonly static bool IsUniversalApiContractV6Available = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6);
        /// <summary>
        /// Windows 10.0.17763 (1809) 引入
        /// </summary>
        public readonly static bool IsUniversalApiContractV7Available = ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 7);
    }
}
