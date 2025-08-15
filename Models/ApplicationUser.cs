using Microsoft.AspNetCore.Identity; 

namespace SURE_Store_API.Models  
{
    
    public class ApplicationUser : IdentityUser  
    {
       
        public string FullName { get; set; } = string.Empty; 
        
       
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); 
        
      
        public virtual Cart? Cart { get; set; }

       
        public string? RefreshToken { get; set; }

       
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
