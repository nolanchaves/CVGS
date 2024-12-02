namespace CVGS.Models
{
    public class UserOrdersViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string PaymentMethod { get; set; }
        public string GameType {  get; set; }
        public string GameTitle {  get; set; }
    }
}
