using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CorePasswordKeeper
{
    public class PasswordKeeper
    {
        public IList<Record> Records { get; private set; }
        public void LoadString(string json)
        {
            Records = JsonConvert.DeserializeObject<List<Record>>(json);
            Records = new List<Record>() {
                new Record() { DisplayName = "aaa",RecordEntries=new List<RecordEntry> {new RecordEntry("pwd","123",true) } }
            , new Record() { DisplayName = "bbb", RecordEntries = new List<RecordEntry> { new RecordEntry("pwd", "123", true) } } };
        }
        public string SaveToJson()
        {
            return JsonConvert.SerializeObject(Records);
        }
    }
}
