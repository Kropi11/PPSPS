using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PPSPS.Models
{
    [Table("aspnetusers", Schema = "ppsps")]

    public class PPSPSUserView : IdentityUser
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [PersonalData]
        [Display(Name = "Křestní jméno")]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Příjmení")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }
    }
}