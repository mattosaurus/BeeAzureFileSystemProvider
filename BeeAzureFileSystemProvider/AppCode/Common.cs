using BeeFreeAzureFileSystemProvider.Models.Bee;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using System.IO;
using HeyRed.Mime;

namespace BeeFreeAzureFileSystemProvider.AppCode
{
    public static class Common
    {
        public static bool IsAuthorized(string username, string password, string connectionString)
        {
            bool authorized = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("sp__check_file_service_provider_authorization"))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.Connection = connection;

                    connection.Open();

                    authorized = (bool)command.ExecuteScalar();

                    connection.Close();
                }
            }

            return authorized;
        }

        public async static Task<BeeFile> GetBeeFileInfo(CloudBlockBlob cloudBlockBlob, CloudBlobContainer thumbnailContainer)
        {
            BeeFile beeFile = new BeeFile();

            beeFile.Name = GetUrlFileName(cloudBlockBlob.Name);

            beeFile.Path = "/" + cloudBlockBlob.Parent.Prefix + cloudBlockBlob.Name;
            beeFile.LastModified = cloudBlockBlob.Properties.LastModified.Value.ToUnixTimeMilliseconds();
            beeFile.Size = cloudBlockBlob.Properties.Length;
            beeFile.Permissions = "rw";
            beeFile.PublicUrl = cloudBlockBlob.Uri.ToString();
            beeFile.Extra = new object();

            MimeTypesMap.AddOrUpdate("image/jpeg", "jpg");
            beeFile.MimeType = MimeTypesMap.GetMimeType(beeFile.Path);

            CloudBlockBlob thumbnailBlockBlob = thumbnailContainer.GetBlockBlobReference(Common.ReplaceUrlExtension(cloudBlockBlob.Name, "png"));

            if (await thumbnailBlockBlob.ExistsAsync())
            {
                beeFile.Thumbnail = thumbnailBlockBlob.Uri.ToString();
            }

            return beeFile;
        }

        public async static Task<BeeDirectory> GetBeeDirectoryInfo(CloudBlobDirectory cloudBlobDirectory)
        {
            BlobResultSegment resultSegment = await cloudBlobDirectory.ListBlobsSegmentedAsync(true, BlobListingDetails.None, null, new BlobContinuationToken(), null, null);
            BeeDirectory beeDirectory = new BeeDirectory();

            beeDirectory.MimeType = "application/directory";

            if (cloudBlobDirectory.Parent == null)
            {
                beeDirectory.Name = "root";
            }
            else
            {
                if (!string.IsNullOrEmpty(cloudBlobDirectory.Parent.Prefix))
                {
                    beeDirectory.Name = cloudBlobDirectory.Prefix.Replace(cloudBlobDirectory.Parent.Prefix, "").Replace("/", "");
                }
                else
                {
                    beeDirectory.Name = cloudBlobDirectory.Prefix.Replace("/", "");
                }
            }

            beeDirectory.Path = "/" + cloudBlobDirectory.Prefix;

            if (resultSegment.Results.Count() > 0)
            {
                beeDirectory.LastModified = resultSegment.Results.Where(x => x.Parent.Prefix.StartsWith(cloudBlobDirectory.Prefix)).Select((y => (y as CloudBlockBlob).Properties.LastModified.Value.ToUnixTimeMilliseconds())).Max();
                beeDirectory.Size = resultSegment.Results.Where(x => x.Parent.Prefix.StartsWith(cloudBlobDirectory.Prefix)).Select(y => (y as CloudBlockBlob).Properties.Length).Sum();
                beeDirectory.ItemCount = resultSegment.Results.Where(x => x.Parent.Prefix.StartsWith(cloudBlobDirectory.Prefix)).Count();
            }
            else
            {
                beeDirectory.LastModified = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
                beeDirectory.Size = 0;
                beeDirectory.ItemCount = 0;
            }

            
            beeDirectory.Permissions = "rw";
            beeDirectory.Extra = new object();

            return beeDirectory;
        }

        public static string ReplaceUrlExtension(string url, string newExtension)
        {
            string newUrl;

            // Split on "?" to remove parameters
            string[] urlParts = url.Split("?");

            // First part should contain the path
            newUrl = urlParts[0].Substring(0, urlParts[0].LastIndexOf("."));

            // If new extension doesn't begin with "." then add
            if (!newExtension.StartsWith("."))
            {
                newExtension = "." + newExtension;
            }

            // Append new extension
            newUrl += newExtension;

            return newUrl;
        }

        public static string GetUrlFileName(string url)
        {
            string[] fileUrlParts = url.Split("/");

            string filename = fileUrlParts[fileUrlParts.Length - 1];

            return filename;
        }
    }
}
