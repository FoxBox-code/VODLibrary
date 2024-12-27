using System.ComponentModel.DataAnnotations;
using static VODLibrary.DataConstants.ConstantsCharacteristics.ChategoryConstants;

namespace VODLibrary.Data.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(CategoryNameMaxLength, ErrorMessage = "{0} must contain a max of {1} symbols")]
        public string Name { get; set; }

        public IEnumerable<VideoRecord> Videos { get; set; }
    }
}
