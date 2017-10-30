using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models.Bee
{
    public class BeeBase
    {
        /// <summary>
        /// Application/directory for directories and specific mime-type for files.
        /// </summary>
        [JsonProperty("mime-type")]
        public string MimeType { get; set; }

        /// <summary>
        /// Resource name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Absolute path to resource in FSP.
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }

        /// <summary>
        /// UNIX time with (milliseconds) of last modification of this resource.
        /// </summary>
        [JsonProperty("last-modified")]
        public long LastModified { get; set; }

        /// <summary>
        /// Size (in byte) of the resource, this is zero (0) for directories.
        /// </summary>
        [JsonProperty("size")]
        public long Size { get; set; }

        /// <summary>
        /// Defines the access grants to the resource, can be ro for read-only access or rw for read-write access.
        /// </summary>
        [JsonProperty("permissions")]
        public string Permissions { get; set; }

        /// <summary>
        /// Generic extra data (for future extensions).
        /// </summary>
        [JsonProperty("extra")]
        public object Extra { get; set; }
    }
}
