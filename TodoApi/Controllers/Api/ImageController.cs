using Microsoft.AspNetCore.Mvc;
using TodoApi.Context;
using TodoApi.Models;

namespace TodoApi.Controllers.Api
{

    [ApiController]
    [Route("api/[controller]")]
    public class ImageController : ControllerBase
    {
        private readonly TodoDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageController(TodoDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Image image)
        {
            try
            {
                if (image.files.Length > 0)
                {
                    string path = _hostEnvironment.WebRootPath + "\\Images\\";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using (FileStream fileStream = System.IO.File.Create(path + image.files.FileName))
                    {
                        image.files.CopyToAsync(fileStream);
                        return Ok();
                    }
                   
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);  
            }
            
        }
    }
}
