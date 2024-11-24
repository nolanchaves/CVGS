using CVGS.Entities;

namespace CVGS.Models
{
    public class OrderViewModel
    {
        public List<OrderDetail> OrderedDetails { get; set; }
        public decimal Subtotal { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
