using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingManager.DataAccess.DAOs;

public abstract class BaseDAO
{

    private string _connectionstring;

    protected BaseDAO(string connectionstring) => _connectionstring = connectionstring;

    protected IDbConnection CreateConnection() => new SqlConnection(_connectionstring);
}
