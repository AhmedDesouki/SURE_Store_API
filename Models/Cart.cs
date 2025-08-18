
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SURE_Store_API.Models  
{
 
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        
        
        [Required]  // Validation: field is mandatory
        [ForeignKey("UserId")]
        public string UserId { get; set; } = string.Empty;
        
     
        public virtual ApplicationUser User { get; set; } = null!;
        
       
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        
        
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        

        
        
        public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
