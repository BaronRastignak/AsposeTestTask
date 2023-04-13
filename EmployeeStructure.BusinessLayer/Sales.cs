namespace EmployeeStructure.BusinessLayer;

/// <summary>
/// Sales employee that can have subordinate employees
/// </summary>
public class Sales : Employee, IHasSubordinates
{
    /// <summary>
    /// Create new sales employee
    /// </summary>
    /// <param name="name">Employee's name</param>
    /// <param name="hireDate">Date when this employee was hired</param>
    /// <param name="salary">Employee's salary</param>
    /// <param name="superior">Employee's manager</param>
    public Sales(string name, DateOnly hireDate, decimal salary = DefaultSalary, IHasSubordinates? superior = null)
        : base(name, hireDate, salary, superior)
    {
        _subordinates = new List<Employee>();
        _subordinatesPremiumPercent = 0.3m;
    }

    private readonly List<Employee> _subordinates;
    private readonly decimal _subordinatesPremiumPercent;

    /// <inheritdoc/>
    public IEnumerable<Employee> Subordinates => _subordinates.AsReadOnly();

    /// <inheritdoc/>
    protected override decimal YearlyPremiumPercent => 1;

    /// <inheritdoc/>
    protected override decimal MaximumPremiumPercent => 35;

    /// <inheritdoc/>
    public void AddSubordinate(Employee employee)
    {
        if (employee.Superior == this)
            throw new InvalidOperationException($"The employee {employee} is already subordinated to this sales employee");

        employee.SetSuperiorDirect(this);
        _subordinates.Add(employee);
    }

    /// <inheritdoc/>
    public void RemoveSubordinate(Employee employee)
    {
        if (employee.Superior != this)
            throw new InvalidOperationException($"The employee {employee} isn't subordinated to this sales employee");

        employee.SetSuperiorDirect(null);
        _subordinates.Remove(employee);
    }

    /// <inheritdoc/>
    public override decimal GetNetSalaryOnDate(DateOnly date)
    {
        var baseSalary = base.GetNetSalaryOnDate(date);
        var subordinatesSalaries = Subordinates
            .Sum(sub => GetNetSalariesFromHierarchy(sub, date));

        return baseSalary + subordinatesSalaries * _subordinatesPremiumPercent / 100;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Sales: {base.ToString()}";
    }

    private static decimal GetNetSalariesFromHierarchy(Employee employee, DateOnly date)
    {
        decimal baseSalary;
        try
        {
            baseSalary = employee.GetNetSalaryOnDate(date);
        }
        catch (ArgumentException)
        {
            baseSalary = 0;
        }

        if (employee is not IHasSubordinates superior)
            return baseSalary;

        var subordinatesSalaries = superior.Subordinates
            .Sum(sub => GetNetSalariesFromHierarchy(sub, date));
        return baseSalary + subordinatesSalaries;
    }
}
