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

    [Test]
    public void GetNetSalaryOnDate_NoSubordinatesAndWholeNumberOfYears_ShouldReturnSalaryWithYearlyPremium()
    {
        var hireDate = new DateOnly(2022, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 105;
        var manager = new Manager("Test Manager", hireDate, salary);

        var netSalary = manager.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_NoSubordinatesAndFractionalYears_ShouldReturnSalaryWithYearlyPremiumForWholeYearsOnly()
    {
        var hireDate = new DateOnly(2020, 10, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 110;
        var manager = new Manager("Test Manager", hireDate, salary);

        var netSalary = manager.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_NoSubordinatesManyYearsOfEmployment_PremiumShouldNotExceedMaximumPercentage()
    {
        var hireDate = new DateOnly(2010, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 140;
        var manager = new Manager("Test Manager", hireDate, salary);

        var netSalary = manager.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_1stLevelSubordinates_ShouldAddPremiumToSalary()
    {
        var hireDate = new DateOnly(2023, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 101;
        var manager = new Manager("Test Manager", hireDate, salary);

        manager.AddSubordinate(new Employee("Test Employee 1", hireDate, salary));
        manager.AddSubordinate(new Employee("Test Employee 2", hireDate, salary));

        var netSalary = manager.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_TransitionalSubordinates_ShouldNotAddPremiumToSalary()
    {
        var hireDate = new DateOnly(2023, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 100.505m;
        var manager = new Manager("Test Manager", hireDate, salary);

        var subManager = new Manager("Subordinate Manager", hireDate, salary, manager);
        
        subManager.AddSubordinate(new Employee("Test Employee 1", hireDate, salary));
        subManager.AddSubordinate(new Employee("Test Employee 2", hireDate, salary));

        var netSalary = manager.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_SubordinatesHiredLater_ShouldBeSkipped()
    {
        var managerHireDate = new DateOnly(2023, 3, 1);
        var hireDate = new DateOnly(2023, 4, 1);
        var payrollDate = new DateOnly(2023, 3, 1);
        var salary = 100;
        var expectedSalary = 100;
        var manager = new Manager("Test Manager", managerHireDate, salary);
        manager.AddSubordinate(new Employee("Test Employee", hireDate, salary));

        var netSalary = manager.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }
}