namespace ForceGetCase.Application.Models.User;

public class CreateUserModel
{
    public string Email { get; set; }

    public string Password { get; set; }
}

public class CreateUserResponseModel : BaseResponseModel { }
