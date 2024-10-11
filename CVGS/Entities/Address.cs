using CVGS.Entities;
using System.ComponentModel.DataAnnotations;

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

    public string? UserId { get; set; } // Make this nullable
    public User? User { get; set; } // Make this nullable
}
