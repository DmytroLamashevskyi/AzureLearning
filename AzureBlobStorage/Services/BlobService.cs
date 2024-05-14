
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorage.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(BlobServiceClient blobClient)
        {
            this._blobClient = blobClient;
        }

        public async Task<bool> DeleteBlob(string name, string containerName)
        {
            var client = _blobClient.GetBlobContainerClient(containerName);
            var blob = client.GetBlobClient(name);
            return await blob.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            var client = _blobClient.GetBlobContainerClient(containerName);
            var blobs = client.GetBlobsAsync();
            var blobsList =  new List<string>();

            await foreach (var blob in blobs)
            {
                blobsList.Add(blob.Name);
            }

            return blobsList;
        }

        public async Task<string> GetBlob(string name, string containerName)
        {
            var client = _blobClient.GetBlobContainerClient(containerName);
            var blob = client.GetBlobClient(name);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<bool> UploadBlob(string name, IFormFile file, string containerName)
        {
            var client = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = client.GetBlobClient(name);
            var httpheaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType
            };
            var result = await blobClient.UploadAsync(file.OpenReadStream(), httpheaders);

            return result != null;
        }
    }
}
