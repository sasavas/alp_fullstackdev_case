using AutoMapper;
using ForceGetCase.Application.Exceptions;
using ForceGetCase.Application.Helpers;
using ForceGetCase.Application.Models;
using ForceGetCase.Application.Models.User;
using ForceGetCase.DataAccess.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ForceGetCase.Application.Services.Impl;

public class UserService : IUserService
{
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public UserService(IMapper mapper,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration
        )
    {
        _mapper = mapper;
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    
    public async Task<CreateUserResponseModel> CreateAsync(CreateUserModel createUserModel)
    {
        var user = _mapper.Map<ApplicationUser>(createUserModel);
        user.UserName = createUserModel.Email;
        
        var result = await _userManager.CreateAsync(user, createUserModel.Password);
        if (!result.Succeeded) throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);
        
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var userId = (await _userManager.FindByNameAsync(user.UserName)).Id;
        await ConfirmEmailAsync(new ConfirmEmailModel() { UserId = userId, Token = token });
        
        return new CreateUserResponseModel
        {
            Id = Guid.Parse(userId)
        };
    }
    
    public async Task<LoginResponseModel> LoginAsync(LoginUserModel loginUserModel)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == loginUserModel.Email);
        
        if (user == null)
            throw new NotFoundException("Username or password is incorrect");
        
        user.UserName = user.Email;
        
        if (await _userManager.IsLockedOutAsync(user))
            throw new BadRequestException("User is locked out");
        
        var signInResult = await _signInManager.PasswordSignInAsync(user, loginUserModel.Password, false, false);
        
        if (!signInResult.Succeeded)
            throw new BadRequestException("Username or password is incorrect");
        
        var token = JwtHelper.GenerateToken(user, _configuration);
        
        return new LoginResponseModel
        {
            Email = user.Email,
            Token = token
        };
    }
    
    public async Task<ConfirmEmailResponseModel> ConfirmEmailAsync(ConfirmEmailModel confirmEmailModel)
    {
        var user = await _userManager.FindByIdAsync(confirmEmailModel.UserId);
        
        if (user == null)
            throw new UnprocessableRequestException("Your verification link is incorrect");
        
        var result = await _userManager.ConfirmEmailAsync(user, confirmEmailModel.Token);
        
        if (!result.Succeeded)
            throw new UnprocessableRequestException("Your verification link has expired");
        
        return new ConfirmEmailResponseModel
        {
            Confirmed = true
        };
    }
    
    public async Task<BaseResponseModel> ChangePasswordAsync(Guid userId, ChangePasswordModel changePasswordModel)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        
        if (user == null)
            throw new NotFoundException("User does not exist anymore");
        
        var result =
            await _userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword,
                changePasswordModel.NewPassword);
        
        if (!result.Succeeded)
            throw new BadRequestException(result.Errors.FirstOrDefault()?.Description);
        
        return new BaseResponseModel
        {
            Id = Guid.Parse(user.Id)
        };
    }
}
