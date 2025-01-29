using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Authorize.Models;
using Authorize.Scripts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorize.Controllers;


public class HomeController(ILogger<HomeController> logger, AuthorizeContext context) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    private readonly AuthorizeContext _context = context;

    public IActionResult Index()
    {
        var users = _context.Users.ToList().Select(m => new UserModel(m.Email ?? ""));
        return View();
    }

    [Authorize]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
