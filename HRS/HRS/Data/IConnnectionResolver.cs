using System.Data;
using System.Threading.Tasks;

namespace HRS.Data
{
    public interface IConnectionResolver<T> where T : IDbConnection
    {
        Task<T> Resolve(string connectionString);
    }
}
