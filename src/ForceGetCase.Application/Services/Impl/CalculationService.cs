using ForceGetCase.Application.Exceptions;
using ForceGetCase.Application.Models.Quote;
using ForceGetCase.Core.Entities;
using ForceGetCase.Core.Entities.Values;
using ForceGetCase.DataAccess.Repositories;

namespace ForceGetCase.Application.Services.Impl;

public class CalculationService : ICalculationService
{
    private readonly IDimensionRepository _dimensionRepository;
    
    public CalculationService(IDimensionRepository dimensionRepository)
    {
        _dimensionRepository = dimensionRepository;
    }
    
    public async Task<IEnumerable<Dimension>> GetAllDimensions()
    {
        return await _dimensionRepository.GetAllAsync();
    }
    
    public async Task<QuoteValidationResult> ValidateQuote(QuoteValidationRequest request)
    {
        var result = await CalculatePalletCount(new PalletCountCalculationRequest(request.PackageType, request.Count));
        return (Mode)request.Mode switch
        {
            Mode.LCL when result.PalletCount >= 24 => new QuoteValidationResult(false, "For LCL, pallet count must be below 24."),
            Mode.FCL when result.PalletCount > 24 => new QuoteValidationResult(false, "For FCL, pallet count cannot exceed 24."),
            _ => new QuoteValidationResult(true)
        };
    }
    
    public async Task<CalculationResult> CalculatePalletCount(PalletCountCalculationRequest request)
    {
        var dimensions = await _dimensionRepository.GetAllAsync();
        var carton = dimensions.FirstOrDefault(d => d.Type == "Carton");
        var box = dimensions.FirstOrDefault(d => d.Type == "Box");
        var pallet = dimensions.FirstOrDefault(d => d.Type == "Pallet");
        
        if (carton == null || box == null || pallet == null)
        {
            throw new UnprocessableRequestException("Dimensions not found");
        }
        
        var dimension = dimensions.FirstOrDefault(d => d.Id == request.PackageType);
        if (dimension == null)
        {
            throw new UnprocessableRequestException("Dimension not found.");
        }
        
        if (request.PackageType == pallet.Id)
        {
            return new CalculationResult(request.Count);
        }
      
        var palletCount = 0;
        
        var cartonToBox = CalculateBoxCount(box, carton);
        var boxToPallets = CalculatePalletCount(pallet, box);
        
        if (request.PackageType == carton.Id)
        {
            var boxCount =  Math.Ceiling((double)request.Count / cartonToBox);
            palletCount = (int)Math.Ceiling(boxCount / boxToPallets) ;
        }
        else if (request.PackageType == box.Id)
        {
            palletCount = (int)Math.Ceiling((double)request.Count / boxToPallets);
        }
        
        return new CalculationResult(palletCount);
    }
    
    private static int CalculateBoxCount(Dimension box, Dimension carton)
    {
        var cartonToBox = (int)Math.Floor((double)box.Width / carton.Width) *
                    (int)Math.Floor((double)box.Length / carton.Length) *
                    (int)Math.Floor((double)box.Height / carton.Height);
        
        return cartonToBox;
    }
    
    private static int CalculatePalletCount(Dimension pallet, Dimension box)
    {
        var palletToBox = (int)Math.Floor((double)pallet.Width / box.Width) *
                      (int)Math.Floor((double)pallet.Length / box.Length) *
                      (int)Math.Floor((double)pallet.Height / box.Height);
        
        return palletToBox;
    }
}
