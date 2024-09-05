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
using System.Linq;

namespace LibraryCatalogue.Controllers;

// [Authorize(Roles = "Librarian")]
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
        
        // I gave the list of authors a descriptive name instead of just model
        List<Author> listOfAuthors = _db.Authors.ToList(); // Using the ToList() method to convert all authors in my database to a C# list
        return View(listOfAuthors);
    }

    public IActionResult Create()
    {
        

        return View();

    }

    [HttpPost]
    public IActionResult Create(Author author)
    {
        _db.Authors.Add(author);
        _db.SaveChanges();
        return  RedirectToAction("Index");
    }

    // The Details() Action for the Author's controller
    // [AllowAnonymous]
    //     public IActionResult Details(int id)
    //     {
    //          var author = _db.Authors.Include(auth => auth.AuthorBooks).ThenInclude(authbk => authbk.Book).FirstOrDefault(auth => auth.AuthorId == id);
    //          return View(author);
    //     }


    [AllowAnonymous] 
    public ActionResult Details(int id)
    {
               
        // Author has only one navigation property, Author.Books; this is why there is only one .Include() method call. If we want to want to access each item's tag(s), we need to use a series of .ThenInclude() method calls to get the Item.JoinEntities data for each item, and then the joinEntity.Tag data for each entity
        Author thisAuthor = _db.Authors
                                .Include(Author => Author.AuthorBooks) 
                                .ThenInclude(authbk => authbk.Book)
                                .FirstOrDefault(Author => Author.AuthorId == id);

        if(thisAuthor == null)
        {
            return NotFound();
        }   
        return View(thisAuthor);
        //Just like before, If we don't explicitly tell EF Core to include the navigation property Author It won't. However We'll still get the Author.AuthorId, Author.Name information, but the navigation property Author will be empty
    }


    // GET: Authors/Delete
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var author = _db.Authors.FirstOrDefault(a => a.AuthorId == id);

        if (author == null)
        {
            return NotFound(); // Return a 404 error if the author is not found
        }

        return View(author); // Show the delete confirmation view
    }


   // POST: Authors/Delete/5
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var author = _db.Authors.FirstOrDefault(a => a.AuthorId == id);

        if (author == null)
        {
            return NotFound(); // Handle the case where the author is not found
        }

        _db.Authors.Remove(author);
        _db.SaveChanges();

        return RedirectToAction("Index"); // Redirect to the Index view after deletion
    }

    // The Edit() Action
    public IActionResult Edit(int id)
    {
        Author thisAuthor = _db.Authors.FirstOrDefault(author => author.AuthorId == id);

        if(thisAuthor == null)
        {
            return NotFound();
        }
        return View(thisAuthor);
    }

    [HttpPost]
    public IActionResult Edit(Author author)
    {
        if (ModelState.IsValid)
        {
            // Correct way to mark the entity as modified
            _db.Entry(author).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // If the model state is invalid, return the same view with the model data
        return View(author);
    }




}