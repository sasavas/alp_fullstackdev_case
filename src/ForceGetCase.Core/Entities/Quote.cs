using ForceGetCase.Core.Common;

namespace ForceGetCase.Core.Entities;

public class Quote : BaseEntity
{
    public int Mode { get; set; }
    public int MovementType { get; set; }
    public int Incoterms { get; set; }
    public int CountryCity { get; set; }
    public int PackageType { get; set; }
    public int Unit1 { get; set; }
    public int Unit2 { get; set; }
    public int Currency { get; set; }
}
