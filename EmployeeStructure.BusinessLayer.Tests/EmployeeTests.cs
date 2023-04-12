using NUnit.Framework;
using NSubstitute;

namespace EmployeeStructure.BusinessLayer.Tests;

[TestFixture]
public class EmployeeTests
{
    [Test]
    public void Constructor_ShouldFillProvidedValues()
    {
        var name = "Test Employee";
        var hireDate = DateTime.Today;
        var salary = 1000;
        var superior = Substitute.For<IHasSubordinates>();

        var employee = new Employee(name, DateOnly.FromDateTime(hireDate), salary, superior);

        Assert.That(employee.Name, Is.EqualTo(name));
        Assert.That(employee.HireDate, Is.EqualTo(DateOnly.FromDateTime(hireDate)));
        Assert.That(employee.Salary, Is.EqualTo(salary));
        Assert.That(employee.Superior, Is.SameAs(superior));
    }

    [Test]
    public void Constructor_NoSuperior_ShouldSetSuperiorToNull()
    {
        var name = "Test Employee";
        var hireDate = DateTime.Today;
        var salary = 1000;

        var employee = new Employee(name, DateOnly.FromDateTime(hireDate), salary);

        Assert.That(employee.Name, Is.EqualTo(name));
        Assert.That(employee.HireDate, Is.EqualTo(DateOnly.FromDateTime(hireDate)));
        Assert.That(employee.Salary, Is.EqualTo(salary));
        Assert.That(employee.Superior, Is.Null);
    }

    [Test]
    public void Constructor_NoSalary_ShouldSetSalaryToDefault()
    {
        var name = "Test Employee";
        var hireDate = DateTime.Today;

        var employee = new Employee(name, DateOnly.FromDateTime(hireDate));

        Assert.That(employee.Name, Is.EqualTo(name));
        Assert.That(employee.HireDate, Is.EqualTo(DateOnly.FromDateTime(hireDate)));
        Assert.That(employee.Salary, Is.EqualTo(Employee.DefaultSalary));
    }

    [Test]
    public void Constructor_EmptyName_ShouldThrowException()
    {
        var name = string.Empty;
        var hireDate = DateTime.Today;

        Assert.That(() => new Employee(name, DateOnly.FromDateTime(hireDate)), Throws.ArgumentNullException);
    }

    [Test]
    public void ToString_ShouldReturn_EmployeeName()
    {
        var name = "Test Employee";
        var hireDate = DateTime.Today;

        var employee = new Employee(name, DateOnly.FromDateTime(hireDate));

        Assert.That(employee.ToString(), Is.EqualTo(name));
    }
}