using System.Data;
using System.Threading.Tasks;

namespace AccountTracker.Data
{
    public interface IConnectionResolver<T> where T : IDbConnection
    {
        Task<T> Resolve(string connectionString);
    }
}
