using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class City : DbEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public long Population { get; set; }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

        public virtual ICollection<TouristLocation> TouristLocations { get; set; }
    }
}
