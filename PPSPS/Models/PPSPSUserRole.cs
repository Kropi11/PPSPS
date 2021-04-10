using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Models
{
    public class PPSPSUserRole : IdentityRole
    {
        [ForeignKey("UserId")]
        [Column(TypeName = "varchar(767)")]
        public string UserId { get; set; }

        [ForeignKey("RoleId")]
        [Column(TypeName = "varchar(767)")]
        public string RoleId { get; set; }
    }
}