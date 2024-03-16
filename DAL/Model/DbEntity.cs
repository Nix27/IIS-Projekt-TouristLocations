using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public abstract class DbEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
