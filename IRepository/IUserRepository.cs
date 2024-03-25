using CrudAPI.Models;

namespace CrudAPI.Repository
{
    public interface IUserRepository
    {
        Task<User> Save(User obj);
        Task<User> GetById(int objid);
        Task<List<User>> GetByName();
        Task<User> GetByEmail(User user);
        Task<String> Delete(User obj);
    }
}
