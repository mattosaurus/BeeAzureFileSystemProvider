using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Text;
using BeeFreeAzureFileSystemProvider.Models.Bee.Response;
using BeeFreeAzureFileSystemProvider.Models.Bee;
using BeeFreeAzureFileSystemProvider.AppCode;
using BeeFreeAzureFileSystemProvider.Models;
using BeeFreeAzureFileSystemProvider.Models.Bee.Request;

namespace BeeFreeAzureFileSystemProvider.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/Bee")]
    public class BeeController : Controller
    {
        private readonly IConfiguration _config;

        public BeeController(IConfiguration config)
        {
            _config = config;
        }

        // GET: api/Bee
        // List directories
        [HttpGet]
        public async Task<Response> ListDirectories()
        {
            CloudBlobContainer imageContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ImageConnection")));

            BeeClient client = new BeeClient(imageContainer, null);

            Response response = await client.ListDirectories();

            return response;
        }

        // GET: api/Bee/{directory path}/
        // List directory contents
        [HttpGet("{*directory:regex(^.*/$)}", Name = "ListDirectoryContents")]
        public async Task<Response> ListDirectoryContents(string directory)
        {
            CloudBlobContainer imageContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ImageConnection")));

            BeeClient client = new BeeClient(imageContainer, null);

            Response response = await client.ListDirectoryContents(directory);

            return response;
        }

        // POST: api/Bee/{directory path}
        // Create directory
        [HttpPost("{*directory:regex(^.*/$)}", Name = "CreateDirectory")]
        public async Task<Response> CreateDirectory(string directory)
        {
            CloudBlobContainer imageContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ImageConnection")));

            BeeClient client = new BeeClient(imageContainer, null);

            Response response = await client.CreateDirectory(directory);

            return response;
        }

        // POST: api/Bee/{directory path}
        // Delete directory
        [HttpDelete("{*directory:regex(^.*/$)}", Name = "DeleteDirectory")]
        public Response DeleteDirectory(string directory)
        {
            CloudBlobContainer imageContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ImageConnection")));

            BeeClient client = new BeeClient(imageContainer, null);

            Response response = client.DeleteDirectory(directory);

            return response;
        }

        // GET: api/Bee/{file path}
        // Get file metadata
        [HttpGet("{*file:regex(^.*(?<!/)$)}", Name = "GetFileMetadata")]
        public async Task<Response> GetFileMetadata(string file)
        {
            CloudBlobContainer imageContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ImageConnection")));

            BeeClient client = new BeeClient(imageContainer, null);

            Response response = await client.GetFileMetadata(file);

            return response;
        }

        // POST: api/Bee/{file path}
        // Upload file
        [HttpPost("{*file:regex(^.*(?<!/)$)}", Name = "UploadFile")]
        public async Task<Response> UploadFile([FromBody]UploadFileRequest uploadFileRequest, string file)
        {
            CloudBlobContainer imageContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ImageConnection")));
            CloudBlobContainer thumbnailContainer = new CloudBlobContainer(new Uri(_config.GetConnectionString("ThumbnailConnection")));

            BeeClient client = new BeeClient(imageContainer, thumbnailContainer);

            Response response = await client.UploadFile(uploadFileRequest.Source, file);

            return response;
        }
    }
}
