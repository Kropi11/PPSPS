using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSSubject
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(45, ErrorMessage = "Název předmětu nesmí mít víc, jak 45 znaků.")]
        [Display(Name = "Název předmětu")]
        [Column(TypeName = "varchar(45)")]
        public string SubjectName { get; set; }

        [StringLength(5, ErrorMessage = "Zkratka předmětu nesmí mít víc, jak 5 znaků.")]
        [Display(Name = "Zkratka předmětu")]
        [Column(TypeName = "varchar(5)")]
        public string SubjectAbbreviation { get; set; }
    }
}