using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using static VODLibrary.DataConstants.ConstantsCharacteristics.CommentConstants;

namespace VODLibrary.Data.Models
{
    public class Reply
    {
        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required]
        [MaxLength(CommentDescriptionMaxLength)]
        public string Description { get; set; }

        public int CommentId { get; set; }

        [JsonIgnore] // Ignore during serialization
        public Comment Comment { get; set; }

        public DateTime Uploaded { get; set; } = DateTime.Now;

        public int Likes { get; set; }

        public int DisLikes { get; set; }

        public int VideoRecordId { get; set; }
    }
}
