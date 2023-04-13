namespace EmployeeStructure.API.Models;

/// <summary>
/// Employee salary data for the given date
/// </summary>
public record SalaryDTO
{
    public SalaryDTO(DateOnly payrollDate, decimal salary, EmployeeDTO? employee = null)
    {
        Employee = employee;
        PayrollDate = payrollDate;
        Salary = salary;
    }

    public EmployeeDTO? Employee { get; }

    public DateOnly PayrollDate { get; }

    public decimal Salary { get; }
}
