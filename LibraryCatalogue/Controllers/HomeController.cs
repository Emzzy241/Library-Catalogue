using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LibraryCatalogue.Solution.Models;

namespace LibraryCatalogue.Solution.Controllers;

public class HomeController : Controller
{
    // 
    public ActionResult Index()
    {
        return View();
    }
}
