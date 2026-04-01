using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Dtos;
using WSC.Shared.Contracts.Enums;

namespace WSC.CRM.Application.Interfaces.Repository
{
    public interface ILeadRepository
    {
        Task<IEnumerable<LeadResponseDto>> GetAllLeadsAsync(CancellationToken ct);
        Task<LeadResponseDto?> GetLeadByIdAsync(int id, CancellationToken ct);
        Task<int> CreateLeadAsync(Lead lead, CancellationToken ct);
        Task<bool> ExistsByLeadAsync(string email, CancellationToken ct);
        Task<bool> UpdateLeadAsync(Lead lead, CancellationToken ct);
        Task<bool> DeleteLeadAsync(int id, CancellationToken ct);
        Task<Lead?> GetLeadEntityByIdAsync(int id, CancellationToken ct);
        Task<bool> UpdateLeadStatusAsync(int id, LeadStatus newStatus, CancellationToken ct);
    }
}
