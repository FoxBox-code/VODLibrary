using System.ComponentModel.DataAnnotations;
using static VODLibrary.DataConstants.ConstantsCharacteristics.VideoRecordsConstants;

namespace VODLibrary.Models.Video
{
    public class VideoUploadViewModel
    {
        [Required]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "{0} must be between {2} and {1} charachters long")]
        public string Title { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Please select a video file")]
        [DataType(DataType.Upload)]
        public IFormFile VideoFile { get; set; }

        [Required(ErrorMessage = "Please select a image for your video")]
        [DataType(DataType.Upload)]
        public IFormFile ImageFile { get; set; }

    }
}
