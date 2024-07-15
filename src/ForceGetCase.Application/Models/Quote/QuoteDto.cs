namespace ForceGetCase.Application.Models.Quote;

public record class QuoteDto(
    string Mode,
    string MovementType,
    string Incoterms,
    string LengthUnit,
    string WeightUnit,
    string Currency,
    string City,
    string PackageType
);
