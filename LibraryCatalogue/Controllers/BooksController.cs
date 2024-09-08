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

   
    public BooksController(LibraryCatalogueContext db)
    {
        _db = db;
    }
    

    // public async Task<IActionResult> Index()
    // {
    //     // Below code is to verify if user is truly signed in & then get all books for them so it can be viewed
    //     // It can be nullable, hence the need for a question mark(?)
    //     string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //     ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //     List<Book> userBooks = _db.Books
    //                             .Where(entry => entry.User.Id == currentUser.Id)
    //                             .Include(book => book.Author)
    //                             .ToList();
    //     return View(userBooks);
    // }
     public IActionResult Index()
    {
        List<Book> books= _db.Books.Include(books => books.AuthorBooks).ThenInclude(authorbook => authorbook.Author).ToList();
        return View(books);
    }

    // [Authorize(Roles = "Librarian")]
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

    
    // [HttpPost]
    // public async Task<IActionResult> Create(Book book, int AuthorId)
    // {
    //     if(!ModelState.IsValid)
    //     {
    //         ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
    //         return View(book);
    //     }
    //     else
    //     {
    //         string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    //         ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
    //         book.User = currentUser;
    //         _db.Books.Add(book);
    //         _db.SaveChanges();
    //         return RedirectToAction("Index");
    //     }
    // }

    // Writing code for the User story: As a librarian, I want to be able to create books
    [Authorize(Roles = "Librarian")]
    [HttpPost]
     public IActionResult Create(Book book , int AuthorId)
        {
            _db.Books.Add(book);
            _db.SaveChanges();
            if(AuthorId != 0)
            {
                _db.AuthorBooks.Add(new AuthorBook{AuthorId = AuthorId , BookId = book.BookId});
            }
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    // The Read Action for all users irrespective of logging in
    public ActionResult Details(int id)
    {
        // Displaying a Book's details to all users
        Book thisBook = _db.Books
                        .Include(book => book.AuthorBooks)
                        .ThenInclude(authbk => authbk.Author)
                        
                        .FirstOrDefault(book => book.BookId == id);
        return View(thisBook);
    } 

    // [Authorize(Roles = "Librarian")]
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
   
    // [Authorize(Roles = "Librarian")]
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

    // The Edit() Action from books
    // [HttpGet]
    // public IActionResult Edit(int id)
    // {
    //     // We use the below code in finding an object based on its id... Hence, once the id from the book we want to get matches an id in our database, we would like to return that specific object
    //     Book thisBook = _db.Books.FirstOrDefault(book => book.BookId == id);
    //     if(thisBook == null)
    //     {
    //         return NotFound();
    //     }
    //     return View(thisBook);
    // } or use the .Find() method
    // [Authorize(Roles = "Librarian")]
    public IActionResult Edit(int id)
    {
        var book = _db.Books.Find(id); // Fetch the book that is being edited
        if (book == null)
        {
            return NotFound();
        }

        // Fetch authors from the database to populate the dropdown
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
        
        return View(book);
    }

        // var authorbook = _db.AuthorBooks.FirstOrDefault(authbk => authbk.AuthorId == AuthorId && authbk.BookId = book.BookId);
    
    // [Authorize(Roles = "Librarian")]
    // [HttpPost]
    // public IActionResult Edit(Book book, int AuthorId)
    // {
        
    //     if (ModelState.IsValid)
    //     {
    //         _db.Entry(book).State = EntityState.Modified;
    //         _db.SaveChanges();
    //         return RedirectToAction("Index");
    //     }

    //     // Repopulate authors list in case of an invalid model state
    //     ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
        
    //     return View(book);
    // }

    [HttpPost]
    public IActionResult Edit(Book book, int AuthorId)
    {
        var authorbook = _db.AuthorBooks.FirstOrDefault(authbk => authbk.AuthorId == AuthorId && authbk.BookId == book.BookId);
        if(authorbook == null)
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

    // Implementing the AddAuthor() action
    // [Authorize(Roles = "Librarian")]
    public IActionResult AddAuthor(int id)
    {
        var book = _db.Books.Find(id);
        // or use the previous: var book = _db.Books.FirstOrDefault(bk => bk.BookId == id);
        ViewBag.AuthorId = new SelectList(_db.Authors, "AuthorId", "Name");
        return View(book);
    }

    // [Authorize(Roles = "Librarian")]
    [HttpPost]
    public IActionResult AddAuthor(Book book, int AuthorId)
    {
        if(AuthorId != 0)
        {
            // Instantiating a new object as soon as the Author's id value is not 0, and giving this object the needed requirement(AuthorId, BookId) to make it properly instantiates
            _db.AuthorBooks.Add(new AuthorBook{ BookId = book.BookId, AuthorId = AuthorId});
        }
        _db.SaveChanges();
        return RedirectToAction("Details", new { id = book.BookId});
    }

    // [Authorize(Roles = "Librarian")]
    [HttpPost]
    public IActionResult RemoveAuthor(int AuthorBookId)
    {
        // var authorbook = _db.AuthorBooks.Find(AuthorBookId);
        // Performing comparison to determine to aid us in finding the Id of the AuthorBook that we want to delete
        var authorbook = _db.AuthorBooks.FirstOrDefault(authb => authb.AuthorBookId == AuthorBookId);
        _db.AuthorBooks.Remove(authorbook);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    // Implementing the Search Feature so a librarian can search for a book by author or title, so that Librarian can find a book when there are a lot of books in the library
    [HttpPost]
    public IActionResult Search(string bookName)
    {
        List<Book> book = _db.Books.Where(bk => bk.Name.ToLower().Contains(bookName.ToLower())).ToList();
        return View("Index", book);
    }

}

