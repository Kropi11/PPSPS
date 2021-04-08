using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSYearsOfStudies
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [Column(TypeName = "int")]
        public int FirstSemester { get; set; }

        [Column(TypeName = "int")]
        public int SecondSemester { get; set; }

        [Display(Name = "Ročník")]
        public string Years
        {
            get { return FirstSemester + "/" + SecondSemester; }
        }
    }
}