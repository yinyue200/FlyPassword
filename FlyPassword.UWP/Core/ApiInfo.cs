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
