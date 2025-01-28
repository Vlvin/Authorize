using Microsoft.AspNetCore.Mvc;
using Authorize.Models;
using Authorize.Scripts;


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
      return View();
    }


    [HttpPost]
    public IActionResult Sign(AuthModel auth)
    {
      string result;
      if (auth.register)
        result = new Authenticator().SignUp(auth);
      else
        result = new Authenticator().SignIn(auth);
      Console.WriteLine(result);
      return Redirect(Url.Action(controller: "Home", action: "Index") ?? "/"); 
    }

}
