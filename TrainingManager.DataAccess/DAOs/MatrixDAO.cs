using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingManager.DataAccess.Models;

namespace TrainingManager.DataAccess.DAOs;

public class MatrixDAO : BaseDAO, IMatrixDAO
{
    private readonly string GET_ALL_ACTIVE_PROCEDURES_ID_AND_NAME = "SELECT procedure_id, procedure_name FROM treat_procedures RIGHT JOIN revisions on fk_treat_procedure_id = procedure_id WHERE revision_is_active = 1;";
    private readonly string GET_ALL_ACTIVE_EMPLOYEES_ID_AND_NAME = "SELECT employee_id, employee_initials FROM employees WHERE employee_is_active = 1;";
    private readonly string GET_ALL_TRAINING_AND_STATUSES = "WITH selected_employee AS (\r\n    SELECT employee_id, employee_initials \r\n    FROM employees \r\n    WHERE employee_initials = @Initials\r\n), \r\nactive_revisions AS (\r\n    SELECT \r\n        r.fk_treat_procedure_id, \r\n        r.revision_id,\r\n        ROW_NUMBER() OVER (PARTITION BY r.fk_treat_procedure_id ORDER BY r.revision_id DESC) AS rn\r\n    FROM revisions r\r\n    WHERE r.revision_is_active = 1\r\n), \r\nactive_procedures AS (\r\n    SELECT tp.procedure_id, tp.procedure_name\r\n    FROM treat_procedures tp\r\n    JOIN active_revisions ar ON tp.procedure_id = ar.fk_treat_procedure_id\r\n    WHERE ar.rn = 1\r\n), \r\nemployee_roles_expanded AS (\r\n    SELECT er.fk_role_id \r\n    FROM selected_employee se \r\n    JOIN employee_roles er ON se.employee_id = er.fk_employee_id\r\n), \r\nmax_required_type AS (\r\n    SELECT\r\n        rtt.fk_treat_procedure_id,\r\n        rtt.fk_role_id,\r\n        MAX(rtt.required_type) AS max_required_type\r\n    FROM\r\n        required_training_types rtt\r\n    JOIN employee_roles er ON er.fk_role_id = rtt.fk_role_id\r\n    WHERE er.fk_employee_id = (SELECT employee_id FROM selected_employee)\r\n    GROUP BY\r\n        rtt.fk_treat_procedure_id, rtt.fk_role_id\r\n),\r\ntraining_status_base AS (\r\n    SELECT \r\n        ap.procedure_name, \r\n        MAX(CASE \r\n                WHEN t.ticket_id IS NOT NULL AND r.revision_is_active = 1 THEN 3 \r\n                WHEN mrt.max_required_type = 2 THEN 2\r\n                WHEN mrt.max_required_type = 1 THEN 1\r\n                ELSE 0\r\n            END) AS StatusRank\r\n    FROM \r\n        active_procedures ap\r\n    CROSS JOIN \r\n        selected_employee se \r\n    LEFT JOIN \r\n        employee_roles er ON er.fk_employee_id = se.employee_id \r\n    LEFT JOIN \r\n        max_required_type mrt \r\n        ON mrt.fk_treat_procedure_id = ap.procedure_id \r\n        AND mrt.fk_role_id = er.fk_role_id\r\n    LEFT JOIN \r\n        tickets t ON t.fk_employee_id = se.employee_id \r\n        AND t.fk_treat_procedure_id = ap.procedure_id\r\n    LEFT JOIN \r\n        revisions r ON r.revision_id = t.fk_revision_id\r\n        AND r.revision_is_active = 1\r\n    GROUP BY \r\n        ap.procedure_name\r\n)\r\nSELECT \r\n    procedure_name, \r\n    CASE StatusRank \r\n        WHEN 3 THEN 'Completed' \r\n        WHEN 2 THEN 'Missing' \r\n        WHEN 1 THEN 'If Performing' \r\n        ELSE 'Optional' \r\n    END AS TrainingStatus\r\nFROM \r\n    training_status_base\r\nORDER BY \r\n    procedure_name;";
   
    public MatrixDAO(string connectionString) : base(connectionString)
    {
    }


    public async Task<MatrixParent> GetAdminComplianceMatrixAsync()
    {
        using var connection = CreateConnection();
        connection.Open();

        try
        {
            MatrixParent res = new MatrixParent();

            var procedures = (await connection.QueryAsync(GET_ALL_ACTIVE_PROCEDURES_ID_AND_NAME)).ToList();
            var employees = (await connection.QueryAsync(GET_ALL_ACTIVE_EMPLOYEES_ID_AND_NAME)).ToList();

            string[,] matrix = new string[employees.Count + 1, procedures.Count + 1];

            matrix[0, 0] = "Document";
            for (int i = 0; i < procedures.Count; i++)
            {
                matrix[0, i + 1] = procedures[i].procedure_name;
            }

            for (int i = 0; i < employees.Count; i++)
            {
                var employee = employees[i];
                matrix[i + 1, 0] = employee.employee_initials;

                var statusResults = await connection.QueryAsync(GET_ALL_TRAINING_AND_STATUSES, new { Initials = employee.employee_initials });
                var statusDict = statusResults.ToDictionary(
                    s => (string)s.procedure_name,
                    s => (string)s.TrainingStatus
                );

                for (int j = 0; j < procedures.Count; j++)
                {
                    var procName = (string)procedures[j].procedure_name;

                    matrix[i + 1, j + 1] = statusDict.TryGetValue(procName, out var status)
                        ? status
                        : "N/A";
                }
            }

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string[] row = new string[matrix.GetLength(1)];
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    row[j] = matrix[i, j];
                }
                res.Matrix.Add(row);
            }

            return res;
        }
        catch (Exception ex)
        {
            throw new Exception($"Could not get AdminComplianceMatrix. Reason: {ex.Message}", ex);
        }
    }

    public Task<MatrixParent> GetAdminComplienceMatrixAsync()
    {
        throw new NotImplementedException();
    }
}