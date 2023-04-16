using Autotest.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Autotest.Mvc.Repositories;
using Autotest.Mvc.Services;
using DataBase.Entietes;

namespace Autotest.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly UserRepository _userRepository;
    private readonly ILogger<HomeController> _logger;
    private readonly UsersService   _usersService;
    public HomeController(ILogger<HomeController> logger, UserRepository userRepository, UsersService usersService)
    {
        _logger = logger;
        _userRepository = userRepository;
        _usersService = usersService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Dashboard()
    {
        if(!_usersService.IsLoggedIn(HttpContext))
        {
            return RedirectToAction("SignUp", "Users");
        }
        else
        {
            var userId = HttpContext.Request.Cookies["user_id"];
            var user = _userRepository.GetUserById(userId!);
            return View(user);
        }
    }
}