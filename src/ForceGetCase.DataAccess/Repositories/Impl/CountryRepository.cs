using ForceGetCase.Core.Entities;
using ForceGetCase.DataAccess.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ForceGetCase.DataAccess.Repositories.Impl;

public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    public CountryRepository(DatabaseContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Country>> GetCountryListPairs()
    {
        return await Context.Countries.Include(p => p.Cities).ToListAsync();
    }
}
