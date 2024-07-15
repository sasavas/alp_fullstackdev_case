using System.ComponentModel.DataAnnotations;

namespace ForceGetCase.Core.Entities.Values;

public enum MovementType
{
    [Display(Name = "Door to Door")]
    DoorToDoor = 1,
    [Display(Name = "Port to Door")]
    PortToDoor = 2,
    [Display(Name = "Door to Port")]
    DoorToPort = 3,
    [Display(Name = "Port to Port")]
    PortToPort = 4
}
