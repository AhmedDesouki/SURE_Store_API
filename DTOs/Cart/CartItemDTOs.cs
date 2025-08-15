namespace SURE_Store_API.DTOs
{
    public class CartItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }   // اسم المنتج
        public decimal Price { get; set; }        // سعر المنتج
        public int Quantity { get; set; }         // الكمية
        public decimal Total => Price * Quantity; // السعر الإجمالي لهذا المنتج
    }
}
