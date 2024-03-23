using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class Continent : DbEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public long Population { get; set; }

        public int PlanetId { get; set; }
        [ForeignKey(nameof(PlanetId))]
        public Planet Planet { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
    }
}
