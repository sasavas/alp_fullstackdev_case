namespace ForceGetCase.Application.Models.Quote;

public class QuoteRequest
{
    public int Mode { get; set; }
    public int MovementType { get; set; }
    public int Incoterms { get; set; }
    public int City { get; set; }
    public int PackageType { get; set; }
    public int Unit1 { get; set; }
    public double Length { get; set; }
    public int Unit2 { get; set; }
    public double Weight { get; set; }
    public int Currency { get; set; }
    public int Count { get; set; }
}
