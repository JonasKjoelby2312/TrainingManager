using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class ProcedureDAO : BaseDAO, IProcedureDAO
{
    private readonly string GET_ALL_PROCEDURE_WITH_ACTIVE_REVISION = "SELECT tp.procedure_name AS ProcedureName, r.revision AS RevisionNumber, r.revision_is_active AS IsActive, r.revision_history_text AS HistoryText FROM treat_procedures tp JOIN revisions r ON r.fk_treat_procedure_id = tp.procedure_id WHERE r.revision_is_active = 1 ORDER BY tp.procedure_name;";
    private readonly string GET_ALL_REVISIONS_FOR_PROCEDURE = "SELECT\r\n  tp.procedure_name AS ProcedureName,\r\n  r.revision AS RevisionNumber,\r\n  r.revision_is_active AS IsActive,\r\n  r.revision_history_text AS HistoryText\r\nFROM revisions r\r\nJOIN treat_procedures tp ON tp.procedure_id = r.fk_treat_procedure_id\r\nWHERE tp.procedure_name = @ProcedureName\r\nORDER BY r.revision DESC;";
    public ProcedureDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Procedure entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Procedure>> GetAllProceduresWithRevisionsAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var procedures = await connection.QueryAsync<Procedure>(GET_ALL_PROCEDURE_WITH_ACTIVE_REVISION);
            connection.Close();
            return procedures;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all procedures, message was: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Procedure>> GetRevisionsForProcedureAsync(string procedureName)
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var revisions = await connection.QueryAsync<Procedure>(GET_ALL_REVISIONS_FOR_PROCEDURE, new { ProcedureName = procedureName });
            connection.Close();
            return revisions;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all procedures and revisions, message was: {ex.Message}", ex);
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
