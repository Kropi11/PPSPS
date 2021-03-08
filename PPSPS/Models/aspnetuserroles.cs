﻿using System.ComponentModel.DataAnnotations.Schema;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Models
{
    public class aspnetuserroles
    {
        [ForeignKey("UserId")]
        [Column(TypeName = "nvarchar(767)")]
        public string UserId { get; set; }

        [ForeignKey("RoleId")]
        [Column(TypeName = "nvarchar(737)")]
        public string RoleId { get; set; }
    }
}