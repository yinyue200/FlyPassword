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
        }
        public string SaveToJson()
        {
            return JsonConvert.SerializeObject(Records);
        }
    }
}
