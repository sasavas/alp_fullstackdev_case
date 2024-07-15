namespace ForceGetCase.Application.Models.Quote;

public record class QuoteConfig(
    Dictionary<int, string> Modes,
    Dictionary<int, string> MovementTypes,
    Dictionary<int, string> Incoterms,
    Dictionary<int, string> LengthUnits,
    Dictionary<int, string> WeightUnits,
    Dictionary<int, string> Currencies,
    Dictionary<int, string> Cities,
    Dictionary<int, string> PackageTypes
);
