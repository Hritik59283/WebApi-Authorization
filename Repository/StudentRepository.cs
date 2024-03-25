using CrudAPI.IRepository;
using CrudAPI.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace CrudAPI.Repository
{
    public class StudentRepository : IStudentRepository
    {
        string _ConnectionString = "";
        Student _oStudent = new Student();
        List<Student> _oStudents = new List<Student>();

        public StudentRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("SchoolDB");

        }


        public async Task<List<Student>> Gets1()
        {
            _oStudents = new List<Student>();
            using (IDbConnection con = new SqlConnection(_ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var Students = await con.QueryAsync<Student>(@"SELECT * FROM Student");
                if (Students != null && Students.Count() > 0)
                {
                    _oStudents = Students.ToList();
                }
            }
            return _oStudents;
        }

        public async Task<List<Student>> Gets2()
        {
            _oStudents = new List<Student>();
            using (IDbConnection con = new SqlConnection(_ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var Students = await con.QueryAsync<Student>(@"SELECT * FROM [Student]");
                if (Students != null && Students.Count() > 0)
                {
                    _oStudents = Students.ToList();
                }
            }
            return _oStudents;
        }
    }
}
