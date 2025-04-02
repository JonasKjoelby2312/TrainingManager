using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class EmployeeDAO : BaseDAO, IEmployeeDAO
{
    private readonly string GET_ALL_EMPLOYEES = ""

    public EmployeeDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var employees = await connection.QueryAsync<Employee>(GET_ALL_EMPLOYEES);
            connection.Close();
            return employees;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all employees, message was: {ex.Message}", ex);
        }
    }

    public Task<Employee> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }
}
