using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class Tasks
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; }

    }
}