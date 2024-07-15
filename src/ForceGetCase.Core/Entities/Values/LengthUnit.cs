using System.ComponentModel.DataAnnotations;

namespace ForceGetCase.Core.Entities.Values;

public enum LengthUnit
{
    [Display(Name = "Centimeter")]
    CM = 1,
    [Display(Name = "Inch")]
    IN = 2
}
