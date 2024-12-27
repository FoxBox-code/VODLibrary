using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VODLibrary.Data;
using VODLibrary.Data.Models;
using VODLibrary.Models;

namespace VODLibrary.Controllers
{
    public class VideoController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public VideoController(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Home()
        {
            IEnumerable<VideoWindowViewModel> videos = await _dbContext
                .VideoRecords
                .Include(v => v.VideoOwner)
                .Select(v => new VideoWindowViewModel()
                {
                    Id = v.Id,
                    Title = v.Title,
                    Uploaded = v.Uploaded,
                    OwnerName = v.VideoOwner.UserName,
                    VideoPath = v.VideoPath,
                })
                .ToListAsync();

            return View(videos);
        }

        [HttpGet]
        public async Task<IActionResult> Upload()
        {
            ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(VideoUploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");
                return View(model);
            }

            var uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadPath);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.VideoFile.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.VideoFile.CopyToAsync(stream);
            }

            var video = new VideoRecord()
            {
                Title = model.Title,
                CategoryId = model.CategoryId,
                VideoPath = $"/uploads/{fileName}",
                Uploaded = DateTime.Now,
                VideoOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            await _dbContext.VideoRecords.AddAsync(video);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");


        }
    }
}
