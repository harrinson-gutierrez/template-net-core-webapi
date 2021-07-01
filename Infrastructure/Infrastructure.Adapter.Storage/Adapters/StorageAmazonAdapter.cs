using System.Threading.Tasks;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Infrastructure.Adapter.Storage.Interfaces;
using Infrastructure.Adapter.Storage.Models;
using Domain.Settings;

namespace Infrastructure.Adapter.Storage.Adapters
{
    public class StorageAmazonAdapter : IStorageService
    {
        private readonly IAmazonS3 Client;
        private readonly IOptions<StorageOptions> StorageConfiguration;
        private readonly ILogger<StorageAmazonAdapter> Logger;
        public StorageAmazonAdapter(IOptions<StorageOptions> storageConfiguration,
                                  IAmazonS3 client,
                                  ILogger<StorageAmazonAdapter> logger)
        {
            Client = client;
            StorageConfiguration = storageConfiguration;
            Logger = logger;
        }

        public async Task<StorageResponse> UploadObject(StorageRequest storageRequest)
        {

            byte[] fileBytes = new Byte[storageRequest.File.Length];
            storageRequest.File.OpenReadStream().Read(fileBytes, 0, Int32.Parse(storageRequest.File.Length.ToString()));

            var fileNameTemp = storageRequest.FileName ?? Guid.NewGuid() + storageRequest.File.FileName.Trim();

            PutObjectResponse response = null;

            using (var stream = new MemoryStream(fileBytes))
            {
                var request = new PutObjectRequest
                {
                    BucketName = StorageConfiguration.Value.Bucket,
                    Key = (storageRequest.FolderName ?? "") + fileNameTemp,
                    InputStream = stream,
                    ContentType = storageRequest.File.ContentType,
                    CannedACL = S3CannedACL.PublicRead   
                };

                response = await Client.PutObjectAsync(request);
            }

            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                var route = string.IsNullOrEmpty(StorageConfiguration.Value.CloudFront) ? string.Format(StorageConfiguration.Value.EndpointAmazon, StorageConfiguration.Value.Bucket) : StorageConfiguration.Value.CloudFront;
                return new StorageResponse
                {
                    Success = true,
                    FileName = route + (storageRequest.FolderName ?? "") + fileNameTemp
                };
            }
            else
            {
                Logger.LogInformation("Not upload storage object");
                return new StorageResponse
                {
                    Success = false,
                    FileName = storageRequest.FileName
                };
            }
         }

        public async Task<StorageResponse> RemoveObject(StorageDeleteRequest storageDeleteRequest)
        {
            var request = new DeleteObjectRequest
            {
                BucketName = StorageConfiguration.Value.Bucket,
                Key = storageDeleteRequest.FileName
            };

            var response = await Client.DeleteObjectAsync(request);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new StorageResponse
                {
                    Success = true,
                    FileName = storageDeleteRequest.FileName
                };
            }
            else
            {
                Logger.LogInformation("Not upload storage object");
                return new StorageResponse
                {
                    Success = false,
                    FileName = storageDeleteRequest.FileName
                };
            }
        }

    }
}

