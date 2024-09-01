using LibraryCatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;

namespace LibrariesController.Controllers;

public class AuthorsController : Controller
{
    private readonly LibraryCatalogueContext _db;

    // Writing the controller's constructor
    public AuthorsController(LibraryCatalogueContext db)
    {
        _db = db;
    }

    public IActionResult Index()
    {
        // using ViewBag For the Page's title
        ViewBag.PageTitle = "List of Authors";

        List<Author> model = _db.Authors.ToList(); // Using the ToList() method to convert all authors in my database to a C# list
        return View();
    }

    public IActionResult Create()
    {
        ViewBag.PageTitle = "Create an Author";

        return View();

    }

    [HttpPost]
    public IActionResult Create(Author author)
    {
        _db.Authors.Add(author);
        _db.SaveChanges();
        return  RedirectToAction("Index");
    }
}