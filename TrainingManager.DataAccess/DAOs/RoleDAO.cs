using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class RoleDAO : BaseDAO, IRoleDAO
{
    private readonly string GET_ALL_ROLES = "SELECT role_name FROM treat_roles;";
    private readonly string INSERT_INTO_ROLES = "INSERT INTO treat_roles (role_name) VALUES (@RoleName) SELECT CAST(SCOPE_IDENTITY() as INT);";
    private readonly string GET_PROCEDURE_ID_BY_PROCEDURE_NAME = "SELECT procedure_id FROM treat_procedures where procedure_name = @ProcedureName;";
    private readonly string INSERT_INTO_REQUIRED_TRAINING_TYPE = "INSERT INTO required_training_types (required_type, fk_role_id, fk_treat_procedure_id) VALUES (@RequiredType, @RoleId, @ProcedureId);";

    public RoleDAO(string connectionString) : base(connectionString)
    {
    }

    public async Task<int> CreateAsync(Role entity)
    {
        using var connection = CreateConnection();
        connection.Open();
        IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            int newRoleId = await connection.ExecuteScalarAsync<int>(INSERT_INTO_ROLES, new { RoleName = entity.Name, IsActive = true }, transaction);

            foreach (string procedureName in entity.ProceduresRequiredTypes.Keys)
            {
                int procedureId = await connection.QuerySingleAsync<int>(GET_PROCEDURE_ID_BY_PROCEDURE_NAME, new { ProcedureName = procedureName }, transaction);
                int test = entity.ProceduresRequiredTypes[procedureName];
                await connection.ExecuteAsync(INSERT_INTO_REQUIRED_TRAINING_TYPE, new { RequiredType = entity.ProceduresRequiredTypes[procedureName], RoleId = newRoleId, ProcedureId = procedureId }, transaction);
            }
            transaction.Commit();
            connection.Close();

            return newRoleId;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Could not create new role, message was: {ex.Message}. {ex}");
        }
    }

    public async Task<IEnumerable<string>> GetAllRolesAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            IEnumerable<string> roles = await connection.QueryAsync<string>(GET_ALL_ROLES);
            connection.Close();
            return roles;
        }
        catch (Exception ex)
        {
            connection.Close();
            throw new Exception($"Couldn't get all role names, message was: {ex.Message}", ex);
        }
    }

    public Task<Role> GetRoleByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Role entity)
    {
        throw new NotImplementedException();
    }
}
