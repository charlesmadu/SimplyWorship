using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SimplyWorship.Models;

namespace SimplyWorship.Controllers;

public class HomeController : Controller
{

   
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        
    }

    

    public IActionResult Index()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Index(string tester_text)
    {
        ViewData["message"] = tester_text;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }


    public IActionResult LivePage()
    {

        ViewData["message"] = "Simply Worship";
        return View();
    }



    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
