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
    private readonly string GET_ALL_EMPLOYEES = "SELECT \r\n    e.employee_id AS EmployeeId,\r\n    e.employee_initials AS Initials,\r\n    e.employee_email AS Email,\r\n    e.employee_is_active AS IsActive,\r\n    STRING_AGG(tr.role_name, ', ') AS Roles  -- Aggregates roles per employee\r\nFROM employees e\r\nLEFT JOIN employee_roles er ON er.fk_employee_id = e.employee_id\r\nLEFT JOIN treat_roles tr ON tr.role_id = er.fk_role_id\r\nGROUP BY \r\n    e.employee_id, e.employee_initials, e.employee_email, e.employee_is_active\r\nORDER BY e.employee_id;";
    private readonly string GET_TRAINING_STATUS = "WITH selected_employee AS (\r\n    SELECT employee_id, employee_initials\r\n    FROM employees\r\n    WHERE employee_initials = @Initials\r\n),\r\nactive_procedures AS (\r\n    SELECT tp.procedure_id, tp.procedure_name\r\n    FROM treat_procedures tp\r\n    JOIN revisions r ON tp.fk_revision_id = r.revision_id\r\n    WHERE r.revision_is_active = 1\r\n),\r\nemployee_roles_expanded AS (\r\n    SELECT er.fk_role_id\r\n    FROM selected_employee se\r\n    JOIN employee_roles er ON se.employee_id = er.fk_employee_id\r\n),\r\ntraining_status_base AS (\r\n    SELECT \r\n        ap.procedure_name,\r\n        MAX(\r\n            CASE \r\n                WHEN t.ticket_id IS NOT NULL THEN 3\r\n                WHEN rtt.required_type = 1 THEN 2\r\n                WHEN rtt.required_type = 2 THEN 1\r\n                ELSE 0\r\n            END\r\n        ) AS StatusRank\r\n    FROM active_procedures ap\r\n    CROSS JOIN selected_employee se\r\n    LEFT JOIN employee_roles er ON er.fk_employee_id = se.employee_id\r\n    LEFT JOIN required_training_types rtt \r\n        ON rtt.fk_role_id = er.fk_role_id \r\n        AND rtt.fk_treat_procedure_id = ap.procedure_id\r\n    LEFT JOIN tickets t \r\n        ON t.fk_employee_id = se.employee_id \r\n        AND t.fk_treat_procedure_id = ap.procedure_id\r\n    GROUP BY ap.procedure_name\r\n)\r\nSELECT \r\n    procedure_name,\r\n    CASE StatusRank\r\n        WHEN 3 THEN 'Training Completed'\r\n        WHEN 2 THEN 'Training Missing'\r\n        WHEN 1 THEN 'Training Optional'\r\n        ELSE 'Place holder'\r\n    END AS TrainingStatus\r\nFROM training_status_base\r\nORDER BY procedure_name;\r\n";

    public EmployeeDAO(string connectionString) : base(connectionString)
    {
    }

    public Task<int> CreateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Employee>> GetAllAsync()
    {
        using var connection = CreateConnection();
        connection.Open();
        try
        {
            var employees = await connection.QueryAsync<Employee>(GET_ALL_EMPLOYEES);

            foreach (Employee employee in employees)
            {
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

    public Task<Employee> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Employee entity)
    {
        throw new NotImplementedException();
    }
}
