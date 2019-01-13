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
using System.Text;
using Newtonsoft.Json;

namespace CorePasswordKeeper
{
    public class Record
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public IList<RecordEntry> RecordEntries { get; set; } = new List<RecordEntry>();
        public ISet<string> Tags { get; } = new HashSet<string>();
        public ISet<string> Folders { get; } = new HashSet<string>();
        [JsonIgnore]
        public bool IsFav
        {
            get => Tags.Contains("_fav");
            set => _ = value ? Tags.Add("_fav") : Tags.Remove("_fav");
        }

    }
    public enum RecordEntryType
    {
        Unspecified,
        ShortText,
        Password,
        Email,
        PhoneNumber,
        Url,
        Pin,
        Date,
        UserName,
        LongText
    }
    public class RecordEntry
    {
        public RecordEntry(string displayName, string value, bool isSecret)
        {
            DisplayName = displayName;
            Value = value;
            IsSecret = isSecret;
        }
        public RecordEntryType Type { get; set; }
        public string DisplayName { get; set; }
        public string Value { get; set; }
        public bool IsSecret { get; set; }
    }
}
