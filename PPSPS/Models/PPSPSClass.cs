﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Models
{
    public class PPSPSClass
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [StringLength(5)]
        [Display(Name = "Název třídy")]
        [Column(TypeName = "varchar(5)")]
        public string ClassName { get; set; }

        [Display(Name = "Třídní učitel")]
        [Column(TypeName = "varchar(767)")]
        public string ClassTeacherId { get; set; }

        ///public PPSPSUser ClassTeacher { get; set; }
    }
}