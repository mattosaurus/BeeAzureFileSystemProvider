using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.Bee.Request
{
    public class UploadFileRequest
    {
        [JsonProperty("source")]
        public string Source { get; set; }
    }
}
