using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Models
{
    public class PPSPSAssignment
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [Column(TypeName = "varchar(767)")]
        public string UserId { get; set; }

        [Column(TypeName = "varchar(767)")]
        public string TaskId { get; set; }

        [Range(0, 5, ErrorMessage = "Známka musí být v rozsahu 1 - 5")]
        [Display(Name = "Známka")]
        [Column(TypeName = "int")]
        public int Grade { get; set; }

        [StringLength(250, ErrorMessage = "Poznámka nesmí mít víc, jak 250 znaků.")]
        [Display(Name = "Poznámka")]
        [Column(TypeName = "varchar(250)")]
        public string Note { get; set; }

        [Column(TypeName = "varchar(767)")]
        [Display(Name = "Práce")]
        public string FileId { get; set; }

        public PPSPSUser User { get; set; }
        public PPSPSTask Task { get; set; }
        public PPSPSFile File { get; set; }
    }
}