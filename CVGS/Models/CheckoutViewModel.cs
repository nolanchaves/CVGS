using CVGS.Entities;
using System.ComponentModel.DataAnnotations;

namespace CVGS.Models
{
    public class CheckoutViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required.")]
        [CreditCard(ErrorMessage = "Invalid credit card number.")]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNumber { get; set; }

        [Required(ErrorMessage = "Card Expiry Date is required.")]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$", ErrorMessage = "Invalid expiry date. Use MM/YY.")]
        [Display(Name = "Card Expiry Date")]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Invalid CVV.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        public int ShippingAddressId { get; set; }

        public decimal TotalPrice { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxRate { get; set; }
        public List<CartItem> CartItems { get; set; }
    }

}