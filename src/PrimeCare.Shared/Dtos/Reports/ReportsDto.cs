// src/PrimeCare.Shared/Dtos/Reports/AdminReportDto.cs
namespace PrimeCare.Shared.Dtos.Reports
{
    /// <summary>
    /// Admin dashboard report summary
    /// </summary>
    public class AdminReportDto
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public OrderStatusSummaryDto OrderStatusSummary { get; set; } = null!;
        public List<TopSellingProductDto> TopSellingProducts { get; set; } = new();
        public List<RevenueChartDataDto> DailyRevenue { get; set; } = new();
        public List<RevenueChartDataDto> MonthlyRevenue { get; set; } = new();
    }

    /// <summary>
    /// Order status breakdown
    /// </summary>
    public class OrderStatusSummaryDto
    {
        public int PendingOrders { get; set; }
        public int ProcessingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
        public decimal PendingRevenue { get; set; }
        public decimal ProcessingRevenue { get; set; }
        public decimal CompletedRevenue { get; set; }
    }

    /// <summary>
    /// Top selling product information
    /// </summary>
    public class TopSellingProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int TotalQuantitySold { get; set; }
        public decimal TotalRevenue { get; set; }
        public string? PhotoUrl { get; set; }
    }

    /// <summary>
    /// Revenue chart data point
    /// </summary>
    public class RevenueChartDataDto
    {
        public string Period { get; set; } = null!; // Date string or month name
        public decimal Revenue { get; set; }
        public int OrderCount { get; set; }
    }
}

// src/PrimeCare.Shared/Dtos/Reports/SellerReportDto.cs
namespace PrimeCare.Shared.Dtos.Reports
{
    /// <summary>
    /// Seller dashboard report summary
    /// </summary>
    public class SellerReportDto
    {
        public string SellerId { get; set; } = null!;
        public string SellerName { get; set; } = null!;
        public int TotalProductsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public List<SellerTopProductDto> TopSellingProducts { get; set; } = new();
        public List<RevenueChartDataDto> MonthlyRevenue { get; set; } = new();
    }

    /// <summary>
    /// Seller's top selling product
    /// </summary>
    public class SellerTopProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string Brand { get; set; } = null!;
        public int QuantitySold { get; set; }
        public decimal Revenue { get; set; }
        public decimal Price { get; set; }
        public string? PhotoUrl { get; set; }
    }

    /// <summary>
    /// Report filter parameters
    /// </summary>
    public class ReportFilterDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public int TopCount { get; set; } = 10; // Default top 10
        public int TopProductsLimit { get; set; }
    }
}