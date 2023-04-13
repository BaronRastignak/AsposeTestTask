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
        superior.When(s => s.AddSubordinate(Arg.Any<Employee>()))
            .Do(ci => ci.Arg<Employee>().SetSuperiorDirect(superior));

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

    [Test]
    public void GetNetSalaryOnDate_DateBeforeHireDate_ShouldThrowException()
    {
        var hireDate = new DateOnly(2023, 4, 1);
        var payrollDate = new DateOnly(2023, 3, 31);
        var employee = new Employee("Test Employee", hireDate);

        Assert.That(() => employee.GetNetSalaryOnDate(payrollDate), Throws.ArgumentException);
    }

    [Test]
    public void GetNetSalaryOnDate_DateOnHireDate_ShouldReturnBaseSalary()
    {
        var hireDate = new DateOnly(2023, 4, 1);
        var salary = 1000;
        var employee = new Employee("Test Employee", hireDate, salary);

        var netSalary = employee.GetNetSalaryOnDate(hireDate);

        Assert.That(netSalary, Is.EqualTo(salary));
    }

    [Test]
    public void GetNetSalaryOnDate_DateLessThanYearAfterHireDate_ShouldReturnBaseSalary()
    {
        var hireDate = new DateOnly(2022, 10, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 1000;
        var employee = new Employee("Test Employee", hireDate, salary);

        var netSalary = employee.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(salary));
    }

    [Test]
    public void GetNetSalaryOnDate_WholeNumberOfYears_ShouldReturnSalaryWithYearlyPremium()
    {
        var hireDate = new DateOnly(2022, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 103;
        var employee = new Employee("Test Employee", hireDate, salary);

        var netSalary = employee.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_FractionalYears_ShouldReturnSalaryWithYearlyPremiumForWholeYearsOnly()
    {
        var hireDate = new DateOnly(2020, 10, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 106;
        var employee = new Employee("Test Employee", hireDate, salary);

        var netSalary = employee.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }

    [Test]
    public void GetNetSalaryOnDate_ManyYearsOfEmployment_PremiumShouldNotExceedMaximumPercentage()
    {
        var hireDate = new DateOnly(2010, 4, 1);
        var payrollDate = new DateOnly(2023, 4, 1);
        var salary = 100;
        var expectedSalary = 130;
        var employee = new Employee("Test Employee", hireDate, salary);

        var netSalary = employee.GetNetSalaryOnDate(payrollDate);

        Assert.That(netSalary, Is.EqualTo(expectedSalary));
    }
}