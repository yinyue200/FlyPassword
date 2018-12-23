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
            Records = JsonConvert.DeserializeObject<List<Record>>(json)??new List<Record>();
        }
        public string SaveToJson()
        {
            var json = JsonConvert.SerializeObject(Records);
#if DEBUG
            _ = JsonConvert.DeserializeObject<List<Record>>(json);
#endif
            System.Diagnostics.Debug.WriteLine(json);
            return json;
        }
    }
}
