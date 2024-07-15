namespace ForceGetCase.Application.Models.Quote;

public record QuoteValidationResult(bool Valid, string Reason = "");
