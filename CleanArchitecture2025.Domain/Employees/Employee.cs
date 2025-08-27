using CleanArchitecture2025.Domain.Abstractions;

namespace CleanArchitecture2025.Domain.Employees;

public sealed class Employee : Entity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);
    public DateOnly BirthDate { get; set; }
    public decimal Salary { get; set; }
    public Address Address { get; set; } = default!;
    public PersonalInformation PersonalInformation { get; set; } = default!;
}