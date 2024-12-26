namespace VODLibrary.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<VideoRecord> Videos { get; set; }
    }
}
