using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmiCerti.Data
{
    public class Project
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public string? TokenSymbol { get; set; }

        [Required]
        public string? WebSite { get; set; }


        [Required]
        public string? Location { get; set; }

        [Required]
        public string? ProjectType { get; set; }

        [Required]
        public string? OffsetCategory { get; set; }

        [Required]
        public string? Protocol { get; set; }

        //[Required]
        public string? Certifier { get; set; }

        [Required]
        public string? Verifier { get; set; }

        [Required]
        public long TokenId { get; set; }

        [Required]
        public string? ImageUrl { get; set; }

        [Required]
        public long? OwnerAccountNumber { get; set; }

        [Required]
        public bool Enable { get; set; }

        [Required]
        public ulong? TokenQuantity { get; set; }

        [Required]
        public DateTime? ReductionsPeriodStart { get; set; }

        [Required]
        public DateTime? ReductionsPeriodEnd { get; set; }

        [Required]
        public DateTime? Expiration { get; set; }

        [Required]
        public decimal? ReductionstCO2e { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool HBARsent { get; set; }

        public string signatureRequestId { get;set; }
    }
}
