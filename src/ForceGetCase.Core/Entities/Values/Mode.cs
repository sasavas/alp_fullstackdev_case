using System.ComponentModel.DataAnnotations;

namespace ForceGetCase.Core.Entities.Values;

public enum Mode
{
    [Display(Name = "LCL")]
    LCL = 1,
    [Display(Name = "FCL")]
    FCL = 2,
    [Display(Name = "AIR")]
    Air = 3,
}
