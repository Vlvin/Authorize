using Microsoft.AspNetCore.Mvc;
using Authorize.Models;
using Microsoft.AspNetCore.Identity;
using Authorize.DBModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;


namespace Authorize.Controllers;

public class AuthController(ILogger<HomeController> logger, UserManager<User> userManager, IConfiguration configuration) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public IActionResult Index()
    {
        return View();
    }
   
    [HttpGet]
    public IActionResult Sign(bool reg)
    {
      ViewBag.reg = reg;
      return View();
    }



    [HttpPost]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Sign(AuthModel auth)
    {
      if (auth.register)
        return await Register(auth);
      else
        return await Login(auth);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Login(AuthModel authData)
    {
      var user = await _userManager.FindByEmailAsync(authData.Email);
      
      if (user == null || !await _userManager.CheckPasswordAsync(user, authData.Password))
        return Unauthorized();

      var claims = new List<Claim> 
      {
        new (ClaimTypes.Email, user.Email ?? ""),
        new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is not set")));

      var token = new JwtSecurityToken(
        issuer: _configuration["JWT:ValidIssuer"] ?? throw new InvalidOperationException("JWT:ValidIssuer is not set"),
        audience: _configuration["JWT:ValidAudience"] ?? throw new InvalidOperationException("JWT:ValidAudience is not set"),
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
      );
      // Here I somehow need to throw token to Authorization header
      return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(AuthModel authData)
    {
      var existingUser = await _userManager.FindByEmailAsync(authData.Email);
      if (existingUser != null)
        return Conflict("User already exists.");
      var user = new User {
        Email = authData.Email,
        UserName = authData.Email.Split('@')[0],
        SecurityStamp = Guid.NewGuid().ToString()
      };
      var result = await _userManager.CreateAsync(user, authData.Password);
      if (!result.Succeeded)
        return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to add user: {string.Join(" ", result.Errors.Select(x => x.Description))}");
      
      var claims = new List<Claim> 
      {
        new (ClaimTypes.Email, user.Email ?? ""),
        new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is not set")));

      var token = new JwtSecurityToken(
        issuer: _configuration["JWT:ValidIssuer"] ?? throw new InvalidOperationException("JWT:ValidIssuer is not set"),
        audience: _configuration["JWT:ValidAudience"] ?? throw new InvalidOperationException("JWT:ValidAudience is not set"),
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(30),
        signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
      );
      
      SignIn(new ClaimsPrincipal(new ClaimsIdentity(claims)));

      // Here I somehow need to throw token to Authorization header
      return RedirectToAction("Index", "Home");
    }


}
