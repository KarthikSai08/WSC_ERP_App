using System;
using System.Collections.Generic;
using System.Text;
using WSC.Gateway.Application.Dtos.AggregatorDtos;
using WSC.Gateway.Domain.Entities;

namespace WSC.Gateway.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<Users?> GetUserByEmailAsync(string email, CancellationToken ct);
        Task<Users?> GetUserByIdAsync(int userId, CancellationToken ct);
        Task<int> CreateUserAsync(Users user, CancellationToken ct);
        Task UpdateLastLoginAsync(int userId, CancellationToken ct);
        Task<bool> EmailExistsAsync(string email, CancellationToken ct);

    }
}
