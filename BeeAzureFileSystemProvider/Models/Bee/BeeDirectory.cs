using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.Bee
{
    public class BeeDirectory : BeeBase
    {
        /// <summary>
        /// Number of contained items (directories + files).
        /// </summary>
        [JsonProperty("item-count")]
        public int ItemCount { get; set; }
    }
}
