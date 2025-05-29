using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrimeCare.Application.Services.Interfaces;
using PrimeCare.Shared.Dtos.Reports;

namespace PrimeCare.Api.Controllers
{
    /// <summary>
    /// API controller for generating reports and analytics for admin and seller dashboards.
    /// </summary>
    [ApiController]
    [Route("api/v1/reports")]
    [Authorize] // All endpoints require authentication
    public class ReportsController : BaseApiController
    {
        private readonly IReportsService _reportsService;

        public ReportsController(IReportsService reportsService)
        {
            _reportsService = reportsService;
        }

        /// <summary>
        /// Get full admin dashboard report with filters.
        /// Requires Admin role.
        /// </summary>
        [HttpGet("admin/dashboard")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<AdminReportDto>> GetAdminDashboardReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? brandId = null,
            [FromQuery] int topProductsLimit = 10)
        {
            var filter = new ReportFilterDto
            {
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = categoryId,
                BrandId = brandId,
                TopProductsLimit = topProductsLimit
            };

            var report = await _reportsService.GetAdminReportAsync(filter);
            return Ok(report);
        }

        /// <summary>
        /// Get seller dashboard report.
        /// Admins can specify any sellerId; sellers can only see their own.
        /// </summary>
        [HttpGet("seller/dashboard")]
        [Authorize(Policy = "AdminOrSeller")]
        public async Task<ActionResult<SellerReportDto>> GetSellerDashboardReport(
            [FromQuery] string? sellerId = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? brandId = null,
            [FromQuery] int topProductsLimit = 10)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            // Non-admin users can only access their own data
            if (userRole != "Admin")
            {
                sellerId = currentUserId;
            }

            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest("Seller ID is required for admin users.");
            }

            var filter = new ReportFilterDto
            {
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = categoryId,
                BrandId = brandId,
                TopProductsLimit = topProductsLimit
            };

            var report = await _reportsService.GetSellerReportAsync(sellerId, filter);
            return Ok(report);
        }

        /// <summary>
        /// Get order status summary for admin.
        /// Requires Admin role.
        /// </summary>
        [HttpGet("admin/order-status-summary")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<OrderStatusSummaryDto>> GetOrderStatusSummary(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var filter = new ReportFilterDto
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var summary = await _reportsService.GetOrderStatusSummaryAsync(filter);
            return Ok(summary);
        }

        /// <summary>
        /// Get top selling products platform-wide.
        /// Requires Admin role.
        /// </summary>
        [HttpGet("admin/top-selling-products")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<ActionResult<List<TopSellingProductDto>>> GetTopSellingProducts(
            [FromQuery] int limit = 10,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? brandId = null)
        {
            var filter = new ReportFilterDto
            {
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = categoryId,
                BrandId = brandId,
                TopProductsLimit = limit
            };

            var products = await _reportsService.GetTopSellingProductsAsync(filter);
            return Ok(products);
        }

        /// <summary>
        /// Get top selling products for a seller.
        /// Admins can specify any sellerId; sellers can only see their own.
        /// </summary>
        [HttpGet("seller/top-selling-products")]
        [Authorize(Policy = "AdminOrSeller")]
        public async Task<ActionResult<List<SellerTopProductDto>>> GetSellerTopSellingProducts(
            [FromQuery] string? sellerId = null,
            [FromQuery] int limit = 10,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int? categoryId = null,
            [FromQuery] int? brandId = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin")
            {
                sellerId = currentUserId;
            }

            if (string.IsNullOrEmpty(sellerId))
            {
                return BadRequest("Seller ID is required for admin users.");
            }

            var filter = new ReportFilterDto
            {
                StartDate = startDate,
                EndDate = endDate,
                CategoryId = categoryId,
                BrandId = brandId,
                TopProductsLimit = limit
            };

            var products = await _reportsService.GetSellerTopProductsAsync(sellerId, filter);
            return Ok(products);
        }

        /// <summary>
        /// Get daily revenue data for charts.
        /// Admins see all data; sellers see their own only.
        /// </summary>
        [HttpGet("daily-revenue")]
        [Authorize(Policy = "AdminOrSeller")]
        public async Task<ActionResult<List<RevenueChartDataDto>>> GetDailyRevenue(
            [FromQuery] int days = 30,
            [FromQuery] string? sellerId = null)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userRole = User.FindFirstValue(ClaimTypes.Role);

            if (userRole != "Admin")
            {
                sellerId = currentUserId;
            }

            var filter = new ReportFilterDto
            {
                StartDate = DateTime.UtcNow.Date.AddDays(-days),
                EndDate = DateTime.UtcNow.Date.AddDays(1)
            }; ;

            var revenueData = sellerId == null
                ? await _reportsService.GetDailyRevenueAsync(filter)
                : await _reportsService.GetSellerDailyRevenueAsync(sellerId, filter);

            return Ok(revenueData);
        }
    }
}
