using PrimeCare.Shared.Dtos.Reports;

namespace PrimeCare.Application.Services.Interfaces
{
    public interface IReportsService
    {
        Task<AdminReportDto> GetAdminReportAsync(ReportFilterDto filter);

        Task<SellerReportDto> GetSellerReportAsync(string sellerId, ReportFilterDto filter);

        Task<OrderStatusSummaryDto> GetOrderStatusSummaryAsync(ReportFilterDto filter);

        Task<List<TopSellingProductDto>> GetTopSellingProductsAsync(ReportFilterDto filter);

        Task<List<RevenueChartDataDto>> GetDailyRevenueAsync(ReportFilterDto filter);

        Task<List<RevenueChartDataDto>> GetMonthlyRevenueAsync(ReportFilterDto filter);

        Task<List<SellerTopProductDto>> GetSellerTopProductsAsync(string sellerId, ReportFilterDto filter);

        Task<List<RevenueChartDataDto>> GetSellerDailyRevenueAsync(string sellerId, ReportFilterDto filter);
    }
}
