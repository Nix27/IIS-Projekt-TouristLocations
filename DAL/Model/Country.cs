using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Model
{
    public class Country : DbEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public int ContinentId { get; set; }
        [ForeignKey(nameof(ContinentId))]
        public Continent Continent { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
