using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;
using TrainingManager.Server.Controllers;
using TrainingManager.WebAPI.Controllers;

namespace trainingmanager.NUnit;

public class EmployeeOverviewCRUDTests
{
    private EmployeesController _controller;
    private EmployeeDAO _employeeDAO;

    [SetUp]
    public void Setup()
    {
        _employeeDAO = new EmployeeDAO("Server=tcp:hildur.ucn.dk,1433;Database=DMA-CSD-S232_10503097;User ID=DMA-CSD-S232_10503097;Password=Password1!;");
        _controller = new EmployeesController(_employeeDAO);
    }

    [Test]
    public async Task EmployeeDAOGetAllEmployeesAndStatusesTest()
    {
        //Assign
        IEnumerable<Employee> res;

        //Act
        res = await _employeeDAO.GetAllEmployeesAndStatusesAsync();

        //Assert
        Assert.IsNotNull(res);
    }

    [Test]
    public async Task EmployeeOverviewControllerGetAsync()
    {
        //Assign
        Microsoft.AspNetCore.Mvc.ActionResult<IEnumerable<Employee>> res;

        //Act
        res = await _controller.GetAsync();

        //Assert
        var okResult = res.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.IsNotNull(okResult.Value);
    }

    [Test]
    public async Task EmployeeDAOUpdateAsyncTest()
    {
        //Assign
        int empID = 1;
        List<string> roles = new List<string>() { "Project Manager", "Specification Team Manager", "Specification Engineer", "Test Manager", "Tester", "QARA Manager", "R&D Manager" };
        Employee employee = new Employee(empID,"lw","lw@treatsystems.com",false);
        employee.Roles = roles;

        //Act
        bool res = await _employeeDAO.UpdateAsync(empID, employee);

        //Assert
        Assert.IsTrue(res);

        //Cleanup
        if (res)
        {
            employee.IsActive = true;
            await _employeeDAO.UpdateAsync(empID, employee);
        }
    }
}