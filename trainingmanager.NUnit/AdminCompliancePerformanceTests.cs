using System.Diagnostics;
using TrainingManager.DataAccess.DAOs;
using TrainingManager.DataAccess.Models;
using TrainingManager.WebAPI.Controllers;

namespace trainingmanager.NUnit;

public class AdminCompliancePerformanceTests
{
    private AdminComplianceMatrixController _controller;
    private MatrixDAO _matrixDAO;

    [SetUp]
    public void Setup()
    {
        _matrixDAO = new MatrixDAO("Server=tcp:hildur.ucn.dk,1433;Database=DMA-CSD-S232_10503097;User ID=DMA-CSD-S232_10503097;Password=Password1!;");
        _controller = new AdminComplianceMatrixController(_matrixDAO);
    }

    [Test]
    [Category("Performance")]
    public async Task AdminCompliancePerformanceTest()
    {
        //Arrange
        var stopwatch = Stopwatch.StartNew();

        //Act
        MatrixParent res = await _controller.Get();

        //Assert
        stopwatch.Stop();
        Assert.NotNull(res);
        TestContext.WriteLine($"Get() on AdminComplianceMatrixController completed in {stopwatch.ElapsedMilliseconds} ms.");
    }

    [Test]
    [Category("Performance")]
    public async Task MatrixDAOPerformanceTest()
    {
        //Arrange
        var stopwatch = Stopwatch.StartNew();

        //Act
        MatrixParent res = await _matrixDAO.GetAdminComplienceMatrixAsync();

        //Assert
        stopwatch.Stop();
        Assert.IsNotNull(res);
        TestContext.WriteLine($"GetAdminComplianceMatrixAsync() on MatrixDAO completed in {stopwatch.ElapsedMilliseconds} ms.");
    }
}