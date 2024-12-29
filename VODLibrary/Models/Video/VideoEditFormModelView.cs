using System.ComponentModel.DataAnnotations;
using static VODLibrary.DataConstants.ConstantsCharacteristics.VideoRecordsConstants;

namespace VODLibrary.Models.Video
{
    public class VideoEditFormModelView
    {
        public int Id { get; set; }

        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "{0} must be between {2} and {1} charachters long")]
        public string Title { get; set; }


        [StringLength(DescriptionMaxLength, ErrorMessage = "Description can hold maximum of 5000 charachters")]
        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public string? ImagePath { get; set; }

        public IFormFile? ThumbnailFile { get; set; }



    }
}
