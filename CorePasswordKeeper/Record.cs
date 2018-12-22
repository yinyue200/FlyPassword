using System;
using System.Collections.Generic;
using System.Text;

namespace CorePasswordKeeper
{
    public class Record
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public IList<RecordEntry> RecordEntries { get; set; }
        public ISet<string> Tags { get; }
        public ISet<string> Folders { get; }

    }
    public enum RecordEntryType
    {
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
