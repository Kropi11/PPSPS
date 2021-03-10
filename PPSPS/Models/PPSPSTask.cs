using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSTask
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string TaskName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateEntered { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateDeadline { get; set; }

    }
}