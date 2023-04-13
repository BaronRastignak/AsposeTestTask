using EmployeeStructure.BusinessLayer;

namespace EmployeeStructure.API.Models;

/// <summary>
/// Employee data
/// </summary>
public record EmployeeDTO
{
    public EmployeeDTO(int id, Employee employee)
    {
        Id = id;
        Name = employee.Name;
        HireDate = employee.HireDate;
        BaseSalary = employee.Salary;
        SuperiorName = (employee.Superior as Employee)?.Name ?? string.Empty;
    }

    public int Id { get; }

    public string Name { get; }

    public DateOnly HireDate { get; }

    public decimal BaseSalary { get; }

    public string SuperiorName { get; }
}
