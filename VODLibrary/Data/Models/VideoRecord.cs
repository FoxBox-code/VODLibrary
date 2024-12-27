using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static VODLibrary.DataConstants.ConstantsCharacteristics.VideoRecordsConstants;

namespace VODLibrary.Data.Models
{
    public class VideoRecord
    {
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "{0} must be between {2} and {1} charachters long")]
        public string Title { get; set; }

        [Required]
        public DateTime Uploaded { get; set; }

        [StringLength(DescriptionMaxLength, ErrorMessage = "The description cannot be longer than 5000 charachters")]
        public string? Description { get; set; }

        [Required]
        [Range(0, MaxVideoTicks, ErrorMessage = "Video must be between 0 or 24 hours")]
        public TimeSpan Length { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int Views { get; set; }

        public int Likes { get; set; }

        public int Dislike { get; set; }

        [Required]
        public string VideoOwnerId { get; set; }

        public IdentityUser VideoOwner { get; set; }

        public string VideoPath { get; set; } //this is the URL for the video that will be in the DataBase

        public string ImagePath { get; set; } // this will provide thumbnail picture 

    }
}
