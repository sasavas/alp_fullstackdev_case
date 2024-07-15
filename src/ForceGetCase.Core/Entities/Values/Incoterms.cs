using System.ComponentModel.DataAnnotations;

namespace ForceGetCase.Core.Entities.Values;

public enum Incoterms
{
    [Display(Name = "DDP")]
    DDP = 1,
    [Display(Name = "DAP")]
    DAT = 2
}
