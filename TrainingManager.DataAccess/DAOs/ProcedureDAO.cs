using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class ProcedureDAO : BaseDAO, IProcedureDAO
{
    private readonly string GET_ALL_PROCEDURES = "";

    public ProcedureDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Procedure entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Procedure>> GetAllAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var procedures = await connection.QueryAsync<Procedure>(GET_ALL_PROCEDURES);
            connection.Close();
            return procedures;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all procedures, message was: {ex.Message}", ex);
        }
    }

    public Task<Procedure> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Procedure entity)
    {
        throw new NotImplementedException();
    }
}
