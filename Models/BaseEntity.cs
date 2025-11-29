using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedacaoAPI.Models
{
    public class BaseTimeEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow.AddHours(-3);
        public DateTime? DeletedAt { get; set; }
    }

    public class BaseEntityGuid : BaseTimeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
    }

    public class BaseEntityInt : BaseTimeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    }
}
