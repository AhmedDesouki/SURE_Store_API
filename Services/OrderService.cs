using Microsoft.EntityFrameworkCore;
using SURE_Store_API.Data;
using SURE_Store_API.DTOs;
using SURE_Store_API.Models;
using SURE_Store_API.Repositories;
using SURE_Store_API.Interfaces;

namespace SURE_Store_API.Services
{
    
    // Service for handling order-related business operations
    // Implements order creation, management, and payment processing
    
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(
            ApplicationDbContext context,
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        
        // Creates a new order from cart items
      
        public async Task<OrderDto> CreateOrderAsync(string userId, CreateOrderRequest request)
        {
            // Validate order items
            if (request.OrderItems == null || !request.OrderItems.Any())
            {
                throw new ArgumentException("Order must contain at least one item");
            }

            // Calculate total and validate products
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemRequest in request.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(itemRequest.ProductId);
                if (product == null)
                {
                    throw new ArgumentException($"Product with ID {itemRequest.ProductId} not found");
                }

                if (product.StockQuantity < itemRequest.Quantity)
                {
                    throw new ArgumentException($"Insufficient stock for product {product.Name}");
                }

                // Update product stock
                product.StockQuantity -= itemRequest.Quantity;
                await _productRepository.UpdateAsync(product);

                // Create order item
                var orderItem = new OrderItem
                {
                    ProductId = itemRequest.ProductId,
                    Quantity = itemRequest.Quantity,
                    Price = product.Price
                };

                orderItems.Add(orderItem);
                totalAmount += product.Price * itemRequest.Quantity;
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = totalAmount,
                Status = OrderStatus.Pending,
                ShippingAddress = request.ShippingAddress,
                ShippingCity = request.ShippingCity,
                ShippingPostalCode = request.ShippingPostalCode,
                ShippingCountry = request.ShippingCountry,
                PhoneNumber = request.PhoneNumber,
                Notes = request.Notes,
                OrderItems = orderItems
            };

            await _orderRepository.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            // Clear user's cart after successful order creation
            await _cartRepository.ClearCartAsync(userId);

            return await MapToOrderDto(order);
        }

        
        // Retrieves an order by its ID
        
        public async Task<OrderDto?> GetOrderByIdAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            return order != null ? await MapToOrderDto(order) : null;
        }

        
        // Retrieves all orders for a specific user
      
        public async Task<OrderListResponse> GetUserOrdersAsync(string userId, int page = 1, int pageSize = 10)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                orderDtos.Add(await MapToOrderDto(order));
            }

            return new OrderListResponse
            {
                Orders = orderDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        
        // Retrieves all orders (admin only)
        
        public async Task<OrderListResponse> GetAllOrdersAsync(int page = 1, int pageSize = 10, OrderStatus? status = null)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.User)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            query = query.OrderByDescending(o => o.OrderDate);

            var totalCount = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var orders = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var orderDtos = new List<OrderDto>();
            foreach (var order in orders)
            {
                orderDtos.Add(await MapToOrderDto(order));
            }

            return new OrderListResponse
            {
                Orders = orderDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };
        }

        
        // Updates the status of an order
        
        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, UpdateOrderRequest request)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new ArgumentException($"Order with ID {orderId} not found");
            }

            order.Status = request.Status;
            order.Notes = request.Notes;

            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return await MapToOrderDto(order);
        }

        
        // Cancels an order
      
        public async Task<bool> CancelOrderAsync(int orderId, string userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
            {
                return false;
            }

            // Only allow cancellation of pending orders
            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Only pending orders can be cancelled");
            }

            // Restore product stock
            foreach (var orderItem in order.OrderItems)
            {
                var product = orderItem.Product;
                product.StockQuantity += orderItem.Quantity;
                await _productRepository.UpdateAsync(product);
            }

            order.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        
        // Processes payment for an order
       
        public async Task<bool> ProcessPaymentAsync(int orderId, string paymentMethod)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return false;
            }

            // Simulate payment processing
            // In a real application, this would integrate with a payment gateway
            await Task.Delay(1000); // Simulate processing time

            // Update order status to processing
            order.Status = OrderStatus.Processing;
            await _orderRepository.UpdateAsync(order);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        
        // Gets order statistics for admin dashboard
        
        public async Task<OrderStatisticsDto> GetOrderStatisticsAsync()
        {
            var orders = await _context.Orders.ToListAsync();

            var statistics = new OrderStatisticsDto
            {
                TotalOrders = orders.Count,
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ProcessingOrders = orders.Count(o => o.Status == OrderStatus.Processing),
                ShippedOrders = orders.Count(o => o.Status == OrderStatus.Shipped),
                DeliveredOrders = orders.Count(o => o.Status == OrderStatus.Delivered),
                CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled),
                TotalRevenue = orders.Where(o => o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0
            };

            return statistics;
        }

        
        // Maps Order entity to OrderDto
        
        private async Task<OrderDto> MapToOrderDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                UserId = order.UserId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                ShippingAddress = order.ShippingAddress,
                ShippingCity = order.ShippingCity,
                ShippingPostalCode = order.ShippingPostalCode,
                ShippingCountry = order.ShippingCountry,
                PhoneNumber = order.PhoneNumber,
                Notes = order.Notes,
                OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    SubTotal = oi.Quantity * oi.Price,
                    Product = oi.Product != null ? new ProductDto
                    {
                        Id = oi.Product.Id,
                        Name = oi.Product.Name,
                        Description = oi.Product.Description,
                        Price = oi.Product.Price,
                        ImageUrl = oi.Product.ImageUrl,
                        StockQuantity = oi.Product.StockQuantity,
                        CategoryId = oi.Product.CategoryId
                    } : null
                }).ToList(),
                User = order.User != null ? new UserDto
                {
                    Id = order.User.Id,
                    FullName = order.User.FullName,
                    Email = order.User.Email ?? string.Empty
                } : null
            };
        }
    }
}