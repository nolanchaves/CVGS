using System.ComponentModel.DataAnnotations;

namespace CVGS.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int CartId { get; set; } // FK to identify the cart
        public int GameId { get; set; }  // FK to identify the game
        public int Quantity { get; set; } = 1;

        public virtual Game Game { get; set; }
        public virtual Cart Cart { get; set; }
    }
}
