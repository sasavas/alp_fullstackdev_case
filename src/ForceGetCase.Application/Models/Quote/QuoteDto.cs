namespace ForceGetCase.Application.Models.Quote;

public record class QuoteDto(
    string Mode,
    string MovementType,
    string Incoterms,
    string Length,
    string Weight,
    string Currency,
    string City,
    string PackageType
);
