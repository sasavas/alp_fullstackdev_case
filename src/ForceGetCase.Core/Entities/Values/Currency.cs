using System.ComponentModel.DataAnnotations;

namespace ForceGetCase.Core.Entities.Values;

public enum Currency
{
    [Display(Name = "USD")]
    USD = 1,
    [Display(Name = "CNY")]
    CNY = 2,
    [Display(Name = "TRY")]
    TRY = 3
}
