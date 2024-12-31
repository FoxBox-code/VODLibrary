using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VODLibrary.Data;
using VODLibrary.Data.Models;
using VODLibrary.Models;
using VODLibrary.Models.Video;

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
                    Uploaded = c.Uploaded,
                    Likes = c.Likes,
                    DisLikes = c.DisLikes,
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

            model.Uploaded = DateTime.Now;

            var comment = new Comment
            {
                UserName = model.UserName,
                Description = model.Description,
                VideoRecordId = model.VideoRecordId,
                Uploaded = model.Uploaded,
                Likes = model.Likes,
                DisLikes = model.DisLikes,
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpGet("replies/{commentId}")]
        public async Task<IActionResult> GetReplies(int commentId)
        {
            var replies = await _context.Replies
                .Where(r => r.CommentId == commentId)
                .Select(r => new ReplyViewModel
                {
                    Id = r.Id,
                    UserName = r.UserName,
                    Description = r.Description,
                    CommentId = r.CommentId,
                    Uploaded = r.Uploaded,
                    Likes = r.Likes,
                    DisLikes = r.DisLikes,
                    VideoRecordId = r.VideoRecordId,
                }).ToListAsync();

            return Ok(replies);
        }


        [HttpPost("replies")]
        public async Task<IActionResult> PostReply([FromBody] ReplyViewModel replyModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == replyModel.CommentId);

            if (comment == null)
            {
                return NotFound($"Comment with ID {replyModel.CommentId} not found.");
            }

            var reply = new Reply
            {
                UserName = replyModel.UserName,
                Description = replyModel.Description,
                CommentId = replyModel.CommentId,
                Uploaded = replyModel.Uploaded == default ? DateTime.Now : replyModel.Uploaded, // Default to DateTime.Now
                Likes = replyModel.Likes,
                DisLikes = replyModel.DisLikes,
                VideoRecordId = comment.VideoRecordId
            };

            await _context.Replies.AddAsync(reply);
            await _context.SaveChangesAsync();

            return Ok(reply);
        }
    }
}
