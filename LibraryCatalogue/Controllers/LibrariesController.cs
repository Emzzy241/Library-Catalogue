// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Rendering;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.EntityFrameworkCore;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using LibraryCatalogue.Models;

// namespace LibraryCatalogue.Controllers;

// public class LibrariesController : Controller
// {

//     private readonly LibraryCatalogueContext _db;


//     public LibrariesController(LibraryCatalogueContext db)
//     {
//         _db = db;
//     }

//     public ActionResult Index()
//     {
//         List<Library> model = _db.Libraries.ToList();
//         ViewBag.PageTitle = "List of Libraries";
//         return View(model);
//     }

//     public ActionResult Create()
//     {
//         ViewBag.PageTitle = "Create a Library";
//     }
// }