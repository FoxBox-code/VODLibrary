using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VODLibrary.Data;
using VODLibrary.Data.Models;
using VODLibrary.Models;

namespace VODLibrary.Controllers.Api
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{videoId}")]
        public async Task<IActionResult> GetComments(int videoId)
        {
            List<CommentViewModel> comments = await _context
                .Comments
                .Where(c => c.VideoRecordId == videoId)
                .Select(c => new CommentViewModel()
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Description = c.Description,
                    VideoRecordId = c.VideoRecordId,
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> AddComments([FromBody] CommentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = new Comment
            {
                UserName = model.UserName,
                Description = model.Description,
                VideoRecordId = model.VideoRecordId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }
    }
}
