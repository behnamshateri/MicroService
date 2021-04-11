using System.Collections.Generic;
using System.Linq;

namespace Basket.Entities
{
    public class BasketCart
    {
        public string UserName { get; set; }
        public List<BasketCartItem> Items { get; set; } = new List<BasketCartItem>();

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(item => item.Price * item.Quantity);
            }
        }

        public BasketCart()
        {
        }
        
        public BasketCart(string userName)
        {
            UserName = userName;
        }
    }
}