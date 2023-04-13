using EmployeeStructure.API.Models;
using EmployeeStructure.BusinessLayer;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeStructure.API.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")]
public class EmployeesController : ControllerBase
{
    private readonly Employee[] _employees;

    public EmployeesController()
    {
        _employees = new Employee[10];
        GenerateEmployees();
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IEnumerable<EmployeeDTO> Get()
    {
        return _employees
            .Select((em, index) => new EmployeeDTO(index + 1, em))
            .ToList();
    }

    [HttpGet("{id}/salary/{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SalaryDTO> GetEmployeeSalary(int id, DateOnly date)
    {
        if (id < 1)
            return BadRequest();

        if (id > _employees.Length)
            return NotFound();

        var employee = _employees[id - 1];
        var salary = employee.GetNetSalaryOnDate(date);
        var employeeDto = new EmployeeDTO(id, employee);
        return new SalaryDTO(date, Math.Round(salary, 2), employeeDto);
    }

    [HttpGet("salary-total/{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public SalaryDTO GetSalariesTotal(DateOnly date)
    {
        var salary = _employees.Sum(em => em.GetNetSalaryOnDate(date));
        return new SalaryDTO(date, Math.Round(salary, 2));
    }

    private void GenerateEmployees()
    {
        _employees[0] = new Employee("Gordon Phillips", new DateOnly(2001, 09, 12), 300);
        _employees[1] = new Manager("Clara Dyer", new DateOnly(2007, 10, 26), 500);
        _employees[2] = new Employee("Dawud Daniel", new DateOnly(2010, 08, 30), 250, (IHasSubordinates) _employees[1]);
        _employees[3] = new Employee("Sami Rangel", new DateOnly(2011, 08, 30), 350, (IHasSubordinates) _employees[1]);
        _employees[4] = new Manager("Jorja Mccann", new DateOnly(2012, 08, 24), 800);
        _employees[5] = new Sales("Iestyn Ruiz", new DateOnly(2013, 05, 23), 400, (IHasSubordinates) _employees[4]);
        _employees[6] = new Sales("Kelsey Christian", new DateOnly(2014, 09, 05), 150, (IHasSubordinates) _employees[5]);
        _employees[7] = new Sales("Marc Frederick", new DateOnly(2015, 05, 07), 500, (IHasSubordinates) _employees[4]);
        _employees[8] = new Sales("Faye Abbott", new DateOnly(2015, 09, 11), 450, (IHasSubordinates) _employees[7]);
        _employees[9] = new Sales("Caroline Francis", new DateOnly(2022, 11, 11), 100, (IHasSubordinates) _employees[7]);
    }
}
