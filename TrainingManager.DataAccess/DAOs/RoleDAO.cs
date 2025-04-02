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
    private readonly string GET_ALL_ROLES_BY_ID = "";

    public RoleDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Role entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Role>> GetAllRolesForEmployeeAsync(int id)
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var roles = await connection.QueryAsync<Role>(GET_ALL_ROLES_BY_ID, new {Id = id});
            connection.Close();
            return roles;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all roles by id: {id}, message was: {ex.Message}", ex);
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
