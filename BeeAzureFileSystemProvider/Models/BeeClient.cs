using BeeFreeAzureFileSystemProvider.AppCode;
using BeeFreeAzureFileSystemProvider.Models.Bee.Response;
using Microsoft.WindowsAzure.Storage.Blob;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BeeFreeAzureFileSystemProvider.Models
{
    public class BeeClient
    {
        private CloudBlobContainer _imageContainer;
        private CloudBlobContainer _thumbnailContainer;

        public BeeClient(CloudBlobContainer imageContainer, CloudBlobContainer thumbnailContainer)
        {
            _imageContainer = imageContainer;
            _thumbnailContainer = thumbnailContainer;
        }

        public async Task<Response> ListDirectories(string prefix = "")
        {
            BlobContinuationToken continuationToken = null;

            CloudBlobDirectory directory = _imageContainer.GetDirectoryReference(prefix);
            BlobResultSegment resultSegment = await directory.ListBlobsSegmentedAsync(continuationToken);

            Response response = new Response();
            response.Data = new Data();
            response.Data.Meta = new Bee.BeeDirectory();
            response.Data.Meta = await Common.GetBeeDirectoryInfo(directory);

            response.Data.Items = new List<object>();

            foreach (IListBlobItem item in resultSegment.Results)
            {
                if (typeof(CloudBlobDirectory) == item.GetType())
                {
                    response.Data.Items.Add(await Common.GetBeeDirectoryInfo((item as CloudBlobDirectory)));
                }
            }

            response.Status = "success";
            return response;
        }

        public async Task<Response> ListDirectoryContents(string prefix = "")
        {
            BlobContinuationToken continuationToken = null;

            CloudBlobDirectory directory = _imageContainer.GetDirectoryReference(prefix);
            BlobResultSegment resultSegment = await directory.ListBlobsSegmentedAsync(continuationToken);

            Response response = new Response();
            response.Data = new Data();
            response.Data.Meta = new Bee.BeeDirectory();
            response.Data.Meta = await Common.GetBeeDirectoryInfo(directory);

            response.Data.Items = new List<object>();

            foreach (IListBlobItem item in resultSegment.Results)
            {
                if (typeof(CloudBlockBlob) == item.GetType())
                {
                    response.Data.Items.Add(await Common.GetBeeFileInfo((item as CloudBlockBlob), _thumbnailContainer));
                }
                else if (typeof(CloudBlobDirectory) == item.GetType())
                {
                    response.Data.Items.Add(await Common.GetBeeDirectoryInfo((item as CloudBlobDirectory)));
                }
            }

            response.Status = "success";
            return response;
        }

        public async Task<Response> CreateDirectory(string prefix)
        {
            BlobContinuationToken continuationToken = null;

            CloudBlobDirectory directory = _imageContainer.GetDirectoryReference(prefix);
            BlobResultSegment resultSegment = await directory.ListBlobsSegmentedAsync(continuationToken);

            Response response = new Response();
            response.Data = new Data();
            response.Data.Meta = new Bee.BeeDirectory();
            response.Data.Meta = await Common.GetBeeDirectoryInfo(directory);

            response.Status = "success";
            return response;
        }

        public Response DeleteDirectory(string prefix)
        {
            Response response = new Response();

            response.Status = "success";
            return response;
        }

        public async Task<Response> GetFileMetadata(string file)
        {
            CloudBlockBlob cloudBlockBlob = _imageContainer.GetBlockBlobReference(file);
            await cloudBlockBlob.FetchAttributesAsync();

            Response response = new Response();
            response.Data = new Data();
            response.Data.Meta = new Bee.BeeFile();
            response.Data.Meta = await Common.GetBeeFileInfo(cloudBlockBlob, _thumbnailContainer);

            response.Status = "success";
            return response;
        }

        public async Task<Response> UploadFile(string fileSource, string fileDestination)
        {
            WebClient webClient = new WebClient();
            MemoryStream imageStream = new MemoryStream(webClient.DownloadData(fileSource));
            CloudBlockBlob imageBlockBlob = _imageContainer.GetBlockBlobReference(fileDestination);

            // Make all thumbnails png for consistancy
            CloudBlockBlob thumbnailBlockBlob = _thumbnailContainer.GetBlockBlobReference(Common.ReplaceUrlExtension(fileDestination, "png"));

            await imageBlockBlob.UploadFromStreamAsync(imageStream);

            // Reset stream position to start
            imageStream.Position = 0;

            using (Image<Rgba32> image = Image.Load<Rgba32>(imageStream))
            {
                image.Mutate(x => x.Resize(200, 200));

                MemoryStream thumbnailStream = new MemoryStream();

                image.Save(thumbnailStream, new PngEncoder());

                // Reset stream position to start
                thumbnailStream.Position = 0;
                
                await thumbnailBlockBlob.UploadFromStreamAsync(thumbnailStream);
            }

            Response response = new Response();
            response.Data = new Data();
            response.Data.Meta = new Bee.BeeFile();
            response.Data.Meta = await Common.GetBeeFileInfo(imageBlockBlob, _thumbnailContainer);

            response.Status = "success";
            return response;
        }

        public async Task<Response> DeleteFile(string file)
        {
            CloudBlockBlob cloudBlockBlob = _imageContainer.GetBlockBlobReference(file);
            await cloudBlockBlob.DeleteIfExistsAsync();

            Response response = new Response();

            response.Status = "success";
            return response;
        }
    }
}
