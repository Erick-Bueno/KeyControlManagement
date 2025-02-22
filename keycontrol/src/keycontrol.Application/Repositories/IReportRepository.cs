using keycontrol.Domain.Entities;
namespace keycontrol.Application.Repositories;
public interface IReportRepository
{
    public Task<Report> AddReport(Report report);
    public Task<Report> FindReportByExternalId(Guid externalId);
}