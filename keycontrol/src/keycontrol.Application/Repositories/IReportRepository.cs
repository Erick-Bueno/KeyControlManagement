using keycontrol.Domain.Entities;
namespace keycontrol.Application.Repositories;
public interface IReportRepository
{
    public Task<Report> AddReport(Report report);
}