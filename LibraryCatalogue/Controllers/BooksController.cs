using LibraryCatalogue.Models;
using Microsoft.EntityFrameworkCore; // namespace required o make use of the Include() method
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // namespace required for SelectList() method
using Microsoft.AspNetCore.Authorization; // TO implement authorization in the 
using Microsoft.AspNetCore.Authentication;
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

    [HttpGet]
    public IActionResult Delete(int id)
    {
        Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
        
        if(thisBook == null)
        {
            return NotFound(); // Return a 404 error if the book is not found
        }
        return
         View(thisBook);
    }
   
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
       if(thisBook == null)
       {
            return NotFound(); // Handling a case where the book is not found
       }
       
        _db.Books.Remove(thisBook);
        _db.SaveChanges();
        return RedirectToAction("Index"); // Redirect to the Index view after deletion
    }

    // The Edit() Action fro books
    [HttpGet]
    public IActionResult Edit(int id)
    {
        // We use the below code in finding an object based on its id... Hence, once the id from the book we want to get matches an id in our database, we would like to return that specific object
        Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
        if(thisBook == null)
        {
            return NotFound();
        }
        return View(thisBook);
    }

    // The POST() Action that actually lets us edit a book
    [HttpPost]
    public IActionResult Edit(Book book, int AuthorId)
    {
        if(ModelState.IsValid)
        {
            var authorBook = _db.AuthorBooks.FirstOrDefault(authbk => authbk.AuthorId == AuthorId && authbk.BookId == book.BookId);
            if(authorBook == null)
            {
                if(AuthorId != 0)
                {
                    _db.AuthorBooks.Add(new AuthorBook {BookId = book.BookId, AuthorId = AuthorId});

                }
            }
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(book);
    }
}

