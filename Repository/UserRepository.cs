using CrudAPI.Models;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Security.Cryptography;
using CrudAPI.Common;
using OperationType = CrudAPI.Common.OperationType;

namespace CrudAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        string _ConnectionString = "";
        User _oUser = new User();
        List<User> _oUsers = new List<User>();

        public UserRepository(IConfiguration configuration)
        {
            _ConnectionString = configuration.GetConnectionString("SchoolDB");

        }


        //public Task<string> Delete(User obj)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<string> Delete(User obj)
        {
            string message = "";

            try
            {

                using (IDbConnection con = new SqlConnection(_ConnectionString))
                {

                    if (con.State == ConnectionState.Closed) con.Open();

                    var Users = await con.QueryAsync<User>("SP_User",
                    this.SetParameters(obj, (int)OperationType.Delete),
                                commandType: CommandType.StoredProcedure);
                message= "Deleted";


                }

            }
            catch (Exception ex) { message = ex.Message; }

            return message;
            }


        public async Task<User> GetByEmail(User user)
        {
            _oUser = new User();


            using (IDbConnection con = new SqlConnection(_ConnectionString))
            {

                if (con.State == ConnectionState.Closed) con.Open();

                string sql = string.Format(@"Select *   FROM [User] where UserName='{0}' AND Password='{1}'",user.UserName, user.Password);
                var Users = await con.QueryAsync<User>(sql);
                if (Users != null && Users.Count() > 0)
                {
                    _oUser = Users.SingleOrDefault();
                }
            }
            return _oUser;


        }


        //public async Task<User> GetByEmail(User user)
        //{
        //    _oUser = new User();

        //    using (IDbConnection con = new SqlConnection(_ConnectionString))
        //    {
        //        if (con.State == ConnectionState.Closed) con.Open();

        //        string sql = "] WHERE UserName=@UserName AND Password=@Password";
        //        var Users = await con.QueryAsync<User>(sql, new { UserName = user.Email, Password = user.Password });

        //        if (Users != null && Users.Any())
        //        {
        //            _oUser = Users.SingleOrDefault();
        //        }
        //    }

        //    return _oUser;
        //}


        public async Task<User> GetById(int objid)
        {
            _oUser = new User();

            

                using (IDbConnection con = new SqlConnection(_ConnectionString))
                {

                    if (con.State == ConnectionState.Closed) con.Open();

                    var Users = await con.QueryAsync<User>(string.Format(@"Select * from user where userid={0}", objid));
                    if(Users!=null && Users.Count()>0) {
                        _oUser = Users.SingleOrDefault();
                    }


                }

            
            
            return _oUser;


        }

        public async Task<List<User>> GetByName()
        {
            _oUsers = new List<User>();
            using (IDbConnection con = new SqlConnection(_ConnectionString))
            {
                if (con.State == ConnectionState.Closed) con.Open();

                var Users = await con.QueryAsync<User>(@"SELECT * FROM User");
                if (Users != null && Users.Count() > 0)
                {
                    _oUsers = Users.ToList();
                }
            }
            return _oUsers;
        }

        public  async Task<User> Save(User obj)
        {
            _oUser = new User();

            try
            {
                int operationType = Convert.ToInt32(obj.UserId != 0 ? OperationType.Insert:OperationType.Update);
                using (IDbConnection con = new SqlConnection(_ConnectionString))
                {

                    if (con.State == ConnectionState.Closed) con.Open();

                    var Users1 = await con.QueryAsync<User>("SP_User",
                    this.SetParameters(obj, operationType),
                                commandType: CommandType.StoredProcedure);

                    if (Users1 != null && Users1.Count() > 0)
                    {
                        _oUser = Users1.FirstOrDefault();
                    }

                }

            }
            catch (Exception ex) {
                _oUser = new User();
                _oUser.Message = ex.Message; }

            return _oUser;
        }

        private DynamicParameters SetParameters(User _oUsers, int noperationtype)
        {
            DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", _oUsers.UserId);
                parameters.Add("@Username", _oUsers.UserName); 
                parameters.Add("@Email", _oUsers.Email); 
                parameters.Add("@Password", _oUsers.Password);
                parameters.Add("@operationType", noperationtype);
                return parameters;
        }
    }
}
