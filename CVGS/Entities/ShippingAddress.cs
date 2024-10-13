using System.ComponentModel.DataAnnotations;

namespace CVGS.Entities
{
    public class ShippingAddress
    {
        [Key]
        public int ShippingAddressId { get; set; }

        [Required]
        [StringLength(15)]
        [DataType(DataType.PhoneNumber)]
        public string ShippingPhoneNumber { get; set; }

        [StringLength(100)]
        public string ShippingStreetAddress { get; set; }

        public string? ShippingAptSuite { get; set; }

        [StringLength(50)]
        public string ShippingCity { get; set; }

        [StringLength(50)]
        public string ShippingProvince { get; set; }

        [StringLength(6)]
        public string ShippingPostalCode { get; set; }

        [StringLength(50)]
        public string ShippingCountry { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
    }
}
