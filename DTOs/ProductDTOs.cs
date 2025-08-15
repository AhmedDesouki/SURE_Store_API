using System.ComponentModel.DataAnnotations;

namespace SURE_Store_API.DTOs
{

    /// Data transfer object for creating new products
    /// Contains all required information to create a product
    public class CreateProductRequest
    {

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]  // Validation: minimum 0.01
        public decimal Price { get; set; }

        
        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater")]  // Validation: minimum 0
        public int StockQuantity { get; set; }


        [Required]
        public int CategoryId { get; set; }
    }


    /// Data transfer object for updating existing products
    /// Contains all fields that can be modified for a product

    public class UpdateProductRequest
    {

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;


        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;


        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]  // Validation: minimum 0.01
        public decimal Price { get; set; }


        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;


        [Required]  // Validation: field is mandatory
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or greater")]  // Validation: minimum 0
        public int StockQuantity { get; set; }


        [Required]  // Validation: field is mandatory
        public int CategoryId { get; set; }
    }


    /// Data transfer object for product information in responses
    /// Contains complete product details including category information

    public class ProductDto
    {

        public int Id { get; set; }


        public string Name { get; set; } = string.Empty;


        public string Description { get; set; } = string.Empty;


        public decimal Price { get; set; }


        public string ImageUrl { get; set; } = string.Empty;


        public int StockQuantity { get; set; }


        public int CategoryId { get; set; }


        public string CategoryName { get; set; } = string.Empty;
    }


    /// Data transfer object for paginated product list responses
    /// Contains products and pagination information

    public class ProductListResponse
    {

        public List<ProductDto> Products { get; set; } = new List<ProductDto>();


        public int TotalCount { get; set; }


        public int PageNumber { get; set; }

        public int PageSize { get; set; }


        public int TotalPages { get; set; }
    }
}
