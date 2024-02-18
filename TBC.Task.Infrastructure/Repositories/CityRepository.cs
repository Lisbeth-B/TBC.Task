using Microsoft.EntityFrameworkCore;
using TBC.Task.Core.Interfaces;
using TBC.Task.Core.PersonEntity;
using TBC.Task.Infrastructure.Database;

namespace TBC.Task.Infrastructure.Repositories
{
    public class CityRepository(DBContext dbContext) : ICityRepository
    {
        private readonly DBContext dbContext = dbContext;

        public async Task<City?> GetCity(int id)
        {
            Database.Tables.City? cityModel = await dbContext.Cities.FirstOrDefaultAsync(x => x.Id == id);

            if (cityModel == null)
            {
                return null;
            }

            return new City { Id = cityModel.Id, Name = cityModel.Name };
        }
    }
}
