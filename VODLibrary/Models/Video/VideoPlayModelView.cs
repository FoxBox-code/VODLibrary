namespace VODLibrary.Models.Video
{
    public class VideoPlayModelView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Uploaded { get; set; }
        public string OwnerName { get; set; }
        public string VideoPath { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
