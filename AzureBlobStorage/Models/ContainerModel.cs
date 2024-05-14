using System.ComponentModel.DataAnnotations;

namespace AzureBlobStorage.Models
{
    public class ContainerModel
    {
        [Required]
        public string Name { get; set; }
    }
}
