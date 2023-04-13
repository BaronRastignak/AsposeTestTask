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

    [Test]
    public void GetNetSalaryOnDate_NoSubordinatesAndWholeNumberOfYears_ShouldReturnSalaryWithYearlyPremium()
    {
        var hireDate = new DateOnly(2022, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 101;
        var sales = new Sales("Test Sales", hireDate, salary);

        var netSalary = sales.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_NoSubordinatesAndFractionalYears_ShouldReturnSalaryWithYearlyPremiumForWholeYearsOnly()
    {
        var hireDate = new DateOnly(2020, 10, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 102;
        var sales = new Sales("Test Sales", hireDate, salary);

        var netSalary = sales.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_NoSubordinatesManyYearsOfEmployment_PremiumShouldNotExceedMaximumPercentage()
    {
        var hireDate = new DateOnly(1985, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 135;
        var sales = new Sales("Test Sales", hireDate, salary);

        var netSalary = sales.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_AllSubordinates_ShouldAddPremiumToSalary()
    {
        var hireDate = new DateOnly(2023, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 100.9009m;
        var sales = new Sales("Test Sales", hireDate, salary);

        sales.AddSubordinate(new Employee("Test Employee 1", hireDate, salary));

        var subordinate = new Sales("Subordinate Sales", hireDate, salary, sales);
        subordinate.AddSubordinate(new Employee("Test Employee 2", hireDate, salary));

        var netSalary = sales.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_SubordinatesHiredLater_ShouldBeSkipped()
    {
        var salesHireDate = new DateOnly(2023, 3, 1);
        var subordinateHireDate = new DateOnly(2023, 4, 1);
        var payrollDate = new DateOnly(2023, 3, 1);
        var salary = 100;
        var expectedSalary = 100.6m;
        var sales = new Sales("Test Sales", salesHireDate, salary);

        sales.AddSubordinate(new Employee("Test Employee 1", salesHireDate, salary));

        var subordinate = new Sales("Subordinate Sales", subordinateHireDate, salary, sales);
        subordinate.AddSubordinate(new Employee("Test Employee 2", salesHireDate, salary));

        var netSalary = sales.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }
}