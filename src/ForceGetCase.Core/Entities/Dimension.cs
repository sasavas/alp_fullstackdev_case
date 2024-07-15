using ForceGetCase.Core.Common;

namespace ForceGetCase.Core.Entities;

public class Dimension : BaseEntity
{
    public string Type { get; private set; }
    public int Width { get; private set; }
    public int Length { get; private set; }
    public int Height { get; private set; }
    
    public static readonly Dimension Carton = new Dimension
        { Id = 1, Type = "Carton", Width = 12, Length = 12, Height = 12 };
    
    public static readonly Dimension Box = new Dimension
        { Id = 2, Type = "Box", Width = 24, Length = 16, Height = 12 };
    
    public static readonly Dimension Pallet = new Dimension
        { Id = 3, Type = "Pallet", Width = 40, Length = 48, Height = 60 };
    
    public static IReadOnlyList<Dimension> ValidDimensions =
    [
        Carton,
        Box,
        Pallet,
    ];
}
