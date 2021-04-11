using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PPSPS.Models
{
    public class PPSPSFile
    {
        [Key]
        [Column(TypeName = "varchar(767)")]
        public string Id { get; set; }

        [Display(Name = "Název souboru")]
        [Column(TypeName = "varchar(100)")]
        public string FileName { get; set; }

        [Display(Name = "Soubor")]
        [Column(TypeName = "longblob")]
        public byte[] File { get; set; }
    }
}