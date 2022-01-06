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
        public async Task<IActionResult> Register([FromForm]User user)
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
                        var file = user.ImageFile;
                        var _user = new User()
                        {
                            Name = user.Name,
                            Email = user.Email,
                            Address = user.Address,
                            Contact = user.Contact,
                            Profile = file.ToString()
                        };
                        _context.Users.Add(_user);
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

        //Update User
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(User user, int? id)
        {
            if (ModelState.IsValid == true)
            {
                var _user = await _context.Users.FindAsync(id);
                var oldImgPath = user.Profile; 
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
                            _context.Entry(user).State = EntityState.Modified;
                            string imgPath = Path.Combine(path, oldImgPath.ToString());
                            var User = await _context.SaveChangesAsync();

                            if (User > 0)
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
                    user.Profile = oldImgPath.ToString();
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }




               /* _user.Name = user.Name;
                    _user.Email = user.Email;
                    _user.Address = user.Address;
                    _user.Contact = user.Contact;
                    _context.Users.Update(_user);
                    await _context.SaveChangesAsync();*/
             
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
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }


        public async Task<string> UploadFile(IFormFile file)
        {
                string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                string path = _hostEnvironment.WebRootPath + "\\Images\\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                var filename = Guid.NewGuid() + extension;
                var filePath = Path.Combine(path, fileName);
                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);
                return fileName;
        }

    }


}
