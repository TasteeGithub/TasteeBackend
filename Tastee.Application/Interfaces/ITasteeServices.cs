using System.Threading.Tasks;
using Tastee.Shared;

namespace Tastee.Application.Interfaces
{
    public interface ITasteeServices<T>
    {
        Task<T> GetByIdAsync(string id);

        Task<Response> InsertAsync(T model);

        Task<Response> UpdateAsync(T model);
    }
}