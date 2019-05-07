using System.ComponentModel.DataAnnotations;

namespace SoftJail.DataProcessor.ImportDto
{
    public class MailDto
    {
        [Required]
        public string Description { get; set; }

        [Required]
        public string Sender { get; set; }

        [RegularExpression("^[A-Za-z 0-9]+ str.$")]
        [Required]
        public string Address { get; set; }
    }
}