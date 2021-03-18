using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Range(0, 5)]
        [Column(TypeName = "int")]
        public int Grade { get; set; }

        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateSubmission { get; set; }
    }
}