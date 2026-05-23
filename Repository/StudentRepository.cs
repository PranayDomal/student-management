using System.Data.SqlTypes;
using System.Data;
using Microsoft.Data.SqlClient;

namespace StudentManagementAPI.Repository
{
    public class StudentRepository
    {
        private readonly IConfiguration _config;

        public StudentRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection GetCon()
        {
            return new SqlConnection(_config.GetConnectionString("DbConnection"));
        }
    }
}
