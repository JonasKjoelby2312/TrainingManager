using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class EmployeeDAO : BaseDAO, IEmployeeDAO
{
    private readonly string GET_ALL_EMPLOYEES = "SELECT \r\n    e.employee_id AS EmployeeId,\r\n    e.employee_initials AS Initials,\r\n    e.employee_email AS Email,\r\n    e.employee_is_active AS IsActive,\r\n    STRING_AGG(tr.role_name, ', ') AS RolesAsString  -- Aggregates roles per employee\r\nFROM employees e\r\nLEFT JOIN employee_roles er ON er.fk_employee_id = e.employee_id\r\nLEFT JOIN treat_roles tr ON tr.role_id = er.fk_role_id\r\nGROUP BY \r\n    e.employee_id, e.employee_initials, e.employee_email, e.employee_is_active\r\nORDER BY e.employee_id;";
    private readonly string GET_TRAINING_STATUS = "WITH selected_employee AS (\r\n    SELECT employee_id, employee_initials \r\n    FROM employees \r\n    WHERE employee_initials = @Initials\r\n), \r\nactive_revisions AS (\r\n    SELECT \r\n        r.fk_treat_procedure_id, \r\n        r.revision_id,\r\n        ROW_NUMBER() OVER (PARTITION BY r.fk_treat_procedure_id ORDER BY r.revision_id DESC) AS rn\r\n    FROM revisions r\r\n    WHERE r.revision_is_active = 1\r\n), \r\nactive_procedures AS (\r\n    SELECT tp.procedure_id, tp.procedure_name\r\n    FROM treat_procedures tp\r\n    JOIN active_revisions ar ON tp.procedure_id = ar.fk_treat_procedure_id\r\n    WHERE ar.rn = 1\r\n), \r\nemployee_roles_expanded AS (\r\n    SELECT er.fk_role_id \r\n    FROM selected_employee se \r\n    JOIN employee_roles er ON se.employee_id = er.fk_employee_id\r\n), \r\nmax_required_type AS (\r\n    SELECT\r\n        rtt.fk_treat_procedure_id,\r\n        rtt.fk_role_id,\r\n        MAX(rtt.required_type) AS max_required_type\r\n    FROM\r\n        required_training_types rtt\r\n    JOIN employee_roles er ON er.fk_role_id = rtt.fk_role_id\r\n    WHERE er.fk_employee_id = (SELECT employee_id FROM selected_employee)\r\n    GROUP BY\r\n        rtt.fk_treat_procedure_id, rtt.fk_role_id\r\n),\r\ntraining_status_base AS (\r\n    SELECT \r\n        ap.procedure_name, \r\n        MAX(CASE \r\n                WHEN t.ticket_id IS NOT NULL THEN 3 \r\n                WHEN mrt.max_required_type = 2 THEN 2\r\n                WHEN mrt.max_required_type = 1 THEN 1\r\n                ELSE 0\r\n            END) AS StatusRank\r\n    FROM \r\n        active_procedures ap\r\n    CROSS JOIN \r\n        selected_employee se \r\n    LEFT JOIN \r\n        employee_roles er ON er.fk_employee_id = se.employee_id \r\n    LEFT JOIN \r\n        max_required_type mrt \r\n        ON mrt.fk_treat_procedure_id = ap.procedure_id \r\n        AND mrt.fk_role_id = er.fk_role_id\r\n    LEFT JOIN \r\n        tickets t ON t.fk_employee_id = se.employee_id \r\n        AND t.fk_treat_procedure_id = ap.procedure_id\r\n    GROUP BY \r\n        ap.procedure_name\r\n)\r\nSELECT \r\n    procedure_name, \r\n    CASE StatusRank \r\n        WHEN 3 THEN 'Completed' \r\n        WHEN 2 THEN 'Missing' \r\n        WHEN 1 THEN 'If Performing' \r\n        ELSE 'Optional' \r\n    END AS TrainingStatus\r\nFROM \r\n    training_status_base\r\nORDER BY \r\n    procedure_name;";

    public EmployeeDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAndStatusesAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var employees = await connection.QueryAsync<Employee>(GET_ALL_EMPLOYEES);

            foreach (Employee employee in employees)
            {

                employee.Roles = employee.RolesAsString.Split(',').Select(role => role.Trim()).ToList();
                var trainingStatuses = await connection.QueryAsync(GET_TRAINING_STATUS, new { Initials = employee.Initials });
                foreach (var status in trainingStatuses)
                {
                    employee.EmployeeTrainingStatuses.Add(status.procedure_name, status.TrainingStatus);
                }
            }

            connection.Close();
            return employees;
        }
        catch (Exception ex)
        {
            throw new Exception($"Couldn't get all employees, message was: {ex.Message}", ex);
        }
    }

    public async Task<IEnumerable<Employee>> GetAllEmployeesAsync()
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

    public Task<int> UpdateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }
}
