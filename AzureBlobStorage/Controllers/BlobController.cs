using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Controllers
{
    public class BlobController : Controller
    {
        private readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            this._blobService = blobService;
        }

        public async Task<IActionResult> Manage(string containerName)
        {
            var blob = await _blobService.GetAllBlobs(containerName);
            return View(blob);
        }

        [HttpGet]
        public IActionResult AddFile(string containerName)
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string name, string containerName)
        {
            return Redirect(await _blobService.GetBlob(name, containerName));
        }


        [HttpGet]
        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            await _blobService.DeleteBlob(name, containerName);
            return RedirectToAction("Index", "Container");
        }


        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile file, string containerName)
        {
            if(file == null || file.Length <= 0) return View();
            var fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName);

            var result = await _blobService.UploadBlob(fileName, file, containerName);

            if(result)
                return RedirectToAction("Index", "Container");

            return View();
        }
    }
}
