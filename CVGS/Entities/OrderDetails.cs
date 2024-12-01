namespace CVGS.Entities
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int GameId { get; set; } 
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string GameType { get; set; }

        public Game? Game { get; set; }
        public Order Order { get; set; } // Navigation property

    }

}
