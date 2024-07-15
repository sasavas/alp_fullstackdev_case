using ForceGetCase.Application.Models.Country;

namespace ForceGetCase.Application.Services;

public interface ICountryService
{
    Task<IEnumerable<Location>> GetList();
}
