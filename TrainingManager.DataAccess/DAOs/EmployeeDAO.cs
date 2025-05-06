using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class EmployeeDAO : BaseDAO, IEmployeeDAO
{
    private readonly string GET_ALL_EMPLOYEES = "SELECT e.employee_id AS EmployeeId, e.employee_initials AS Initials, e.employee_email AS Email, e.employee_is_active AS IsActive, STRING_AGG(tr.role_name, ', ') AS RolesAsString FROM employees e LEFT JOIN employee_roles er ON er.fk_employee_id = e.employee_id LEFT JOIN treat_roles tr ON tr.role_id = er.fk_role_id GROUP BY e.employee_id, e.employee_initials, e.employee_email, e.employee_is_active ORDER BY e.employee_id;";
    private readonly string GET_TRAINING_STATUS = "WITH selected_employee AS (\r\n    SELECT employee_id, employee_initials \r\n    FROM employees \r\n    WHERE employee_initials = @Initials\r\n), \r\nactive_revisions AS (\r\n    SELECT \r\n        r.fk_treat_procedure_id, \r\n        r.revision_id,\r\n        ROW_NUMBER() OVER (PARTITION BY r.fk_treat_procedure_id ORDER BY r.revision_id DESC) AS rn\r\n    FROM revisions r\r\n    WHERE r.revision_is_active = 1\r\n), \r\nactive_procedures AS (\r\n    SELECT tp.procedure_id, tp.procedure_name\r\n    FROM treat_procedures tp\r\n    JOIN active_revisions ar ON tp.procedure_id = ar.fk_treat_procedure_id\r\n    WHERE ar.rn = 1\r\n), \r\nemployee_roles_expanded AS (\r\n    SELECT er.fk_role_id \r\n    FROM selected_employee se \r\n    JOIN employee_roles er ON se.employee_id = er.fk_employee_id\r\n), \r\nmax_required_type AS (\r\n    SELECT\r\n        rtt.fk_treat_procedure_id,\r\n        rtt.fk_role_id,\r\n        MAX(rtt.required_type) AS max_required_type\r\n    FROM\r\n        required_training_types rtt\r\n    JOIN employee_roles er ON er.fk_role_id = rtt.fk_role_id\r\n    WHERE er.fk_employee_id = (SELECT employee_id FROM selected_employee)\r\n    GROUP BY\r\n        rtt.fk_treat_procedure_id, rtt.fk_role_id\r\n),\r\ntraining_status_base AS (\r\n    SELECT \r\n        ap.procedure_name, \r\n        MAX(CASE \r\n                WHEN t.ticket_id IS NOT NULL AND r.revision_is_active = 1 THEN 3 \r\n                WHEN mrt.max_required_type = 2 THEN 2\r\n                WHEN mrt.max_required_type = 1 THEN 1\r\n                ELSE 0\r\n            END) AS StatusRank\r\n    FROM \r\n        active_procedures ap\r\n    CROSS JOIN \r\n        selected_employee se \r\n    LEFT JOIN \r\n        employee_roles er ON er.fk_employee_id = se.employee_id \r\n    LEFT JOIN \r\n        max_required_type mrt \r\n        ON mrt.fk_treat_procedure_id = ap.procedure_id \r\n        AND mrt.fk_role_id = er.fk_role_id\r\n    LEFT JOIN \r\n        tickets t ON t.fk_employee_id = se.employee_id \r\n        AND t.fk_treat_procedure_id = ap.procedure_id\r\n    LEFT JOIN \r\n        revisions r ON r.revision_id = t.fk_revision_id\r\n        AND r.revision_is_active = 1\r\n    GROUP BY \r\n        ap.procedure_name\r\n)\r\nSELECT \r\n    procedure_name, \r\n    CASE StatusRank \r\n        WHEN 3 THEN 'Completed' \r\n        WHEN 2 THEN 'Missing' \r\n        WHEN 1 THEN 'If Performing' \r\n        ELSE 'Optional' \r\n    END AS TrainingStatus\r\nFROM \r\n    training_status_base\r\nORDER BY \r\n    procedure_name;";
    private readonly string GET_EMPLOYEE_COMPLIANCE_BY_INITIALS = "WITH selected_employee AS (\r\n    SELECT employee_id, employee_initials \r\n    FROM employees \r\n    WHERE employee_initials = @Initials\r\n), \r\nactive_revisions AS (\r\n    SELECT \r\n        r.fk_treat_procedure_id, \r\n        r.revision_id,\r\n        r.revision,\r\n        ROW_NUMBER() OVER (PARTITION BY r.fk_treat_procedure_id ORDER BY r.revision_id DESC) AS rn\r\n    FROM revisions r\r\n    WHERE r.revision_is_active = 1\r\n), \r\nactive_procedures AS (\r\n    SELECT tp.procedure_id, tp.procedure_name\r\n    FROM treat_procedures tp\r\n    JOIN active_revisions ar ON tp.procedure_id = ar.fk_treat_procedure_id\r\n    WHERE ar.rn = 1\r\n), \r\nemployee_roles_expanded AS (\r\n    SELECT er.fk_role_id \r\n    FROM selected_employee se \r\n    JOIN employee_roles er ON se.employee_id = er.fk_employee_id\r\n), \r\nmax_required_type AS (\r\n    SELECT\r\n        rtt.fk_treat_procedure_id,\r\n        rtt.fk_role_id,\r\n        MAX(rtt.required_type) AS max_required_type\r\n    FROM\r\n        required_training_types rtt\r\n    JOIN employee_roles er ON er.fk_role_id = rtt.fk_role_id\r\n    WHERE er.fk_employee_id = (SELECT employee_id FROM selected_employee)\r\n    GROUP BY\r\n        rtt.fk_treat_procedure_id, rtt.fk_role_id\r\n),\r\ntraining_status_base AS (\r\n    SELECT \r\n        ap.procedure_name, \r\n        MAX(CASE \r\n                WHEN t.ticket_id IS NOT NULL AND r.revision_is_active = 1 THEN 3 \r\n                WHEN mrt.max_required_type = 2 THEN 2\r\n                WHEN mrt.max_required_type = 1 THEN 1\r\n                ELSE 0\r\n            END) AS StatusRank,\r\n        r.revision AS completed_revision,\r\n        ar.revision AS required_revision\r\n    FROM \r\n        active_procedures ap\r\n    CROSS JOIN \r\n        selected_employee se \r\n    LEFT JOIN \r\n        employee_roles er ON er.fk_employee_id = se.employee_id \r\n    LEFT JOIN \r\n        max_required_type mrt \r\n        ON mrt.fk_treat_procedure_id = ap.procedure_id \r\n        AND mrt.fk_role_id = er.fk_role_id\r\n    LEFT JOIN \r\n        tickets t ON t.fk_employee_id = se.employee_id \r\n        AND t.fk_treat_procedure_id = ap.procedure_id\r\n    LEFT JOIN \r\n        revisions r ON r.revision_id = t.fk_revision_id\r\n        AND r.revision_is_active = 1\r\n    LEFT JOIN\r\n        active_revisions ar ON ar.fk_treat_procedure_id = ap.procedure_id AND ar.rn = 1\r\n    GROUP BY \r\n        ap.procedure_name, r.revision, ar.revision\r\n)\r\nSELECT \r\n    procedure_name, \r\n    CASE StatusRank \r\n        WHEN 3 THEN 'Completed' \r\n        WHEN 2 THEN 'Missing' \r\n        WHEN 1 THEN 'If Performing' \r\n        ELSE 'Optional' \r\n    END AS TrainingStatus,\r\n    completed_revision,\r\n    required_revision\r\nFROM \r\n    training_status_base\r\nORDER BY \r\n    procedure_name;";
    private readonly string INSERT_EMPLOYEE = "INSERT INTO employees (employee_email, employee_initials, employee_is_active) Values (@Email, @Initials, @IsActive) SELECT CAST(SCOPE_IDENTITY() as INT);";
    
    private readonly string INSERT_EMPLOYEE_ROLES = "INSERT INTO employee_roles (fk_employee_id, fk_role_id) VALUES (@EmployeeId, @RoleId);";
    private readonly string GET_ROLE_ID_BY_ROLE_NAME = "SELECT role_id FROM treat_roles WHERE role_name = @RoleName;";
    
    private readonly string UPDATE_EMPLOYEE_BY_ID = "UPDATE employees SET employee_email = @EmployeeEmail, employee_initials = @EmployeeInitials, employee_is_active = @EmployeeIsActive WHERE employee_id = @EmployeeId";
    private readonly string GET_EMPLOYEES_ROLES_BY_EMPLOYEE_ID = "SELECT role_name FROM employees e JOIN employee_roles er on e.employee_id = er.fk_employee_id JOIN treat_roles tr on er.fk_role_id = tr.role_id WHERE employee_id = @EmployeeId;";
    private readonly string DELETE_ROLE_FROM_EMPLOYEE_ROLES_BY_EMPLOYEE_ID_AND_ROLE_ID = "DELETE FROM employee_roles WHERE fk_employee_id = @EmployeeId AND fk_role_id = @RoleId;";


    public EmployeeDAO(string connectionString) : base(connectionString)
    {
    }

    public async Task<int> CreateAsync(Employee entity) //TODO Husk at lave initials og email UNIQUE i DB
    {
        using var connection = CreateConnection();
        connection.Open();
        IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            int IdFromDB = await connection.ExecuteScalarAsync<int>(INSERT_EMPLOYEE, new { Email = entity.Email, Initials = entity.Initials, IsActive = entity.IsActive }, transaction);

            IEnumerable<int> rolesIds = await connection.QueryAsync<int>(GET_ROLE_ID_BY_ROLE_NAME, new { Roles = entity.Roles }, transaction);

            foreach (int roleId in rolesIds)
            {
                await connection.QueryAsync(INSERT_EMPLOYEE_ROLES, new { EmployeeId = IdFromDB, RoleId = roleId }, transaction);
            }

            transaction.Commit();

            return IdFromDB;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Could not create employee with initials: {entity.Initials}, message was: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAndStatusesAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            List<Employee> res = new List<Employee>();

            //EmployeeId - Initials - Email - IsActive - RolesAsString
            var allEmployees = await connection.QueryAsync(GET_ALL_EMPLOYEES);

            foreach (var dbEmployee in allEmployees)
            {
                List<string> roles = ((string)dbEmployee.RolesAsString).Split(',').Select(role => role.Trim()).ToList();
                var trainingStatuses = await connection.QueryAsync(GET_TRAINING_STATUS, new { Initials = dbEmployee.Initials });

                Employee currEmployee = new Employee(dbEmployee.EmployeeId, dbEmployee.Initials, dbEmployee.Email, dbEmployee.IsActive);
                currEmployee.Roles = roles;

                foreach (var status in trainingStatuses)
                {
                    currEmployee.EmployeeTrainingStatuses.Add(status.procedure_name, status.TrainingStatus);
                }

                res.Add(currEmployee);
            }

            connection.Close();
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all employees, message was: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Employee>> GetAllActiveAndInactiveEmployees()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            List<Employee> res = new List<Employee>();

            //EmployeeId - Initials - Email - IsActive - RolesAsString
            var allEmployees = await connection.QueryAsync(GET_ALL_EMPLOYEES);

            foreach (var dbEmployee in allEmployees)
            {
                List<string> roles = ((string)dbEmployee.RolesAsString).Split(',').Select(role => role.Trim()).ToList();

                Employee currEmployee = new Employee(dbEmployee.EmployeeId, dbEmployee.Initials, dbEmployee.Email, dbEmployee.IsActive);
                currEmployee.Roles = roles;

                res.Add(currEmployee);
            }

            connection.Close();
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all inactive employees, message was: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync() // depricated - also not currently used
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
            throw new Exception($"Could not get all employees, message was: {ex.Message}", ex);
        }
    }

    public Task<Employee> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<MatrixParent> GetEmployeeComplianceByInitialsAsync(string inits)
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            MatrixParent res = new MatrixParent();

            var queryResult = await connection.QueryAsync(GET_EMPLOYEE_COMPLIANCE_BY_INITIALS, new { Initials = inits });

            foreach (var item in queryResult)
            {
                string[] rowData = new string[4];
                rowData[0] = item.procedure_name;
                var reqRev = item.required_revision;
                rowData[1] = reqRev != null ? reqRev.ToString() : "N/A";
                rowData[2] = item.TrainingStatus;
                var comRev = item.completed_revision;
                rowData[3] = comRev != null ? comRev.ToString() : "N/A";

                res.Matrix.Add(rowData);
            }
            connection.Close();
            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get EmployeeCompliance, message was: {ex.Message}", ex);
        }
    }

    public async Task<bool> UpdateAsync(int id, Employee entity)
    {
        using var connection = CreateConnection();
        connection.Open();
        IDbTransaction transaction = connection.BeginTransaction();
        try
        {
            await connection.QueryAsync<int>(UPDATE_EMPLOYEE_BY_ID, new { EmployeeEmail = entity.Email, EmployeeInitials = entity.Initials, EmployeeIsActive = entity.IsActive, EmployeeId = id }, transaction);

            var employeesCurrRolesInDB = await connection.QueryAsync<string>(GET_EMPLOYEES_ROLES_BY_EMPLOYEE_ID, new { EmployeeId = entity.EmployeeId }, transaction);

            //Find the ones to remove
            List<string> updatedRoles = entity.Roles;
            List<string> rolesToRemove = new List<string>();
            foreach (var roleInDB in employeesCurrRolesInDB)
            {
                bool found = false;
                foreach (string roleNameFromUser in updatedRoles)
                {
                    if (roleInDB.Equals(roleNameFromUser))
                    {
                        found = true;
                        updatedRoles.Remove(roleNameFromUser); //We're only left with new roles
                        break;
                    }
                }

                if (!found)
                {
                    rolesToRemove.Add(roleInDB);
                }
            }

            //Add new roles
            foreach (string newRole in updatedRoles)
            {
                int roleID = await connection.QuerySingleAsync<int>(GET_ROLE_ID_BY_ROLE_NAME, new { RoleName = newRole }, transaction);
                await connection.QueryAsync(INSERT_EMPLOYEE_ROLES, new { EmployeeId = entity.EmployeeId, RoleId = roleID }, transaction);
            }

            //Delete old roles
            foreach(string oldRole in rolesToRemove)
            {
                int roleID = await connection.QuerySingleAsync<int>(GET_ROLE_ID_BY_ROLE_NAME, new { RoleName = oldRole }, transaction);
                await connection.QueryAsync(DELETE_ROLE_FROM_EMPLOYEE_ROLES_BY_EMPLOYEE_ID_AND_ROLE_ID, new { EmployeeId = id, RoleId = roleID }, transaction);
            }

            transaction.Commit();

            return true;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception($"Could not create employee with initials: {entity.Initials}, message was: {ex.Message}", ex);
        }
    }
}
