namespace EmployeeStructure.BusinessLayer;

/// <summary>
/// Represents higher-level employee that may have subordinate employees
/// </summary>
public interface IHasSubordinates
{
    /// <summary>
    /// Subordinate employees
    /// </summary>
    IEnumerable<Employee> Subordinates { get; }

    /// <summary>
    /// Adds subordinate employee
    /// </summary>
    /// <param name="employee">Employee subordinate to this superior</param>
    void AddSubordinate(Employee employee);

    /// <summary>
    /// Adds multiple subordinate employees
    /// </summary>
    /// <param name="employees">Collection of employees to add</param>
    void AddSubordinates(IEnumerable<Employee> employees)
    {
        if (employees is null)
            return;

        foreach (var employee in employees)
            AddSubordinate(employee);
    }

    /// <summary>
    /// Removes subordinate employee
    /// </summary>
    /// <param name="employee">Subordinate employee to remove</param>
    void RemoveSubordinate(Employee employee);
}
