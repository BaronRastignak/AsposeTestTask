using NUnit.Framework;

namespace EmployeeStructure.BusinessLayer.Tests;

[TestFixture]
public class SalesTests
{
    [Test]
    public void AddSubordinate_ShouldSetOurselvesAsSuperiorAndAddEmployeeToSubordinates()
    {
        var sales = new Sales("Test Sales", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today));

        sales.AddSubordinate(employee);

        Assert.That(employee.Superior, Is.SameAs(sales));
        CollectionAssert.Contains(sales.Subordinates, employee);
    }

    [Test]
    public void AddSubordinate_AlreadySubordinatedEmployee_ShouldThrowException()
    {
        var sales = new Sales("Test Sales", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: sales);

        Assume.That(employee.Superior, Is.SameAs(sales));

        Assert.That(() => sales.AddSubordinate(employee), Throws.InvalidOperationException);
    }

    [Test]
    public void RemoveSubordinate_ShouldClearEmployeeSuperiorAndRemoveEmployeeFromSubordinates()
    {
        var sales = new Sales("Test Sales", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: sales);

        sales.RemoveSubordinate(employee);

        Assert.That(employee.Superior, Is.Null);
        CollectionAssert.DoesNotContain(sales.Subordinates, employee);
    }

    [Test]
    public void RemoveSubordinate_EmployeeNotSubordinatedToThis_ShouldThrowException()
    {
        var sales = new Sales("Test Sales", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today));

        Assume.That(employee.Superior, Is.Not.SameAs(sales));

        Assert.That(() => sales.RemoveSubordinate(employee), Throws.InvalidOperationException);
    }

    [Test]
    public void SuperiorSetter_DirectSetToManager_ShouldAddEmployeeToManagerSubordinates()
    {
        var sales = new Sales("Test Sales", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today));

        Assume.That(employee.Superior, Is.Not.SameAs(sales));

        employee.Superior = sales;

        CollectionAssert.Contains(sales.Subordinates, employee);
    }

    [Test]
    public void SuperiorSetter_DirectSetToNull_ShouldRemoveEmployeeFromManagerSubordinates()
    {
        var sales = new Sales("Test Sales", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: sales);

        Assume.That(employee.Superior, Is.SameAs(sales));

        employee.Superior = null;

        CollectionAssert.DoesNotContain(sales.Subordinates, employee);
    }

    [Test]
    public void SuperiorSetter_ReplacingEmployeeSuperior_ShouldRemoveEmployeeFromOldSubordinatesAndAddToNewSubordinates()
    {
        var oldSales = new Sales("Test Sales Old", DateOnly.FromDateTime(DateTime.Today));
        var newSales = new Sales("Test Sales New", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: oldSales);

        Assume.That(employee.Superior, Is.SameAs(oldSales));

        employee.Superior = newSales;

        CollectionAssert.DoesNotContain(oldSales.Subordinates, employee);
        CollectionAssert.Contains(newSales.Subordinates, employee);
    }

    [Test]
    public void ToString_ShouldReturn_SalesName()
    {
        var name = "Test Sales";
        var hireDate = DateTime.Today;

        var sales = new Sales(name, DateOnly.FromDateTime(hireDate));

        Assert.That(sales.ToString(), Is.EqualTo($"Sales: {name}"));
    }
}