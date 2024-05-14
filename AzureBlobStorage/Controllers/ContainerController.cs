using AzureBlobStorage.Models;
using AzureBlobStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace AzureBlobStorage.Controllers
{
    public class ContainerController : Controller
    {
        private readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService)
        {
            this._containerService = containerService;
        }

        public async Task<IActionResult> Index()
        {
            var allContainers =await _containerService.GetAllContainers();
            return View(allContainers);
        }

        public async Task<IActionResult> Delete(string name)
        {
            await _containerService.DeleteContainer(name);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View(new ContainerModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContainerModel container)
        {
            await _containerService.CreateContainer(container.Name);
            return RedirectToAction(nameof(Index));
        }
    }
}
