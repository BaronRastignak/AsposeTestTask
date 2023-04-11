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
}
