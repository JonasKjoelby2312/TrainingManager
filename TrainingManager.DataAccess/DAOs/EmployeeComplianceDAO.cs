using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class EmployeeComplianceDAO : BaseDAO, IEmployeeComplianceDAO
{
    private readonly string STRING = "";

    public EmployeeComplianceDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<EmployeeCompliance> GetEmployeeCompliance(string inits)
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            EmployeeCompliance res = new EmployeeCompliance();

            var queryResult = connection.QueryAsync(STRING, new { Initials = inits});


        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get EmployeeCompliance, message was: {ex.Message}", ex);
        }
    }
}
