namespace VODLibrary.Models.Video
{
    public class VideoCategoryViewModel
    {
        public int Id { get; set; }

        public string CategoryTitle { get; set; }

        public IEnumerable<VideoWindowViewModel> Videos { get; set; } = new List<VideoWindowViewModel>();

    }
}
