using System.ComponentModel.DataAnnotations;

namespace DawaaNeo.Services
{
    public class CreateServiceDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string ArTitle { get; set; }
        [Required]
        public bool IsActive { get; set; }
        [Required]
        public string Blop { get; set; }
        [Required]
        public string FileType { get; set; }
        [Required]
        public string FileName { get; set; }
        [Required]
        public int FileSize { get; set; }
        [Required]
        public bool IsIconUpdated { get; set; }
    }
}
