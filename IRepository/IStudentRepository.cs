using CrudAPI.Models;
using Microsoft.OpenApi.Models;
using System.Data.SqlClient;
using System.Data;
using Dapper;
using System.Security.Cryptography;
using CrudAPI.Common;
using OperationType = CrudAPI.Common.OperationType;

namespace CrudAPI.IRepository
{
    public interface IStudentRepository
    {
        Task<List<Student>> Gets1();
        Task<List<Student>> Gets2();
    }
}
