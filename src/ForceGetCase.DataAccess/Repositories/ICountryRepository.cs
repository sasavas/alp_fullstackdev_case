using ForceGetCase.Core.Entities;

namespace ForceGetCase.DataAccess.Repositories;

public interface ICountryRepository : IBaseRepository<Country>
{
    Task<IEnumerable<Country>> GetCountryListPairs();
}
