using ImageTest1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ImageTest1.Controllers
{
    [Route("Images")]
    public class ImageController : Controller
    {
        private readonly LaptopDBContext _context;
        private readonly IWebHostEnvironment environment;

        public ImageController(LaptopDBContext context, IWebHostEnvironment environment)
        {
            this._context = context;
            this.environment = environment;
        }

        [HttpGet]
        public IEnumerable<Laptop> Index()
        {
            var data = _context.Laptops.ToList();
            return data;
        }

        [HttpPost]
        public IActionResult AddLaptop(Laptop model)
        {
            string uniqueFileName = UploadImage(model);
            var data = new Laptop()
            {
                Brand = model.Brand,
                Description = model.Description,
                Path = uniqueFileName,
            };

            var Adddata = _context.Laptops.Add(data);
            _context.SaveChanges();
            return Ok(Adddata.Entity);
        }

        private string UploadImage(Laptop model)
        {
            string uniqueFileName = string.Empty;
            if (model.ImagePath != null)
            {
                string uploadFolder = Path.Combine(environment.WebRootPath, "Content");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.ImagePath.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        [HttpGet("{id}")]
        public IActionResult GetImageBuId(int id)
        {
            var data = _context.Laptops.Where(e => e.Id == id).SingleOrDefault();

            if(data != null)
            {
                string findFromFolder = Path.Combine(environment.WebRootPath,"Content");
                string currentImage = Path.Combine(Directory.GetCurrentDirectory(),findFromFolder,data.Path);
                if (System.IO.File.Exists(currentImage))
                {
                    var imageBytes = System.IO.File.ReadAllBytes(currentImage);
                    return File(imageBytes, "image/jpeg");
                }
            }

            return NotFound();
        }

        [HttpGet]
        [Route("AllImages")]
        public IActionResult GetAllImages()
        {
            string findFromFolder = Path.Combine(environment.WebRootPath, "Content");

            var imageFiles = Directory.GetFiles(findFromFolder);

            var imageInfoList = imageFiles.Select(filePath => new
            {
                Name = Path.GetFileName(filePath),
                Format = Path.GetExtension(filePath).ToLower()
            });

            return Ok(imageInfoList);
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if(id == 0)
            {
                return NotFound();
            }
            else
            {
                var data = _context.Laptops.Where(e => e.Id == id).SingleOrDefault();
                if(data != null)
                {
                    string deleteFromFolder = Path.Combine(environment.WebRootPath,"Content");
                    string currentImage = Path.Combine(Directory.GetCurrentDirectory(),deleteFromFolder,data.Path);

                    if(currentImage != null)
                    {
                        if (System.IO.File.Exists(currentImage))
                        {
                            System.IO.File.Delete(currentImage);
                        }
                    }

                    _context.Laptops.Remove(data);
                    _context.SaveChanges();
                    return Ok("Delete Successfully");
                }
            }
            return BadRequest();
        }
    }
}
