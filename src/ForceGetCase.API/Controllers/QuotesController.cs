using ForceGetCase.Application.Models;
using ForceGetCase.Application.Models.Quote;
using ForceGetCase.Application.Services;
using ForceGetCase.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ForceGetCase.API.Controllers;

public class QuotesController : ApiController
{
    private readonly ICalculationService _calculationService;
    private readonly IQuoteService _quoteService;
    
    public QuotesController(ICalculationService calculationService, IQuoteService quoteService)
    {
        _calculationService = calculationService;
        _quoteService = quoteService;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<QuoteDto>>> GetQuoteList()
    {
        return Ok(ApiResult<IEnumerable<QuoteDto>>.Success(await _quoteService.GetQuotes()));
    }
    
    [HttpGet("dimensions")]
    public async Task<ActionResult<IEnumerable<Dimension>>> GetDimensions()
    {
        return Ok(ApiResult<IEnumerable<Dimension>>.Success(await _calculationService.GetAllDimensions()));
    }
    
    [HttpPost("validateQuote")]
    public async Task<ActionResult<QuoteValidationResult>> Calculate([FromBody] QuoteValidationRequest request)
    {
        var result = await _calculationService.ValidateQuote(request);
        return Ok(ApiResult<QuoteValidationResult>.Success(result));
    }
    
    [HttpPost("calculatePalletCount")]
    public async Task<ActionResult<CalculationResult>> CalculatePalletCount(PalletCountCalculationRequest request)
    {
        return Ok(ApiResult<CalculationResult>.Success(await _calculationService.CalculatePalletCount(request)));
    }
    
    [HttpPost]
    public async Task<ActionResult<QuoteDto>> CreateQuote([FromBody] QuoteRequest request)
    {
        return Ok(ApiResult<QuoteDto>.Success(await _quoteService.AddQuote(request)));
    }
    
    [HttpGet("config")]
    public async Task<ActionResult<QuoteConfig>> GetQuoteConfig()
    {
        return Ok(ApiResult<QuoteConfig>.Success(await _quoteService.GetQuoteConfig()));
    }
}
