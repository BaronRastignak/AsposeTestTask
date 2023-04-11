﻿namespace EmployeeStructure.BusinessLayer;

/// <summary>
/// Manager level employee that can have subordinate employees
/// </summary>
public class Manager : Employee, IHasSubordinates
{
    /// <summary>
    /// Create new manager employee
    /// </summary>
    /// <param name="name">Manager's name</param>
    /// <param name="hireDate">Date when this manager was hired</param>
    /// <param name="salary">Manager's salary</param>
    /// <param name="superior">Manager's superior manager</param>
    public Manager(string name, DateOnly hireDate, decimal salary = DefaultSalary, IHasSubordinates? superior = null)
        : base(name, hireDate, salary, superior)
    {
        _subordinates = new List<Employee>();
    }

    private readonly List<Employee> _subordinates;

    /// <inheritdoc/>
    public IEnumerable<Employee> Subordinates => _subordinates.AsReadOnly();
}