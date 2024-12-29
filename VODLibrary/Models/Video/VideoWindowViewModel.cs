using System.ComponentModel.DataAnnotations;

namespace VODLibrary.Models.Video
{
    public class VideoWindowViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public DateTime Uploaded { get; set; }


        public string? ImagePath { get; set; }

    }
}
