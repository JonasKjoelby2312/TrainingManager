using Dapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class ProcedureDAO : BaseDAO, IProcedureDAO
{
    private readonly string GET_ALL_PROCEDURE_WITH_ACTIVE_REVISION = "SELECT tp.procedure_name AS ProcedureName, r.revision AS RevisionNumber, r.revision_is_active AS IsActive, r.revision_history_text AS HistoryText FROM treat_procedures tp JOIN revisions r ON r.fk_treat_procedure_id = tp.procedure_id WHERE r.revision_is_active = 1 ORDER BY tp.procedure_name;";
    private readonly string GET_ALL_REVISIONS_FOR_PROCEDURE = "SELECT tp.procedure_name AS ProcedureName, r.revision AS RevisionNumber, r.revision_is_active AS IsActive, r.revision_history_text AS HistoryText FROM revisions r JOIN treat_procedures tp ON tp.procedure_id = r.fk_treat_procedure_id WHERE tp.procedure_name = @ProcedureName ORDER BY r.revision DESC;";
    private readonly string INSERT_INTO_PROCEDURES = "INSERT INTO treat_procedures (procedure_name) OUTPUT INSERTED.procedure_id VALUES (@ProcedureName)";
    private readonly string INSERT_INTO_REVISIONS = "INSERT INTO revisions (revision, revision_is_active, revision_history_text, fk_treat_procedure_id) VALUES(@RevisionNumber, @IsActive, @HistoryText, @ProcedureId)";
    private readonly string INSERT_INTO_REQUIRED_TRAINING_TYPES = "INSERT INTO required_training_types (required_type, fk_role_id, fk_treat_procedure_id) VALUES (@RequiredType, @RoleId, @ProcedureId)";

    public ProcedureDAO(string connectionString) : base(connectionString)
    {
    }

    public async Task<int> CreateAsync(Procedure entity)
    {
        using var connection = CreateConnection();
        connection.Open();
        IDbTransaction dbTransaction = connection.BeginTransaction();

        try
        {
            
            var procedureId = await connection.ExecuteScalarAsync<int>(
                INSERT_INTO_PROCEDURES,
                new { entity.ProcedureName},
                dbTransaction
            );

            
            await connection.ExecuteAsync(
                INSERT_INTO_REVISIONS,
                new
                {
                    RevisionNumber = entity.RevisionNumber,
                    IsActive = entity.IsActive,
                    HistoryText = entity.HistoryText,
                    ProcedureId = procedureId

                },
                dbTransaction
            );


            if (entity.RolesRequiredTrainingList != null && entity.RolesRequiredTrainingList.Any())
            {
                foreach (var role in entity.RolesRequiredTrainingList)
                {
                    await connection.ExecuteAsync(INSERT_INTO_REQUIRED_TRAINING_TYPES,
                        new
                        {
                            RequiredType = role.TrainingRequiredTypes["default"],
                            RoleId = role.RoleId,
                            ProcedureId = procedureId
                        }, dbTransaction);
                }

            }


            dbTransaction.Commit();
            return procedureId;
        }
        catch (Exception ex)
        {
            dbTransaction.Rollback();
            throw new Exception("Failed to insert procedure and related data: {ex.Message}", ex);
        }
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
