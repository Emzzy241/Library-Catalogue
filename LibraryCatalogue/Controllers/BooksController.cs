using LibraryCatalogue.Models;
using Microsoft.EntityFrameworkCore; // namespace required o make use of the Include() method
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // namespace required for SelectList() method
using Microsoft.AspNetCore.Authorization; // TO implement authorization in the BooksController
using Microsoft.AspNetCore.Identity; // using the Microsoft Identity namespace so our controller can make use of the UserManager and other tools from identity to get users from the database
using System;
using System.Linq; // This is allows to call the ToList() method on our database
using System.Threading.Tasks; // This is necessary to call async methods, te async methods we will use from the UserManager class
using System.Security.Claims; //This namespace is necessary for using claim based authorization
using System.Collections.Generic;

namespace LibraryCatalogue.Controllers;

[Authorize]
public class BooksController : Controller
{
    private readonly LibraryCatalogueContext _db;

    // Adding Identity's UserManager class to our controller so we can access tools to get us data about the signed-in user
    // TO do this we will add a new property in the controller ad also update our constructor with that property
    private readonly UserManager<ApplicationUser> _userManager;

    public BooksController(LibraryCatalogueContext db, UserManager<ApplicationUser> userManager)
    {
        _db = db;
        _userManager = userManager;
    }
    

    public async Task<IActionResult> Index()
    {
        // Below code is to verify if user is truly signed in & then get all books for them so it can be viewed
        // It can be nullable, hence the need for a question mark(?)
        string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
        List<Book> userBooks = _db.Books
                                .Where(entry => entry.User.Id == currentUser.Id)
                                .Include(book => book.Author)
                                .ToList();
        return View(userBooks);
    }

    // The Create Action for logged in users
    public IActionResult Create()
    {
        ViewBag.PageTitle = "Create A Book";
        // SelectList() method provides a list of the data needed to create an html <select> list of all the books from our database.
        //  The displayed text of each option will be the Authors's Name property, and the value of the <option> will be the Authors's AuthorsId. 
        // That way, a user can select an Authopr from the dropdown to associate with the Book we are creating or editting.
        // 
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Book book, int AuthorId)
    {
        if(!ModelState.IsValid)
        {
            ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
            return View(book);
        }
        else
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            book.User = currentUser;
            _db.Books.Add(book);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    // The Read Action for all users irrespective of logging in
    public ActionResult Details(int id)
    {
        // Displaying a Book's details to all users
        Book thisBook = _db.Books
                        .Include(book => book.Author)
                        // .Include(book => book.JoinEntities)
                        // .ThenInclude(join => join.Tag)
                        .FirstOrDefault(book => book.BookId == id);
        return View(thisBook);
    } 
}