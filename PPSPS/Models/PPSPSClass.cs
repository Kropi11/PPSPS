using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSClass
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(5)]
        [Column(TypeName = "varchar(5)")]
        public string ClassName { get; set; }

        [Column(TypeName = "varchar(767)")]
        public string ClassTeacher { get; set; }
    }
}