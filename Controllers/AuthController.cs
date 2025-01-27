using Microsoft.AspNetCore.Mvc;
using Authorize.Models;

namespace Authorize.Controllers;

public class AuthController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public AuthController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
   
    [HttpGet]
    public IActionResult Sign(bool reg)
    {
      ViewBag.reg = reg;
      return View(null);
    }


    [HttpPost]
    public IActionResult Sign(AuthModel auth)
    {
      Console.WriteLine(auth.Email);
      Console.WriteLine(auth.Password);
      return Ok($"GoodSign {auth.register}"); 
    }

}
