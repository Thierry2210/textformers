using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedacaoAPI.Models
{
    [Table("users")]
    public class User : BaseEntityGuid
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        // Somente Hash no banco
        public string Password { get; set; }

        [MaxLength(11)]
        public string UserPermission { get; set; }
        public int PlanId { get; set; }
        public Plan? Plan { get; set; }

        public int MonthlyEssayCount { get; set; } = 0;
        public DateTime LastEssayReset { get; set; } = DateTime.UtcNow;

        public ICollection<Essay> Essays { get; set; } = new List<Essay>();

        public User() { }
    }
}
