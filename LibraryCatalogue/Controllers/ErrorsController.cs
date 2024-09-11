using LibraryCatalogue.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq; // For accessing the .ToList() method that is capable of converting IList<T> to List<T>. The ToList() method can also convert items in ou rdatabase into a C# List<T>
using LibraryCatalogue.ViewModels;

namespace LibraryCatalogue.Controllers;
    [AllowAnonymous]
    public class ErrorsController : Controller
    {
        [Route("Error/404")]
        public IActionResult NotFound()
        {
            return View();
        }

        [Route("Error/403")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [Route("Error/500")]
        public IActionResult InternalServerError()
        {
            return View();
        }
    }