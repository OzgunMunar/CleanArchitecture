using CleanArchitecture2025.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture2025.Infrastructure.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {

        builder.OwnsOne(p => p.PersonalInformation, builder =>
        {
            builder.Property(i => i.TcNo).HasColumnName("TcNo");
            builder.Property(i => i.Email).HasColumnName("Email");
            builder.Property(i => i.PhoneNumber1).HasColumnName("PhoneNumber1");
            builder.Property(i => i.PhoneNumber2).HasColumnName("PhoneNumber2");
        });

        builder.OwnsOne(p => p.Address, builder =>
        {
            builder.Property(i => i.City).HasColumnName("City");
            builder.Property(i => i.Country).HasColumnName("Country");
            builder.Property(i => i.Street).HasColumnName("Street");
        });

        builder.Property(x => x.Salary).HasColumnType("money");

    }
}