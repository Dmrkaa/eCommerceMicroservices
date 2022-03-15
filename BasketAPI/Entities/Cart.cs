using System.Collections.Generic;
using System.Linq;

namespace CartAPI.Entities
{
    public class Cart
    {
        public string UserName { get; set; }
        List<CartItem> Items { get; set; }
        public Cart()
        {
        }
        public Cart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                if (Items != null)
                    totalPrice = Items.Sum(x => x.Price);
                return totalPrice;
            }
        }

    }
}
