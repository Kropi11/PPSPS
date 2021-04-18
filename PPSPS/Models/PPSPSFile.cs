using System;
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

        [Display(Name = "Přípona")]
        [Column(TypeName = "varchar(10)")]
        public string Extension { get; set; }

        [Display(Name = "Druh souboru")]
        [Column(TypeName = "varchar(250)")]
        public string FileType { get; set; }

        [Display(Name = "Soubor")]
        [Column(TypeName = "longblob")]
        public byte[] File { get; set; }

        [Display(Name = "Datum odevzdání")]
        [Column(TypeName = "datetime")]
        [DisplayFormat(DataFormatString = "{0:dd. MM. yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime DateSubmission { get; set; }
    }
}