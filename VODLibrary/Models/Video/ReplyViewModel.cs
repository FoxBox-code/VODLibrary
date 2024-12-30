using System.ComponentModel.DataAnnotations;
using static VODLibrary.DataConstants.ConstantsCharacteristics.CommentConstants;

namespace VODLibrary.Models.Video
{
    public class ReplyViewModel
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required]
        [MaxLength(CommentDescriptionMaxLength)]
        public string Description { get; set; }

        public int CommentId { get; set; }

        public DateTime Uploaded { get; set; }

        public int Likes { get; set; }

        public int DisLikes { get; set; }

        public int VideoRecordId { get; set; }
    }
}
