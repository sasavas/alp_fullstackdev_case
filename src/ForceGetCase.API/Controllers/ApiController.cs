using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForceGetCase.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class ApiController : ControllerBase { }
