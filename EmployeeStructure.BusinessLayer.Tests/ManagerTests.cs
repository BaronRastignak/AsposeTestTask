using NUnit.Framework;

namespace EmployeeStructure.BusinessLayer.Tests;

[TestFixture]
public class ManagerTests
{
    [Test]
    public void AddSubordinate_ShouldSetOurselvesAsSuperiorAndAddEmployeeToSubordinates()
    {
        var manager = new Manager("Test Manager", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today));

        manager.AddSubordinate(employee);

        Assert.That(employee.Superior, Is.SameAs(manager));
        CollectionAssert.Contains(manager.Subordinates, employee);
    }

    [Test]
    public void AddSubordinate_AlreadySubordinatedEmployee_ShouldThrowException()
    {
        var manager = new Manager("Test Manager", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: manager);

        Assume.That(employee.Superior, Is.SameAs(manager));

        Assert.That(() => manager.AddSubordinate(employee), Throws.InvalidOperationException);
    }

    [Test]
    public void RemoveSubordinate_ShouldClearEmployeeSuperiorAndRemoveEmployeeFromSubordinates()
    {
        var manager = new Manager("Test Manager", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: manager);

        manager.RemoveSubordinate(employee);

        Assert.That(employee.Superior, Is.Null);
        CollectionAssert.DoesNotContain(manager.Subordinates, employee);
    }

    [Test]
    public void RemoveSubordinate_EmployeeNotSubordinatedToThis_ShouldThrowException()
    {
        var manager = new Manager("Test Manager", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today));

        Assume.That(employee.Superior, Is.Not.SameAs(manager));

        Assert.That(() => manager.RemoveSubordinate(employee), Throws.InvalidOperationException);
    }

    [Test]
    public void SuperiorSetter_DirectSetToManager_ShouldAddEmployeeToManagerSubordinates()
    {
        var manager = new Manager("Test Manager", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today));

        Assume.That(employee.Superior, Is.Not.SameAs(manager));

        employee.Superior = manager;

        CollectionAssert.Contains(manager.Subordinates, employee);
    }

    [Test]
    public void SuperiorSetter_DirectSetToNull_ShouldRemoveEmployeeFromManagerSubordinates()
    {
        var manager = new Manager("Test Manager", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: manager);

        Assume.That(employee.Superior, Is.SameAs(manager));

        employee.Superior = null;

        CollectionAssert.DoesNotContain(manager.Subordinates, employee);
    }

    [Test]
    public void SuperiorSetter_ReplacingEmployeeSuperior_ShouldRemoveEmployeeFromOldSubordinatesAndAddToNewSubordinates()
    {
        var oldManager = new Manager("Test Manager Old", DateOnly.FromDateTime(DateTime.Today));
        var newManager = new Manager("Test Manager New", DateOnly.FromDateTime(DateTime.Today));
        var employee = new Employee("Test Employee", DateOnly.FromDateTime(DateTime.Today), superior: oldManager);

        Assume.That(employee.Superior, Is.SameAs(oldManager));

        employee.Superior = newManager;

        CollectionAssert.DoesNotContain(oldManager.Subordinates, employee);
        CollectionAssert.Contains(newManager.Subordinates, employee);
    }

    [Test]
    public void ToString_ShouldReturn_ManagerName()
    {
        var name = "Test Manager";
        var hireDate = DateTime.Today;

        var manager = new Manager(name, DateOnly.FromDateTime(hireDate));

        Assert.That(manager.ToString(), Is.EqualTo($"Manager: {name}"));
    }
}