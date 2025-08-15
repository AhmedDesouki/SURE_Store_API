using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;  

namespace SURE_Store_API.Models  // Define namespace for all domain entities
{
    
    public class Product
    {
       
        public int Id { get; set; }
        
        
        [Required]  
        [StringLength(200)]  
        public string Name { get; set; } = string.Empty;  // Initialize as empty string to avoid null reference exceptions
        
        
        [StringLength(1000)]  
        public string Description { get; set; } = string.Empty;  
        
        
        // Uses decimal type for accurate financial calculations without rounding errors
        [Required]  
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]  // Validation attribute: minimum value 0.01
        public decimal Price { get; set; }
        
        
        [StringLength(500)]  
        public string ImageUrl { get; set; } = string.Empty;  
        
        
        [Required]  
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater")]  
        public int StockQuantity { get; set; }

        
        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }
        
        
        public virtual Category Category { get; set; } = null!;
        
        
        // Virtual keyword enables Entity Framework lazy loading (order items loaded only when accessed)
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
