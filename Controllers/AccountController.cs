using Microsoft.AspNetCore.Mvc;
using Authorize.Models;
using Microsoft.AspNetCore.Identity;
using Authorize.DBModels;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using MySqlX.XDevAPI;
using Microsoft.AspNetCore.Authorization;


namespace Authorize.Controllers;

public class AccountController(ILogger<HomeController> logger, UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login(string ReturnUrl) 
    {
      ViewBag.ReturnUrl = ReturnUrl;
      return View();
      // return RedirectToAction("Sign", routeValues: new { ReturnUrl, reg = true });
    }
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Login(string ReturnUrl, AuthModel authData)
    {
      var user = await _userManager.FindByEmailAsync(authData.Email);
      
      if (user == null || !await _userManager.CheckPasswordAsync(user, authData.Password))
        return Unauthorized();

      await _signInManager.SignInAsync(user, true);
      return Redirect(ReturnUrl);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout(string returnUrl)
    {
      await _signInManager.SignOutAsync();
      return Redirect(returnUrl);
    }

    [HttpGet]
    public IActionResult Register(string ReturnUrl) 
    {
      ViewBag.ReturnUrl = ReturnUrl;
      return View();
      // return RedirectToAction("Sign", routeValues: new { ReturnUrl, reg = true });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register(string ReturnUrl, AuthModel authData)
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

      await _signInManager.SignInAsync(user, true);
      // Here I somehow need to throw token to Authorization header
      return Redirect(ReturnUrl);
    }


    [HttpGet]
    public async Task<IActionResult> Delete(string email) 
    {
      var user = await _userManager.FindByEmailAsync(email);
      await _userManager.DeleteAsync(user ?? throw new InvalidOperationException("User not found"));
      return RedirectToAction("Index", "Home");
    }


}
