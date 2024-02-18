using TBC.Task.Core.PersonEntity;

namespace TBC.Task.Core.Interfaces
{
    public interface ICityRepository
    {
        Task<City?> GetCity(int id);
    }
}
