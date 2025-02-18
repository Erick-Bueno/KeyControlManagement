using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;

namespace keycontrol.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _appDbContext;

    public ReportRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<Report> AddReport(Report report)
    {
        await _appDbContext.AddAsync(report);
        await _appDbContext.SaveChangesAsync();
        return report;
    }
}