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
        Text,
        Password,
        Email,
        PhoneNumber,
        Url,
        Pin,
        Date,
        UserName,
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
