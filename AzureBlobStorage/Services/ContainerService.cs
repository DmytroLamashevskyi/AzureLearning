
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorage.Services
{
    public class ContainerService : IContainerService
    {
        private readonly BlobServiceClient _blobClient; 
        public ContainerService(BlobServiceClient blobClient)
        {
            this._blobClient = blobClient;
        }

        public async Task CreateContainer(string containerName)
        {
            var client = _blobClient.GetBlobContainerClient(containerName);
            await client.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            var client = _blobClient.GetBlobContainerClient(containerName);
            await client.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainers()
        {
            var containerNames = new List<string>();
            await foreach(var blobContainerItem in _blobClient.GetBlobContainersAsync())
            {
                containerNames.Add(blobContainerItem.Name);
            }

            return containerNames;
        }

        public async Task<List<string>> GetAllContainersAndBlobs()
        {
            var blobContainers = new List<string>();
            blobContainers.Add("Account Name: " + _blobClient.AccountName);
            blobContainers.Add("-----------------------------------------------------------------------------");
            await foreach(var container in _blobClient.GetBlobContainersAsync())
            {
                blobContainers.Add("--" + container.Name);
                var blobContainer = _blobClient.GetBlobContainerClient(container.Name);

                await foreach(var blob in blobContainer.GetBlobsAsync())
                {
                    var blobClient = blobContainer.GetBlobClient(blob.Name);
                    var blobProperties =await blobClient.GetPropertiesAsync();
                    string blobToAdd = blob.Name;
                    if(blobProperties.Value.Metadata.ContainsKey("title"))
                    {
                        blobToAdd += "(" + blobProperties.Value.Metadata["title"] + ")";
                    }
                    blobContainers.Add("------" + blobToAdd);
                }
                blobContainers.Add("-----------------------------------------------------------------------------");
            }
            return blobContainers;
        }
    }
}
