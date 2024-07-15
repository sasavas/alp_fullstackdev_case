namespace ForceGetCase.Application.Models.Quote;

public record QuoteValidationRequest(int PackageType, int Count, int Mode);

public record PalletCountCalculationRequest(int PackageType, int Count);
