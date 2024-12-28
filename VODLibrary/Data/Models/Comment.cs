using System.ComponentModel.DataAnnotations;
using static VODLibrary.DataConstants.ConstantsCharacteristics.CommentConstants;

namespace VODLibrary.Data.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required]
        [MaxLength(CommentDescriptionMaxLength)]
        public string Description { get; set; }

        public int VideoRecordId { get; set; }

        public VideoRecord VideoRecord { get; set; }
    }
}
