using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSGroup
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(45, ErrorMessage = "Název skupiny nesmí mít víc, jak 45 znaků.")]
        [Display(Name = "Název skupiny")]
        [Column(TypeName = "varchar(45)")]
        public string GroupName { get; set; }

        [StringLength(20, ErrorMessage = "Zkratka skupiny nesmí mít víc, jak 20 znaků.")]
        [Display(Name = "Zkratka skupiny")]
        [Column(TypeName = "varchar(20)")]
        public string GroupAbbreviation { get; set; }
    }
}