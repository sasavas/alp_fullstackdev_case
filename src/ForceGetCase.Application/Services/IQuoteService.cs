using ForceGetCase.Application.Models.Quote;

namespace ForceGetCase.Application.Services;

public interface IQuoteService
{
    Task<QuoteDto> AddQuote(QuoteRequest request);
    Task<IEnumerable<QuoteDto>> GetQuotes();
    Task<QuoteConfig> GetQuoteConfig();
}
