using System;
using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class PrisonerDto
    {
        [StringLength(20, MinimumLength = 3)]
        [Required]
        public string FullName { get; set; }

        [RegularExpression(@"^The \b[A-Z][A-Za-z]+\b$")]
        [Required]
        public string Nickname { get; set; }

        [Range(18, 65)]
        [Required]
        public int Age { get; set; }

        [Required]
        public string IncarcerationDate { get; set; }

        public string ReleaseDate { get; set; }

        [Range(0, Double.PositiveInfinity)]
        public decimal? Bail { get; set; }

        [Required]
        public int CellId { get; set; }

        public MailDto[] Mails { get; set; }
    }
}