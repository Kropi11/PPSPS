using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using PPSPS.Areas.Identity.Data;

namespace PPSPS.Models
{
    public class PPSPSRoles
    {
        [ForeignKey("Id")]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [ForeignKey("Name")]
        [Column(TypeName = "VARCHAR(256)")]
        public string RoleId { get; set; }
    }
}