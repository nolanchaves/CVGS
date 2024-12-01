using CVGS.Entities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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
        [ValidExpiryDate]
        public string ExpiryDate { get; set; }

        [Required(ErrorMessage = "CVV is required.")]
        [RegularExpression(@"^[0-9]{3,4}$", ErrorMessage = "Invalid CVV.")]
        [Display(Name = "CVV")]
        public string CVV { get; set; }

        public string? PaymentMethod { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxRate { get; set; }
        public List<CartItem> CartItems { get; set; }


        public void CalculateTotal()
        {
            TotalPrice = CartItems.Sum(item => item.Quantity * item.Game.Price);
        }

        public class ValidExpiryDateAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is string expiryDate)
                {
                    // Parse expiry date
                    if (DateTime.TryParseExact(expiryDate, "MM/yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                    {
                        // Set the last day of the month
                        parsedDate = parsedDate.AddMonths(1).AddDays(-1);

                        // Check if the date is in the future
                        if (parsedDate >= DateTime.Now)
                        {
                            return ValidationResult.Success;
                        }
                        return new ValidationResult("The card has expired.");
                    }
                    return new ValidationResult("Invalid expiry date format. Use MM/YY.");
                }

                return new ValidationResult("Card Expiry Date is required.");
            }
        }
    }
}