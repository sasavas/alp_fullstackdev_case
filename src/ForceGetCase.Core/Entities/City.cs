using ForceGetCase.Core.Common;

namespace ForceGetCase.Core.Entities;

public class City : BaseEntity
{
    public string Name { get; set; }
    public Country Country { get; set; }
}
