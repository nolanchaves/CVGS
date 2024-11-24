namespace CVGS.Entities
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
        public int ShippingAddressId { get; set; } // FK to ShippingAddress table
        public virtual ShippingAddress ShippingAddress { get; set; } // Navigation property
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }

}
