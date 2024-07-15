using ForceGetCase.Core.Common;

namespace ForceGetCase.Core.Entities;

public class Country : BaseEntity
{
    public string Name { get; set; }
    public ICollection<City> Cities { get; set; }
}
