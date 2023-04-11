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
    }

    private readonly List<Employee> _subordinates;

    /// <inheritdoc/>
    public IEnumerable<Employee> Subordinates => _subordinates.AsReadOnly();
}
