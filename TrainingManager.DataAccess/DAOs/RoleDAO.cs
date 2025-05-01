using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class RoleDAO : BaseDAO, IRoleDAO
{
    private readonly string GET_ALL_ROLES = "SELECT role_name FROM treat_roles;";

    public RoleDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Role entity)
    {
        throw new NotImplementedException();
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
