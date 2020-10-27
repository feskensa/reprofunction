using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Repro.Functions.Storage
{
    [ExcludeFromCodeCoverage]
    public class BlobStorage : IStorage
    {
        public readonly CloudBlobClient _cloudBlobClient;

        public BlobStorage(CloudBlobClient cloudBlobClient)
        {
            _cloudBlobClient = cloudBlobClient;
        }

        public async Task<string> DownloadTextAsync(string blobRef)
        {
            var cloudBlockBlob = new CloudBlockBlob(new Uri(blobRef), _cloudBlobClient.Credentials);

            return await cloudBlockBlob.DownloadTextAsync();
        }
    }
}
