using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.Bee
{
    public class BeeFile : BeeBase
    {
        /// <summary>
        /// Public url of this file.
        /// </summary>
        [JsonProperty("public-url")]
        public string PublicUrl { get; set; }

        /// <summary>
        /// Public url of the thumbnail of this file.
        /// </summary>
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
