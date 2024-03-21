using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class TouristLocation : DbEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [Range(1, 10, ErrorMessage = "Value must be between 1 and 10 included")]
        public int Rating { get; set; }

        public int CityId { get; set; }
        [ForeignKey(nameof(CityId))]
        public City City { get; set; }
    }
}
