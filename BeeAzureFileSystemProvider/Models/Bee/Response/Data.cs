using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.Bee.Response
{
    public class Data
    {
        [JsonProperty("meta")]
        public object Meta { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }
    }
}
