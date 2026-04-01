using AutoMapper;
using WSC.CRM.Application.Dtos;
using WSC.CRM.Application.Interfaces.Repository;
using WSC.CRM.Application.Interfaces.Services;
using WSC.CRM.Domain.Entities;
using WSC.Shared.Contracts.Common;
using WSC.Shared.Contracts.Dtos;
using WSC.Shared.Contracts.Exceptions;
using WSC.Shared.Contracts.ValueObjects;

namespace WSC.CRM.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<ApiResponse<int>> CreateCustomerAsync(CreateCustomerDto dto, CancellationToken ct)
        {
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

            return ApiResponse<int>.Ok(newCxId, "Customer created Successfully");
        }

        public async Task<ApiResponse<bool>> DeleteCustomerAsync(int id, CancellationToken ct)
        {
            var result = await _repo.DeleteCustomerAsync(id, ct);
            return result
               ? ApiResponse<bool>.Ok(true, "Customer DeActivated Successfully")
               : ApiResponse<bool>.Failed("Customer deactivation failed!!");
        }

        public async Task<ApiResponse<IEnumerable<CustomerResponseDto>>> GetAllAsync(CancellationToken ct)
        {
            var customers = await _repo.GetAllCustomersAsync(ct);

            if (customers == null || !customers.Any())
                return ApiResponse<IEnumerable<CustomerResponseDto>>.Ok(new List<CustomerResponseDto>(), "No Customers found");

            var allCustomers = _mapper.Map<IEnumerable<CustomerResponseDto>>(customers);
            return ApiResponse<IEnumerable<CustomerResponseDto>>.Ok(allCustomers, "Customers retrived Successfully");
        }

        public async Task<ApiResponse<CustomerResponseDto?>> GetByIdAsync(int id, CancellationToken ct)
        {
            if (id <= 0)
                throw new Exception("Id must be greater than 0");

            var customer = await _repo.GetCustomerByIdAsync(id, ct);

            if (customer == null)
                throw new NotFoundException("Customer", id);

            var result = _mapper.Map<CustomerResponseDto>(customer);
            return ApiResponse<CustomerResponseDto>.Ok(result, "Customer Found!");
        }


        public async Task<ApiResponse<bool>> UpdateCustomerAsync(UpdateCustomerDto dto, CancellationToken ct)
        {
            var customer = await _repo.GetCustomerByIdAsync(dto.CxId, ct);
            if (customer == null)
                throw new NotFoundException("Customer", dto.CxId);

            _mapper.Map(dto, customer);

            var updatedCustomer = await _repo.UpdateCustomerAsync(customer, ct);

            return updatedCustomer
                ? ApiResponse<bool>.Ok(true, "Updated Successfully")
                : ApiResponse<bool>.Failed("Customer update Failed");
        }
        public async Task<ApiResponse<PagedResponse<CustomerResponseDto>>> GetCustomersAsync(PaginationRequest request, CancellationToken ct)
        {
            var (data, totalCount) = await _repo.GetPagedCustomersAsync(request, ct);

            var response = new PagedResponse<CustomerResponseDto>(
                data,
                request.PageNumber,
                request.PageSize,
                totalCount
            );

            return ApiResponse<PagedResponse<CustomerResponseDto>>
                .Ok(response, "Leads retrieved successfully.");
        }
    }
}
