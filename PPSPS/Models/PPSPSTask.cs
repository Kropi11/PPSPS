using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PPSPS.Models
{
    public class PPSPSTask
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string TaskName { get; set; }

        [StringLength(250)]
        [Column(TypeName = "varchar(250)")]
        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateEntered { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime DateDeadline { get; set; }

        [Column(TypeName = "varchar(767)")]
        public string Class { get; set; }
    }
}