using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalogue.Solution.Models;

namespace LibraryCatalogue.Controllers;

public class HomeController : Controller
{
    // The very first page
    public IActionResult Index()
    {
        // ViewBag.Cont 
        return View();
    }

    // The Privacy page
    public IActionResult Privacy()
    {
        return View();
    }
}
