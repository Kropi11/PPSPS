using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSSubject
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(45)")]
        public string SubjectName { get; set; }

        [Column(TypeName = "varchar(5)")]
        public string SubjectAbbreviation { get; set; }
    }
}