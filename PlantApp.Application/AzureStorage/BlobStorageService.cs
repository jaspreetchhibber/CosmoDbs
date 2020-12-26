using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace PlantApp.Application.AzureStorage
{
    public class BlobStorageService : IBlobStorageService
    {
        private IConfiguration _configuration;


        public BlobStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> UploadFileToBlobAsync(IFormFile file)
        {
            try
            {
                string url = string.Empty;

                if (file != null)
                {
                    var accessKey = _configuration.GetSection("Storage").GetSection("BlobStorageKey").Value;
                    CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(accessKey);
                    CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
                    string strContainerName = "data";
                    CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

                    string filename = GenerateFileName(file);
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
                    cloudBlockBlob.Properties.ContentType = file.ContentType;

                    using (Stream stream = file.OpenReadStream())
                    {
                        await cloudBlockBlob.UploadFromByteArrayAsync(ReadFully(stream, cloudBlockBlob.StreamWriteSizeInBytes),
                    0, (int)stream.Length);
                    }

                     url = GetBlobSasUri(cloudBlockBlob);

                }
                return url;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        static byte[] ReadFully(Stream input, int size)
        {
            byte[] buffer = new byte[size];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, size)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private static string GenerateFileName(IFormFile file)
        {

            FileInfo fi = new FileInfo(file.FileName);
            Guid guid = Guid.NewGuid();
            var filename = guid.ToString() + fi.Extension;
            return filename;
        }


        private static string GetBlobSasUri(CloudBlockBlob blob)
        {
            try
            {
                //Set the expiry time and permissions for the blob.
                //In this case, the start time is specified as a few minutes in the past, to mitigate clock skew.
                //The shared access signature will be valid immediately.
                SharedAccessBlobPolicy sasConstraints = new SharedAccessBlobPolicy
                {
                    SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5),
                    SharedAccessExpiryTime = DateTimeOffset.UtcNow.AddHours(24),
                    Permissions = SharedAccessBlobPermissions.Read
                };
                //Generate the shared access signature on the blob, setting the constraints directly on the signature.
                string sasBlobToken = blob.GetSharedAccessSignature(sasConstraints);

                //Return the URI string for the container, including the SAS token.
                return blob.StorageUri.PrimaryUri.OriginalString + sasBlobToken;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
