﻿using Microsoft.AspNetCore.Http;

namespace AzureBlobStorage.Services
{
    public interface IBlobService
    {
        Task<string> GetBlob(string name, string containerName);
        Task<List<string>> GetAllBlobs(string containerName); 
        Task<bool> UploadBlob(string name,IFormFile file, string containerName, Models.BlobModel model);
        Task<bool> DeleteBlob(string name, string containerName);
    }
}
