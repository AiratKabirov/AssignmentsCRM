using System.Collections.Generic;
using System.Threading.Tasks;

namespace SampleCRM.Services
{
    public interface IDataService<T>
    {
        Task<IEnumerable<T>> ListEntities();

        Task<T> GetEntity(string outerId, string innerId);

        Task<T> CreateEntity(T assignment);

        Task<T> UpdateEntity(string outerId, string innerId, T assignment);

        Task DeleteEntity(string outerId, string innerId);
    }
}
