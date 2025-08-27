using CleanArchitecture2025.Domain.Employees;
using CleanArchitecture2025.Infrastructure.Context;
using GenericRepository;

namespace CleanArchitecture2025.Infrastructure.Repositories;

public sealed class EmployeeRepository(ApplicationDbContext context) : Repository<Employee, ApplicationDbContext>(context), IEmployeeRepository
{
}