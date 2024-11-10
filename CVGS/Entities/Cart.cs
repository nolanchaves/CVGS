namespace CVGS.Entities
{
    public class Cart
    {
        public int CartID { get; set; }
        public string UserID { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public virtual User? User { get; set; }
    }
}
