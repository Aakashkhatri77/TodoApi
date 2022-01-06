using System.ComponentModel.DataAnnotations.Schema;

namespace TodoApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Profile { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

    }
}
