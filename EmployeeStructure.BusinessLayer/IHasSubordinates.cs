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
    /// Removes subordinate employee
    /// </summary>
    /// <param name="employee">Subordinate employee to remove</param>
    void RemoveSubordinate(Employee employee);
}
