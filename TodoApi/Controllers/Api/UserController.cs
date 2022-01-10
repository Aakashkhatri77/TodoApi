using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Context;
using TodoApi.Models;

namespace TodoApi.Controllers.Api
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly TodoDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UserController(TodoDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        //Create User
        [HttpPost]
        public async Task<IActionResult> Register([FromForm] User user)
        {
            if (ModelState.IsValid == true)
            {
                string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                string extension = Path.GetExtension(user.ImageFile.FileName);
                user.Profile = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                IFormFile postedFile = user.ImageFile;
                long length = postedFile.Length;
                if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                {
                    if (length <= 1000000)
                    {

                        string path = _hostEnvironment.WebRootPath + "\\Images\\";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (FileStream filestream = System.IO.File.Create(path + user.ImageFile.FileName))
                        {
                            await user.ImageFile.CopyToAsync(filestream);

                        }
                        _context.Users.Add(user);
                        _context.SaveChangesAsync();

                    }
                }
            }
            return Ok(user);



        }

        //Display User
        [HttpGet]
        public async Task<IActionResult> Display()
        {
            var user = await _context.Users.ToListAsync();
            return Ok(user);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetDisplay(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return Ok(user);
        }



        //Update User
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromForm] User user, int? id)
        {
            var _user = await _context.Users.FindAsync(id);
            var oldImagePath = _user.Profile;
            if (ModelState.IsValid == true)
            {
                if (user.ImageFile != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                    string extension = Path.GetExtension(user.ImageFile.FileName);
                    user.Profile = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    IFormFile postedFile = user.ImageFile;
                    long length = postedFile.Length;
                    if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" || extension.ToLower() == ".jpeg")
                    {
                        if (length <= 1000000)
                        {
                            string path = _hostEnvironment.WebRootPath + "\\Images\\";
                            using (FileStream filestream = System.IO.File.Create(path + user.ImageFile.FileName))
                            {
                                await user.ImageFile.CopyToAsync(filestream);
                            }
                            _user.Name = user.Name;
                            _user.Email = user.Email;
                            _user.Address = user.Address;
                            _user.Contact = user.Contact;
                            _user.Profile = user.ImageFile.FileName.ToString();
                            _context.Entry(_user).State = EntityState.Modified;
                            var UserNo = await _context.SaveChangesAsync();
                            string imgPath = Path.Combine(path, oldImagePath.ToString());
                            if (UserNo > 0)
                            {
                                if (System.IO.File.Exists(imgPath))
                                {
                                    System.IO.File.Delete(imgPath);
                                }
                            }
                        }
                    }
                }
                else
                {
               /*     _user.Name = user.Name;
                    _user.Email = user.Email;
                    _user.Address = user.Address;
                    _user.Contact = user.Contact;*/
                    user.Profile = oldImagePath.ToString();
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }
            return Ok(user);
        }

        //Delete User
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                string path = _hostEnvironment.WebRootPath + "\\Images\\";
                var image = user.Profile.ToString();
                var imgPath = Path.Combine(path, image);

                if (System.IO.File.Exists(imgPath))
                {
                    System.IO.File.Delete(imgPath);
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

    }
}
    