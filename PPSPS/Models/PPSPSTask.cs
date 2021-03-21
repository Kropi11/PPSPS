using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Models
{
    public class PPSPSTask
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(100)]
        [Display(Name = "Název práce")]
        [Column(TypeName = "varchar(100)")]
        public string TaskName { get; set; }

        [StringLength(250)]
        [Display(Name = "Zadání")]
        [Column(TypeName = "varchar(250)")]
        public string Description { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "Od")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateEntered { get; set; }

        [Column(TypeName = "datetime")]
        [Display(Name = "Do")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateDeadline { get; set; }

        [Column(TypeName = "varchar(767)")]
        [Display(Name = "Třída")]
        public string ClassId { get; set; }

        [Column(TypeName = "varchar(767)")]
        [Display(Name = "Předmět")]
        public string SubjectId { get; set; }

        [Column(TypeName = "varchar(767)")]
        [Display(Name = "Vyučující")]
        public string TeacherId { get; set; }

        [Column(TypeName = "longblob")]
        [Display(Name = "Přílohy")]
        public string File { get; set; }

        public PPSPSUser Teacher { get; set; }
        public PPSPSClass Class { get; set; }
        public PPSPSSubject Subject { get; set; }

    }
}