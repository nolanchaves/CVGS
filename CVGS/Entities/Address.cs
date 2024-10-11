using System.ComponentModel.DataAnnotations;

namespace CVGS.Entities
{
    public class Address
    {
        [Key]
        public int AddressId { get; set; }

        [Required]
        [StringLength(100)]
        public string StreetAddress { get; set; }

        public string? AptSuite { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Province { get; set; }

        [Required]
        [StringLength(6)]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        public string? DeliveryInstructions { get; set; }
    }
}
