using EmployeeStructure.BusinessLayer.Extensions;

namespace EmployeeStructure.BusinessLayer;

/// <summary>
/// The company's employee
/// </summary>
public class Employee
{
    /// <summary>
    /// Default employee salary
    /// </summary>
    public const decimal DefaultSalary = 100;

    private IHasSubordinates? _superior;

    /// <summary>
    /// Creates new employee 
    /// </summary>
    /// <param name="name">Employee's name</param>
    /// <param name="hireDate">Date when the employee was hired</param>
    /// <param name="salary">Employee's salary</param>
    /// <param name="superior">Employee's manager</param>
    public Employee(string name, DateOnly hireDate, decimal salary = DefaultSalary, IHasSubordinates? superior = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name), "Employee's name can't be empty");

        Name = name;
        HireDate = hireDate;
        Salary = salary;
        Superior = superior;
    }

    /// <summary>
    /// Employee's name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Date when this employee was hired
    /// </summary>
    public DateOnly HireDate { get; }

    /// <summary>
    /// Employee's salary
    /// </summary>
    public decimal Salary { get; }

    /// <summary>
    /// Employee's manager
    /// </summary>
    public IHasSubordinates? Superior
    {
        get => _superior;
        set
        {
            if (_superior == value)
                return;

            _superior?.RemoveSubordinate(this);
            value?.AddSubordinate(this);
        }
    }

    /// <summary>
    /// Percentage of salary increase for each whole year of employment
    /// </summary>
    protected virtual int YearlyPremiumPercent => 3;

    /// <summary>
    /// Maximum percentage of salary increase regardless of employment length
    /// </summary>
    protected virtual int MaximumPremiumPercent => 30;

    public virtual decimal GetNetSalaryOnDate(DateOnly date)
    {
        if (date < HireDate)
            throw new ArgumentException("Payroll date can't be before the employee's hire date", nameof(date));

        var yearsOfEmployment = HireDate.DifferenceInYears(date);
        var premiumPercent = Math.Min(yearsOfEmployment * YearlyPremiumPercent, MaximumPremiumPercent);
        return Salary * (100 + premiumPercent) / 100;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return Name;
    }

    /// <summary>
    /// Internal use only method to set superior value bypassing setter
    /// </summary>
    /// <param name="superior">Managing employee</param>
    internal void SetSuperiorDirect(IHasSubordinates? superior)
    {
        _superior = superior;
    }
}
