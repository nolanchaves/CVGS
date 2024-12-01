namespace CVGS.Models
{
    public class OrderDetailViewModel
    {
        public string GameTitle { get; set; }
        public string GameImageUrl { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string GameType { get; set; }
    }
}
