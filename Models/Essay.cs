using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedacaoAPI.Models
{
    [Table("essays")]
    public class Essay : BaseEntityGuid
    {
        public string Title { get; set; }
        public string Content { get; set; }

        // Relacionamento com User
        public Guid UserId { get; set; }
        public User? User { get; set; }

        // Resultados da correção pela IA
        public string? CorrectedContent { get; set; }
        public int? Score { get; set; }
        public string? Feedback { get; set; }

        public Essay() { }
    }
}
