using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmiCerti.Models.ProjectModel
{
    public class CreateProjectFormModel
    {
        [Required]
        public string? Name { get; set; }

        [Required]
        [DisplayName("Token Symbol")]
        public string? TokenSymbol { get; set; }

        [Required]
        public string? Location { get; set; }

        [Required]
        [DisplayName("Project Type")]
        public string? ProjectType { get; set; }

        [Required]
        [DisplayName("Offset Category")]
        public string? OffsetCategory { get; set; }

        [Required]
        public string? Protocol { get; set; }

        [Required]
        public string? WebSite { get; set; }

        [Required]
        [DisplayName("Image Url")]
        public string? ImageUrl { get; set; }

        [Required]
        [DisplayName("Owner Account Number")]
        public long? OwnerAccountNumber { get; set; }

        [Required]
        public DateTime? Expiration { get; set; }

        //[Required]
        public string? Certifier { get; set; }

        [Required]
        public string? Verifier { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        [DisplayName("Start Period Reductions")]
        public DateTime? ReductionsPeriodStart { get; set; }

        [Required]
        [DisplayName("End Period Reductions")]
        public DateTime? ReductionsPeriodEnd { get; set; }

        [Required]
        [DisplayName("Reductions (tCO₂e)")]
        public decimal? ReductionstCO2e { get; set; }

        [Required]
        [DisplayName("Token Quantity")]
        public ulong? TokenQuantity { get; set; }

    }
}
