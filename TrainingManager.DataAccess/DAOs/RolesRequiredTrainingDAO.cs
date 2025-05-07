using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class RolesRequiredTrainingDAO : BaseDAO, IRolesRequiredTrainingDAO  
{
    private readonly string GET_ALL_ROLES_ID_AND_NAME = "SELECT \r\n    role_id,\r\n    role_name\r\nFROM \r\n    treat_roles\r\nORDER BY \r\n    role_name;";
    private readonly string GET_ROLES_REQUIRED_TRAINING_TYPE = "SELECT tp.procedure_name, rtt.required_type FROM  required_training_types rtt JOIN treat_roles tr ON rtt.fk_role_id = tr.role_id JOIN treat_procedures tp ON rtt.fk_treat_procedure_id = tp.procedure_id WHERE tr.role_name = @RoleName ORDER BY tp.procedure_id, rtt.required_type;";

    private readonly string UPDATE_REQUIRED_TYPE_SQL = "UPDATE required_training_types SET required_type = @RequiredType WHERE fk_role_id = @RoleId AND fk_treat_procedure_id = ( SELECT procedure_id FROM treat_procedures WHERE procedure_name = @ProcedureName )";
    public RolesRequiredTrainingDAO(string connectionString) : base(connectionString)
    {
    }

    public async Task<IEnumerable<RolesRequiredTraining>> GetAllRolesRequiredTraining()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            List<RolesRequiredTraining> res = new List<RolesRequiredTraining>();

            var roleNamesAndIds = await connection.QueryAsync(GET_ALL_ROLES_ID_AND_NAME);

            foreach (var roleNameAndId in roleNamesAndIds)
            {
                string currRoleName = roleNameAndId.role_name;
                int currRoleId = roleNameAndId.role_id;

                var procedureNamesAndRequiredTrainingTypes = await connection.QueryAsync(GET_ROLES_REQUIRED_TRAINING_TYPE, new { RoleName = currRoleName});

                RolesRequiredTraining currentRolesRequiredTraining = new RolesRequiredTraining(currRoleId, currRoleName);

                foreach (var procedureAndRequiredType in procedureNamesAndRequiredTrainingTypes)
                {
                    string currProcedureName = procedureAndRequiredType.procedure_name;
                    int currRequiredType = procedureAndRequiredType.required_type;

                    currentRolesRequiredTraining.TrainingRequiredTypes.Add(currProcedureName, currRequiredType);
                }

                res.Add(currentRolesRequiredTraining);
            }

            connection.Close();
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get roles and their required training types, message was: {ex.Message}", ex);
        }
    }

    public async Task UpdateTrainingRequirementAsync(UpdateTrainingRequirementDto dto)
    {
        using var connection = CreateConnection();
        await connection.ExecuteAsync(UPDATE_REQUIRED_TYPE_SQL, dto);
    }


}
