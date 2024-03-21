using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class Planet : DbEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        public virtual ICollection<Continent> Continents { get; set; }
    }
}
