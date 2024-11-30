namespace CVGS.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalBeforeTax { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
