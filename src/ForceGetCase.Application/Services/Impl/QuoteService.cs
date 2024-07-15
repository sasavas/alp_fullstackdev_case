using ForceGetCase.Application.Exceptions;
using ForceGetCase.Application.Models.Quote;
using ForceGetCase.Core.Entities;
using ForceGetCase.Core.Entities.Values;
using ForceGetCase.Core.Extensions;
using ForceGetCase.DataAccess.Repositories;

namespace ForceGetCase.Application.Services.Impl;

public class QuoteService : IQuoteService
{
    private readonly ICalculationService _calculationService;
    private readonly ICountryService _countryService;
    private readonly IQuoteRepository _quoteRepository;
    
    public QuoteService(
        ICalculationService calculationService, 
        IQuoteRepository quoteRepository,
        ICountryService countryService)
    {
        _calculationService = calculationService;
        _quoteRepository = quoteRepository;
        _countryService = countryService;
    }
    
    public async Task<QuoteDto> AddQuote(QuoteRequest request)
    {
        var calculationResult =
            await _calculationService.ValidateQuote(
                new QuoteValidationRequest(request.PackageType, request.Count, request.Mode));
        
        if (!calculationResult.Valid)
        {
            throw new UnprocessableRequestException(calculationResult.Reason);
        }
        
        var quote = new Quote
        {
            PackageType = request.PackageType,
            Mode = request.Mode,
            CountryCity = request.City,
            Currency = request.Currency,
            Incoterms = request.Incoterms,
            MovementType = request.MovementType,
            Unit1 = request.Unit1,
            Unit2 = request.Unit2,
        };
        
        var added = await _quoteRepository.AddAsync(quote);
        
        var cnf = await GetQuoteConfig();
        return MapToQuoteDto(cnf, quote);
    }
    
    public async Task<IEnumerable<QuoteDto>> GetQuotes()
    {
        var cnf = await GetQuoteConfig();
        var quotes = await _quoteRepository.GetAllAsync();
        return quotes.Select(q => MapToQuoteDto(cnf, q));
    }
    
    private static QuoteDto MapToQuoteDto(QuoteConfig cnf, Quote quote)
    {
        return new QuoteDto(
            cnf.Modes.FirstOrDefault(x => x.Key == quote.Mode).Value,
            cnf.MovementTypes.FirstOrDefault(x => x.Key == quote.MovementType).Value,
            cnf.Incoterms.FirstOrDefault(x => x.Key == quote.Incoterms).Value,
            cnf.LengthUnits.FirstOrDefault(x => x.Key == quote.Unit1).Value,
            cnf.WeightUnits.FirstOrDefault(x => x.Key == quote.Unit2).Value,
            cnf.Currencies.FirstOrDefault(x => x.Key == quote.Currency).Value,
            cnf.Cities.FirstOrDefault(x => x.Key == quote.CountryCity).Value,
            cnf.PackageTypes.FirstOrDefault(x => x.Key == quote.PackageType).Value
        );
    }
    
    public async Task<QuoteConfig> GetQuoteConfig()
    {
        var cities = (await _countryService.GetList())
            .ToDictionary(c => c.CityId, c => c.CountryCityPair);
        
        var packageTypes = Dimension.ValidDimensions.ToDictionary(x => x.Id, x => x.Type); 
            
        var quoteConfig = new QuoteConfig
        (
            EnumExtensions.GetEnumDisplayNames<Mode>(),
            EnumExtensions.GetEnumDisplayNames<MovementType>(),
            EnumExtensions.GetEnumDisplayNames<Incoterms>(),
            EnumExtensions.GetEnumDisplayNames<LengthUnit>(),
            EnumExtensions.GetEnumDisplayNames<WeightUnit>(),
            EnumExtensions.GetEnumDisplayNames<Currency>(),
            cities,
            packageTypes
        );
        
        return quoteConfig;
    }
}
