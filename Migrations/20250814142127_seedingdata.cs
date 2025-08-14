using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SURE_Store_API.Migrations
{
    /// <inheritdoc />
    public partial class seedingdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Devices and gadgets", "Electronics" },
                    { 2, "Apparel and accessories", "Clothing" },
                    { 3, "Literature and educational materials", "Books" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price", "StockQuantity" },
                values: new object[,]
                {
                    { 1, 1, "Latest Apple smartphone", "https://example.com/iphone15.jpg", "iPhone 15", 999.99m, 50 },
                    { 2, 1, "Latest Samsung smartphone", "https://example.com/galaxys23.jpg", "Samsung Galaxy S23", 899.99m, 30 },
                    { 3, 1, "Noise-cancelling headphones", "https://example.com/sonyheadphones.jpg", "Sony WH-1000XM5", 349.99m, 100 },
                    { 4, 2, "Comfortable running shoes", "https://example.com/nikeairmax.jpg", "Nike Air Max", 129.99m, 200 },
                    { 5, 2, "High-performance running shoes", "https://example.com/adidasultraboost.jpg", "Adidas Ultraboost", 159.99m, 150 },
                    { 6, 2, "Classic denim jeans", "https://example.com/levisjeans.jpg", "Levi's Jeans", 59.99m, 300 },
                    { 7, 3, "Classic novel by F. Scott Fitzgerald", "https://example.com/greatgatsby.jpg", "The Great Gatsby", 10.99m, 500 },
                    { 8, 3, "Dystopian novel about totalitarianism", "https://example.com/1984.jpg", "1984 by George Orwell", 12.99m, 400 },
                    { 9, 3, "Classic novel by Harper Lee", "https://example.com/tokillamockingbird.jpg", "To Kill a Mockingbird", 14.99m, 350 },
                    { 10, 3, "Novel by J.D. Salinger", "https://example.com/catcherintherye.jpg", "The Catcher in the Rye", 11.99m, 450 },
                    { 11, 1, "Apple's high-performance laptop", "https://example.com/macbookpro.jpg", "MacBook Pro", 1999.99m, 20 },
                    { 12, 1, "Compact and powerful laptop", "https://example.com/dellxps13.jpg", "Dell XPS 13", 1299.99m, 25 },
                    { 13, 1, "Wireless noise-cancelling headphones", "https://example.com/boseqc35.jpg", "Bose QuietComfort 35 II", 299.99m, 80 },
                    { 14, 1, "Professional mirrorless camera", "https://example.com/canoneosr5.jpg", "Canon EOS R5", 3899.99m, 15 },
                    { 15, 1, "Full-frame mirrorless camera", "https://example.com/sonya7iii.jpg", "Sony A7 III", 1999.99m, 30 },
                    { 16, 1, "Advanced fitness tracker with GPS", "https://example.com/fitbitcharge5.jpg", "Fitbit Charge 5", 149.99m, 100 },
                    { 17, 1, "Premium Android tablet with S Pen support", "https://example.com/galaxytabs8.jpg", "Samsung Galaxy Tab S8", 699.99m, 40 },
                    { 18, 1, "Smartwatch with health tracking features", "https://example.com/applewatchseries7.jpg", "Apple Watch Series 7", 399.99m, 60 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
