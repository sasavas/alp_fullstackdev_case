using ForceGetCase.Application.Models.Quote;
using ForceGetCase.Core.Entities;

namespace ForceGetCase.Application.Services;

public interface ICalculationService
{
    Task<IEnumerable<Dimension>> GetAllDimensions();
    
    Task<QuoteValidationResult> ValidateQuote(QuoteValidationRequest request);
    
    Task<CalculationResult> CalculatePalletCount(PalletCountCalculationRequest request);
}
