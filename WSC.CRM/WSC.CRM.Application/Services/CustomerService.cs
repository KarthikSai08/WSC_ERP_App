using AutoMapper;
using Microsoft.Extensions.Logging;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos.CRMLayer;
using WSC.Shared.Contracts.Exceptions;
using WSC.Shared.Contracts.ValueObjects;

namespace WSC.CRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ICustomerRepository repo, IMapper mapper, ILogger<CustomerService> logger)
        {
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<int>> CreateCustomerAsync(CreateCustomerDto dto, CancellationToken ct)
        {
            _logger.LogInformation("Customer Creation initiated for email: {Email} at {Time}", dto.CxEmail, DateTime.UtcNow);
            var exist = await _repo.ExistsByEmailAsync(dto.CxEmail, ct);
            if (exist)
                throw new DuplicateException("Customer", dto.CxEmail);

            var customer = _mapper.Map<Customer>(dto);
            customer.CxAddress ??= new Address();
            customer.CxAddress.Street = dto.Street;
            customer.CxAddress.City = dto.City;
            customer.CxAddress.State = dto.State;
            customer.CxAddress.ZipCode = dto.ZipCode;
            customer.CxAddress.Country = dto.Country;
            var newCxId = await _repo.CreateCustomerAsync(customer, ct);

            _logger.LogInformation("Customer created with ID: {CustomerId} at {Time}", newCxId, DateTime.UtcNow);

            return ApiResponse<int>.Ok(newCxId, "Customer created Successfully");

        }

        public async Task<ApiResponse<bool>> DeleteCustomerAsync(int id, CancellationToken ct)
        {
            _logger.LogInformation("Customer Deactivation initiated for ID: {CustomerId} at {Time}", id, DateTime.UtcNow);
            var result = await _repo.DeleteCustomerAsync(id, ct);
            if (result)
            {
                _logger.LogInformation("Customer deactivated with ID: {CustomerId} at {Time}", id, DateTime.UtcNow);
            }
            return result
               ? ApiResponse<bool>.Ok(true, "Customer DeActivated Successfully")
               : ApiResponse<bool>.Failed("Customer deactivation failed!!");
        }

        public async Task<ApiResponse<IEnumerable<CustomerResponseDto>>> GetAllAsync(CancellationToken ct)
        {

            _logger.LogInformation("All Customers retrieval initiated at {Time}", DateTime.UtcNow);
            var customers = await _repo.GetAllCustomersAsync(ct);

            if (customers == null || !customers.Any())
                return ApiResponse<IEnumerable<CustomerResponseDto>>.Ok(new List<CustomerResponseDto>(), "No Customers found");

            var allCustomers = _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
            _logger.LogInformation("Retrieved {CustomerCount} customers at {Time}", allCustomers.Count(), DateTime.UtcNow);

            return ApiResponse<IEnumerable<CustomerResponseDto>>.Ok(allCustomers, "Customers retrived Successfully");
        }

        public async Task<ApiResponse<CustomerResponseDto?>> GetByIdAsync(int id, CancellationToken ct)
        {
            _logger.LogInformation("Customer retrieval initiated for ID: {CustomerId} at {Time}", id, DateTime.UtcNow);
            if (id <= 0)
                throw new Exception("Id must be greater than 0");

            var customer = await _repo.GetCustomerByIdAsync(id, ct);

            if (customer == null)
                throw new NotFoundException("Customer", id);

            var result = _mapper.Map<CustomerResponseDto>(customer);
            _logger.LogInformation("Customer found with ID: {CustomerId} at {Time}", id, DateTime.UtcNow);

            return ApiResponse<CustomerResponseDto>.Ok(result, "Customer Found!");
        }


        public async Task<ApiResponse<bool>> UpdateCustomerAsync(UpdateCustomerDto dto, CancellationToken ct)
        {
            _logger.LogInformation("Customer update initiated for ID: {CustomerId} at {Time}", dto.CxId, DateTime.UtcNow);
            var customer = await _repo.GetCustomerByIdAsync(dto.CxId, ct);
            if (customer == null)
                throw new NotFoundException("Customer", dto.CxId);

            _mapper.Map(dto, customer);

            var updatedCustomer = await _repo.UpdateCustomerAsync(customer, ct);
            _logger.LogInformation("Customer update {Status} for ID: {CustomerId} at {Time}", updatedCustomer ? "succeeded" : "failed", dto.CxId, DateTime.UtcNow); 

            return updatedCustomer
                ? ApiResponse<bool>.Ok(true, "Updated Successfully")
                : ApiResponse<bool>.Failed("Customer update Failed");
        }
        public async Task<ApiResponse<PagedResponse<CustomerResponseDto>>> GetCustomersAsync(PaginationRequest request, CancellationToken ct)
        {
            _logger.LogInformation("Paged Customers retrieval initiated for PageNumber: {PageNumber}, PageSize: {PageSize} at {Time}", request.PageNumber, request.PageSize, DateTime.UtcNow);
            var (data, totalCount) = await _repo.GetPagedCustomersAsync(request, ct);

            var response = new PagedResponse<CustomerResponseDto>(
                data,
                request.PageNumber,
                request.PageSize,
                totalCount
            );
            _logger.LogInformation("Retrieved {CustomerCount} customers for PageNumber: {PageNumber}, PageSize: {PageSize} at {Time}", data.Count(), request.PageNumber, request.PageSize, DateTime.UtcNow);
            return ApiResponse<PagedResponse<CustomerResponseDto>>
                .Ok(response, "Leads retrieved successfully.");
        }
    }
}
