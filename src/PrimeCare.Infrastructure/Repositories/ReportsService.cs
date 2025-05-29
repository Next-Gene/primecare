// src/PrimeCare.Application/Services/Implementations/ReportsService.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Core.Entities.Identity;
using PrimeCare.Core.Entities.Order;
using PrimeCare.Core.Entities.OrderAggregate;
using PrimeCare.Infrastructure.Data;
using PrimeCare.Shared.Dtos.Reports;

namespace PrimeCare.Application.Services.Implementations
{
    /// <summary>
    /// Service for generating various business reports
    /// </summary>
    public class ReportsService : IReportsService
    {
        private readonly PrimeCareContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportsService(PrimeCareContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private (DateTime StartDate, DateTime EndDate) GetDateRange(ReportFilterDto filter, int defaultMonthsBack)
        {
            var startDate = filter.StartDate ?? DateTime.UtcNow.AddMonths(-defaultMonthsBack);
            var endDate = filter.EndDate ?? DateTime.UtcNow;
            return (startDate, endDate);
        }

        public async Task<AdminReportDto> GetAdminReportAsync(ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 6);

            var orders = await GetOrdersQueryAsync(startDate, endDate, filter);

            return new AdminReportDto
            {
                TotalOrders = orders.Count(),
                TotalRevenue = orders.Sum(o => o.Subtotal),
                OrderStatusSummary = await GetOrderStatusSummaryAsync(filter),
                TopSellingProducts = await GetTopSellingProductsAsync(filter),
                DailyRevenue = await GetDailyRevenueAsync(filter),
                MonthlyRevenue = await GetMonthlyRevenueAsync(filter)
            };
        }

        public async Task<SellerReportDto> GetSellerReportAsync(string sellerId, ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 6);

            var sellerProducts = await _context.Products
                .Where(p => p.CreatedBy == sellerId)
                .Select(p => p.Id)
                .ToListAsync();

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemOrderd)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Where(o => o.OrderItems.Any(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId)))
                .ToListAsync();

            var sellerOrderItems = orders
                .SelectMany(o => o.OrderItems)
                .Where(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId))
                .ToList();

            var seller = await _userManager.FindByIdAsync(sellerId);

            return new SellerReportDto
            {
                SellerId = sellerId,
                SellerName = seller?.FullName ?? "Unknown Seller",
                TotalProductsSold = sellerOrderItems.Sum(oi => oi.Quantity),
                TotalRevenue = sellerOrderItems.Sum(oi => oi.Price * oi.Quantity),
                TotalOrders = orders.Count,
                TopSellingProducts = (await GetSellerTopProductsAsync(sellerId, filter))
                    .Select(tp => new SellerTopProductDto
                    {
                        ProductId = tp.ProductId,
                        ProductName = tp.ProductName,
                        Category = tp.Category,
                        Brand = tp.Brand,
                        QuantitySold = tp.QuantitySold, // Fixed: was tp.TotalQuantitySold
                        Revenue = tp.Revenue, // Fixed: was tp.TotalRevenue
                        Price = tp.Price, // Fixed: calculate once in GetSellerTopProductsAsync
                        PhotoUrl = tp.PhotoUrl
                    }).ToList(),
                MonthlyRevenue = await GetSellerMonthlyRevenueAsync(sellerId, filter)
            };
        }

        // Fix for CS0117: 'OrderStatus' does not contain a definition for 'Delivered'
        // The issue occurs because the 'OrderStatus' enum does not have a 'Delivered' value.
        // Based on the context, it seems 'PaymentReceived' might be the intended value for completed orders.
        // Update the code to use 'PaymentReceived' instead of 'Delivered'.

        public async Task<OrderStatusSummaryDto> GetOrderStatusSummaryAsync(ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 1);

            var orders = await GetOrdersQueryAsync(startDate, endDate, filter);

            return new OrderStatusSummaryDto
            {
                PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
                ProcessingOrders = orders.Count(o => o.Status == OrderStatus.PaymentReceived),
                CompletedOrders = orders.Count(o => o.Status == OrderStatus.PaymentReceived), // Updated: Use PaymentReceived for completed orders
                CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled),
                PendingRevenue = orders.Where(o => o.Status == OrderStatus.Pending).Sum(o => o.Subtotal),
                ProcessingRevenue = orders.Where(o => o.Status == OrderStatus.PaymentReceived).Sum(o => o.Subtotal),
                CompletedRevenue = orders.Where(o => o.Status == OrderStatus.PaymentReceived).Sum(o => o.Subtotal) // Updated: Use PaymentReceived for completed revenue
            };
        }

        // Fix for CS0019: Operator '??' cannot be applied to operands of type 'int' and 'int'
        // The issue occurs because 'filter.TopCount' is of type 'int', which cannot be null.
        // To fix this, we need to use a nullable type or check for a default value explicitly.

        public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 3);

            var topProducts = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemOrderd)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.ItemOrderd.ProductItemId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Price * oi.Quantity),
                    ProductName = g.First().ItemOrderd.ProductName,
                    PhotoUrl = g.First().ItemOrderd.ProductImageUrl
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(filter.TopCount > 0 ? filter.TopCount : 10) // Fixed: Explicitly check for a valid TopCount value
                .ToListAsync();

            var result = new List<TopSellingProductDto>();
            foreach (var item in topProducts)
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductBrand)
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                result.Add(new TopSellingProductDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Category = product?.Category?.Name ?? "Unknown",
                    Brand = product?.ProductBrand?.Name ?? "Unknown",
                    TotalQuantitySold = item.TotalQuantity,
                    TotalRevenue = item.TotalRevenue,
                    PhotoUrl = item.PhotoUrl
                });
            }

            return result;
        }

        public async Task<List<RevenueChartDataDto>> GetDailyRevenueAsync(ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 1);

            var dailyRevenueRaw = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(e => e.Subtotal),
                    OrderCount = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var dailyRevenue = dailyRevenueRaw.Select(g => new RevenueChartDataDto
            {
                Period = g.Date.ToString("yyyy-MM-dd"),
                Revenue = g.Revenue,
                OrderCount = g.OrderCount
            }).ToList();

            return dailyRevenue;
        }

        public async Task<List<RevenueChartDataDto>> GetMonthlyRevenueAsync(ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 12);

            var monthlyRevenueRaw = await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.Sum(o => o.Subtotal),
                    OrderCount = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            var monthlyRevenue = monthlyRevenueRaw.Select(g => new RevenueChartDataDto
            {
                Period = $"{g.Year}-{g.Month:D2}",
                Revenue = g.Revenue,
                OrderCount = g.OrderCount
            }).ToList();

            return monthlyRevenue;
        }

        private async Task<List<RevenueChartDataDto>> GetSellerMonthlyRevenueAsync(string sellerId, ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 12);

            var sellerProducts = await _context.Products
                .Where(p => p.CreatedBy == sellerId)
                .Select(p => p.Id)
                .ToListAsync();

            var monthlyRevenueRaw = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemOrderd)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Where(o => o.OrderItems.Any(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId)))
                .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Revenue = g.SelectMany(o => o.OrderItems)
                             .Where(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId))
                             .Sum(oi => oi.Price * oi.Quantity),
                    OrderCount = g.Count()
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            var monthlyRevenue = monthlyRevenueRaw.Select(g => new RevenueChartDataDto
            {
                Period = $"{g.Year}-{g.Month:D2}",
                Revenue = g.Revenue,
                OrderCount = g.OrderCount
            }).ToList();

            return monthlyRevenue;
        }

        private async Task<List<Order>> GetOrdersQueryAsync(DateTime startDate, DateTime endDate, ReportFilterDto filter)
        {
            var query = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemOrderd)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate);

            if (filter.CategoryId.HasValue || filter.BrandId.HasValue)
            {
                var productIds = await _context.Products
                    .Where(p => (!filter.CategoryId.HasValue || p.CategoryId == filter.CategoryId.Value) &&
                               (!filter.BrandId.HasValue || p.ProductBrandId == filter.BrandId.Value))
                    .Select(p => p.Id)
                    .ToListAsync();

                query = query.Where(o => o.OrderItems.Any(oi => productIds.Contains(oi.ItemOrderd.ProductItemId)));
            }

            return await query.ToListAsync();
        }

        public async Task<List<RevenueChartDataDto>> GetSellerDailyRevenueAsync(string sellerId, ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 1);

            var sellerProducts = await _context.Products
                .Where(p => p.CreatedBy == sellerId)
                .Select(p => p.Id)
                .ToListAsync();

            var dailyRevenueRaw = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemOrderd)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Where(o => o.OrderItems.Any(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId)))
                .GroupBy(o => o.OrderDate.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.SelectMany(o => o.OrderItems)
                               .Where(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId))
                               .Sum(oi => oi.Price * oi.Quantity),
                    OrderCount = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();

            var dailyRevenue = dailyRevenueRaw.Select(g => new RevenueChartDataDto
            {
                Period = g.Date.ToString("yyyy-MM-dd"),
                Revenue = g.Revenue,
                OrderCount = g.OrderCount
            }).ToList();

            return dailyRevenue;
        }

        public async Task<List<SellerTopProductDto>> GetSellerTopProductsAsync(string sellerId, ReportFilterDto filter)
        {
            var (startDate, endDate) = GetDateRange(filter, 6);

            var sellerProducts = await _context.Products
                .Where(p => p.CreatedBy == sellerId)
                .Select(p => p.Id)
                .ToListAsync();

            var topProducts = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ItemOrderd)
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .Where(o => o.OrderItems.Any(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId)))
                .SelectMany(o => o.OrderItems)
                .Where(oi => sellerProducts.Contains(oi.ItemOrderd.ProductItemId))
                .GroupBy(oi => oi.ItemOrderd.ProductItemId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalQuantity = g.Sum(oi => oi.Quantity),
                    TotalRevenue = g.Sum(oi => oi.Price * oi.Quantity),
                    ProductName = g.First().ItemOrderd.ProductName,
                    PhotoUrl = g.First().ItemOrderd.ProductImageUrl
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(filter.TopCount > 0 ? filter.TopCount : 10) // Fixed: Explicitly check for a valid TopCount value
                .ToListAsync();

            var result = new List<SellerTopProductDto>();
            foreach (var item in topProducts)
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Include(p => p.ProductBrand)
                    .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                result.Add(new SellerTopProductDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Category = product?.Category?.Name ?? "Unknown",
                    Brand = product?.ProductBrand?.Name ?? "Unknown",
                    QuantitySold = item.TotalQuantity,
                    Revenue = item.TotalRevenue,
                    Price = item.TotalQuantity > 0 ? item.TotalRevenue / item.TotalQuantity : 0, // Fixed: prevent division by zero
                    PhotoUrl = item.PhotoUrl
                });
            }

            return result;
        }
    }
}