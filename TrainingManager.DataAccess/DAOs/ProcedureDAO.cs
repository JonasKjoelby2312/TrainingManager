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
    private readonly string GET_ALL_PROCEDURES_WITH_REVISIONS = "SELECT tp.procedure_name AS ProcedureName, r.revision AS RevisionNumber, r.revision_is_active AS IsActive, r.revision_history_text AS HistoryText FROM treat_procedures tp JOIN revisions r ON tp.fk_revision_id = r.revision_id WHERE r.revision_is_active = 1 ORDER BY tp.procedure_name;";

    public ProcedureDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Procedure entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Procedure>> GetAllPorceduresWithRevisionsAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var procedures = await connection.QueryAsync<Procedure>(GET_ALL_PROCEDURES_WITH_REVISIONS);
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
