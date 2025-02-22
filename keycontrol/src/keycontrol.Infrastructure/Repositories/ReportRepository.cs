using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

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

    public async Task<Report> FindReportByExternalId(Guid externalId)
    {
        return await _appDbContext.reports.Where(r => r.ExternalId == externalId).FirstOrDefaultAsync();
    }
}