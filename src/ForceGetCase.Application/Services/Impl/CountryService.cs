using ForceGetCase.Application.Models.Country;
using ForceGetCase.DataAccess.Repositories;

namespace ForceGetCase.Application.Services.Impl;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;
    
    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }
    
    public async Task<IEnumerable<Location>> GetList()
    {
        return (await _countryRepository.GetCountryListPairs())
            .SelectMany(c => c.Cities)
            .Select(c => new Location(c.Id, c.Country.Name + " - " + c.Name));
    }
}
