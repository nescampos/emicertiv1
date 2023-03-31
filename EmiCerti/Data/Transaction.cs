using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmiCerti.Data
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public long AccountNum { get; set; }

        [Required]
        public int ValidStartNanos { get; set; }

        [Required]
        public long ValidStartSeconds { get; set; }

        public long? From { get; set; }
        public long? Quantity { get; set; }

    }
}
