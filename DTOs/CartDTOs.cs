using System.ComponentModel.DataAnnotations;

namespace SURE_Store_API.DTOs
{
    public class AddToCartRequest {
        [Required]  // Validation: field is mandatory
        public int ProductId { get; set; }
        [Required]  // Validation: field is mandatory
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]  // Validation: minimum 1
        public int Quantity { get; set; }
    }
    public class UpdateCartItemRequest
    {
        [Required]  // Validation: field is mandatory
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]  // Validation: minimum 1
        public int Quantity { get; set; }

    }
    public class CartItemDto {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string ProductImageUrl { get; set; } = string.Empty;
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public int AvailableStock { get; set; }
    }
    public class CartDto {
        public int Id { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public decimal TotalAmount { get; set; }
        public int TotalItems { get; set; }
    }
    public class CartResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<CartItemDto> CartItems { get; set; } = new List<CartItemDto>();
        public int TotalItems { get; set; }
        public decimal TotalAmount { get; set; }


    }



    public class CartDTOs
    {
    }
}
