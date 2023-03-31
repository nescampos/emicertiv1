using System.ComponentModel.DataAnnotations;

namespace EmiCerti.Models.ProjectModel
{
    public class BuyTokenFormModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public long? Amount { get; set; }

        [Required]
        public long? AccountNumber { get; set; }

        [Required]
        public string? PrivateKey { get; set; }

    }
}
