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

        [Display(Name = "Datum odevzdání")]
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateSubmission { get; set; }

        [StringLength(250, ErrorMessage = "Poznámka nesmí mít víc, jak 250 znaků.")]
        [Display(Name = "Poznámka")]
        [Column(TypeName = "varchar(250)")]
        public string Note { get; set; }

        [Column(TypeName = "longblob")]
        [Display(Name = "Práce")]
        public byte[] File { get; set; }

        public PPSPSUser User { get; set; }
        public PPSPSTask Task { get; set; }
    }
}