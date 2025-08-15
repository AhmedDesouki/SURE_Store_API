using System.Collections.Generic;

namespace SURE_Store_API.DTOs
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;
                foreach (var item in Items)
                {
                    total += item.Total;
                }
                return total;
            }
        }
    }
}
