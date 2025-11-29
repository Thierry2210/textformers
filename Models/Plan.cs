using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedacaoAPI.Models
{
    [Table("plans")]
    public class Plan : BaseEntityInt
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MaxCorrections { get; set; }

        public ICollection<User>? Users { get; set; } = new List<User>();

        public Plan() { }
    }
}