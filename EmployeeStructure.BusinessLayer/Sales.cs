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

    /// <inheritdoc/>
    public void AddSubordinate(Employee employee)
    {
        if (employee.Superior == this)
            throw new InvalidOperationException($"The employee {employee} is already subordinated to this sales employee");

        employee.SuperiorAdded += EmployeeOnSuperiorAdded;
        employee.SuperiorRemoved += EmployeeOnSuperiorRemoved;
        employee.Superior = this;
    }

    /// <inheritdoc/>
    public void RemoveSubordinate(Employee employee)
    {
        if (employee.Superior != this)
            throw new InvalidOperationException($"The employee {employee} isn't subordinated to this sales employee");

        employee.Superior = null;
        employee.SuperiorAdded -= EmployeeOnSuperiorAdded;
        employee.SuperiorRemoved -= EmployeeOnSuperiorRemoved;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return $"Sales: {base.ToString()}";
    }

    /// <summary>
    /// Adds an employee to the subordinates collection when this sales employee is set as the employee's superior
    /// </summary>
    private void EmployeeOnSuperiorAdded(object? sender, EventArgs e)
    {
        if (sender is not Employee employee)
            return;

        _subordinates.Add(employee);
    }

    /// <summary>
    /// Removes an employee from the subordinates collection when this sales employee is this employee's superior no more
    /// </summary>
    private void EmployeeOnSuperiorRemoved(object? sender, EventArgs e)
    {
        if (sender is not Employee employee)
            return;

        _subordinates.Remove(employee);
    }
}
