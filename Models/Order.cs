using System.ComponentModel.DataAnnotations;  
using System.ComponentModel.DataAnnotations.Schema;  

namespace SURE_Store_API.Models { 
   
    public class Order
    {
      
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
        
      
        [Required]  
        public OrderStatus Status { get; set; }  
        
       
        [Range(0.01, double.MaxValue, ErrorMessage = "Order total must be greater than 0")]  
        [Column(TypeName = "decimal(18,2)")]  
        public decimal TotalAmount { get; set; }
        
        
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;  
        
        
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;  
        
        
        [Required] 
        [StringLength(500)]  
        public string ShippingAddress { get; set; } = string.Empty;  
        
        [Required] 
        [StringLength(20)]  
        public string PhoneNumber { get; set; } = string.Empty; 
        
       
        [StringLength(1000)]  
        public string? Notes { get; set; }  
        
      
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
