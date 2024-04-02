using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Continent : DbEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Country> Countries { get; set; }
    }
}
