using System;
using System.ComponentModel.DataAnnotations;

namespace FastFood.Models
{
    public class OrderItem
    {
        // OrderId -> integer, Primary Key
        // Order -> the item’s order(required)
        // ItemId -> integer, Primary Key
        // Item -> the order’s item(required)
        // Quantity -> the quantity of the item in the order(required, non-negative and non-zero)

        
        public int OrderId { get; set; }
        [Required]
        public Order Order { get; set; }

        
        public int ItemId { get; set; }
        [Required]
        public Item Item { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}