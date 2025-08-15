using System.ComponentModel.DataAnnotations;

namespace SURE_Store_API.DTOs
{

    /// Data transfer object for creating new categories
    /// Contains the required information to create a product category

    public class CreateCategoryRequest
    {

        [Required]  // Validation: field is mandatory
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;


        [StringLength(500)]
        public string? Description { get; set; }
    }

    /// <summary>
    /// Data transfer object for updating existing categories
    /// Contains the field that can be modified for a category
    /// </summary>
    public class UpdateCategoryRequest
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;


        [StringLength(500)]
        public string? Description { get; set; }
    }


    /// Data transfer object for category information in responses
    /// Contains complete category details including product count
    /// Data transfer object for category information in responses

    public class CategoryDto
    {

        public int Id { get; set; }


        public string Name { get; set; } = string.Empty;


        public string? Description { get; set; }


        public int ProductCount { get; set; }
    }
}
