using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPSPS.Models;

namespace PPSPS.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the PPSPSUser class
    public class PPSPSUser : IdentityUser
    {
        [PersonalData]
        [Display(Name = "Křestní jméno")]
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [PersonalData]
        [Display(Name = "Příjmení")]
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

        [PersonalData]
        [Display(Name = "Třída")]
        [Column(TypeName = "nvarchar(767)")]
        public string ClassId { get; set; }

        public PPSPSClass Class { get; set; }
    }
}
