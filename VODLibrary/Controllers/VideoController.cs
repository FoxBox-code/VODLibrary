using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VODLibrary.Data;
using VODLibrary.Data.Models;
using VODLibrary.Models.Video;

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
                    ImagePath = v.ImagePath,
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
        [RequestSizeLimit(100_000_000)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)]
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

            var thumbnailFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ImageFile.FileName)}";
            string thumnailPath = Path.Combine(uploadPath, thumbnailFileName);

            using (FileStream stream = new FileStream(thumnailPath, FileMode.Create))
            {
                await model.ImageFile.CopyToAsync(stream);
            }

            var video = new VideoRecord()
            {
                Title = model.Title,
                CategoryId = model.CategoryId,
                VideoPath = $"/uploads/{fileName}",
                ImagePath = $"/uploads/{thumbnailFileName}",
                Uploaded = DateTime.Now,
                VideoOwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            await _dbContext.VideoRecords.AddAsync(video);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Home));
        }

        public async Task<IActionResult> Mine()
        {
            IEnumerable<VideoWindowViewModel> videoCollection = await _dbContext
                .VideoRecords
                .Include(v => v.VideoOwner)
                .Where(v => v.VideoOwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .Select(v => new VideoWindowViewModel()
                {
                    Id = v.Id,
                    Title = v.Title,
                    OwnerName = v.VideoOwner.UserName,
                    Uploaded = v.Uploaded,
                    ImagePath = v.ImagePath,

                })
                .ToListAsync();

            return View(videoCollection);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            VideoRecord? selectedVod = await _dbContext
                .VideoRecords
                .Where(v => v.Id == id && v.VideoOwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .FirstOrDefaultAsync();

            if (selectedVod == null)
            {
                return BadRequest();
            }

            VideoEditFormModelView model = new VideoEditFormModelView()
            {
                Id = selectedVod.Id,
                Title = selectedVod.Title,
                Description = selectedVod.Description,
                CategoryId = selectedVod.CategoryId,
                ImagePath = selectedVod.ImagePath,
            };

            ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        [RequestSizeLimit(100_000_000)] // 100 MB
        [RequestFormLimits(MultipartBodyLengthLimit = 100_000_000)]
        public async Task<IActionResult> Edit(int id, VideoEditFormModelView model)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");
                return View(model);
            }

            VideoRecord? selectedVod = await _dbContext
                .VideoRecords
                .Where(v => v.Id == id && v.VideoOwnerId == User.FindFirstValue(ClaimTypes.NameIdentifier))
                .FirstOrDefaultAsync();

            if (selectedVod == null)
            {
                return BadRequest();
            }

            selectedVod.Title = model.Title;
            selectedVod.Description = model.Description;
            selectedVod.CategoryId = model.CategoryId;


            if (model.ThumbnailFile != null)
            {
                string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadPath);

                string thumbnailFileName = $"{Guid.NewGuid()}{Path.GetExtension(model.ThumbnailFile.FileName)}";
                string thumbnailUploadPath = Path.Combine(uploadPath, thumbnailFileName);

                using (FileStream stream = new FileStream(thumbnailUploadPath, FileMode.Create))
                {
                    await model.ThumbnailFile.CopyToAsync(stream);
                }

                selectedVod.ImagePath = $"/uploads/{thumbnailFileName}";

            }

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Mine));
        }

        public async Task<IActionResult> Play(int Id)
        {
            VideoRecord? selectedVideo = await _dbContext
                .VideoRecords
                .Include(v => v.VideoOwner)
                .FirstOrDefaultAsync(v => v.Id == Id);

            if (selectedVideo == null)
            {
                return BadRequest();
            }

            selectedVideo.Views++;
            await _dbContext.SaveChangesAsync();

            VideoPlayModelView model = new VideoPlayModelView()
            {
                Id = selectedVideo.Id,
                Title = selectedVideo.Title,
                Uploaded = selectedVideo.Uploaded,
                OwnerName = selectedVideo.VideoOwner.UserName,
                VideoPath = selectedVideo.VideoPath,
                Views = selectedVideo.Views,
                Likes = selectedVideo.Likes,
                Dislikes = selectedVideo.Dislike,
            };

            return View(model);
        }
    }
}
