using System;
using System.Collections.Generic;
using AutoMapper;
using ForceGetCase.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using ForceGetCase.Shared.Services;
using NSubstitute;

namespace ForceGetCase.Application.UnitTests.Services;

public class BaseServiceTestConfiguration
{
    protected readonly IClaimService ClaimService;
    protected readonly IConfiguration Configuration;
    protected readonly IMapper Mapper;
    protected readonly ICountryRepository CountryRepository;
    protected readonly IQuoteRepository QuoteRepository;
    
    protected BaseServiceTestConfiguration()
    {
        Mapper = new MapperConfiguration(cfg => { }).CreateMapper();
        
        var configurationBody = new Dictionary<string, string>
        {
            { "JwtConfiguration:SecretKey", "Super secret token key" }
        };
        
        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationBody)
            .Build();
        
        CountryRepository = Substitute.For<ICountryRepository>();
        QuoteRepository = Substitute.For<IQuoteRepository>();
        
        ClaimService = Substitute.For<IClaimService>();
        ClaimService.GetUserId().Returns(new Guid().ToString());
    }
}
