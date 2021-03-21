using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSYearsOfStudies
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(4)]
        [Column(TypeName = "varchar(10)")]
        public string FirstSemester { get; set; }

        [StringLength(4)]
        [Column(TypeName = "varchar(10)")]
        public string SecondSemester { get; set; }

        [Display(Name = "Ročník")]
        public string Years
        {
            get { return FirstSemester + "/" + SecondSemester; }
        }
    }
}