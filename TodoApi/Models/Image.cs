using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class Image
    {
        
        public string Id { get; set; }
        public string file { get; set; }
        [NotMapped]
        public IFormFile files { get; set; }

    }
}
