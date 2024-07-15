using System.ComponentModel.DataAnnotations;

namespace ForceGetCase.Core.Entities.Values;

public enum WeightUnit
{
    [Display(Name = "Kilogram")]
    KG = 1,
    [Display(Name = "Pound")]
    LB = 2
}
