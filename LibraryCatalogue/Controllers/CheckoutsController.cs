using LibraryCatalogue.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace LibraryCatalogue.Controllers;
public class CheckoutsController : Controller
{
    private readonly LibraryCatalogueContext _db;
    private readonly SignInManager<LibraryUser> _signInManager;
    private readonly UserManager<LibraryUser> _userManager;

    public CheckoutsController(LibraryCatalogueContext db, SignInManager<LibraryUser> signInManager, UserManager<LibraryUser> userManager)
    {
        _db = db;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        // using ? because there are times when the userId can be zero... Its even one of the big reasons why we used var to declare our variable, we could have simply used somethng else like an int since user's id is expected to be an integer
        // Using var to declareour variables when writing async code is very beneficial because since its a code that runs later, we don't have an idea on what it will return

        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var currentUser = await _userManager.FindByIdAsync(userId);
        var model = _db.Checkouts.Include(checkout => checkout.Book)
                                 .Where(entry => entry.User.Id == currentUser.Id)
                                 .OrderByDescending(checkout => checkout.CheckoutId).ToList();
        // Remmeber, the ToLIst() method is a method that helps in converting the output from our database into a C# List<T>
        ViewBag.ErrorMessage = "";
        return View(model);
    }

    public async Task<IActionResult> Create(int bookId)
    {
        var book = _db.Books.FirstOrDefault(bk => bk.BookId == bookId);
        if(book.Copies == 0)
        {
            ViewBag.ErrorMessage = $"Sorry {book.Name} is finished in this library";
            return RedirectToAction("Details", "Books", new {@id = bookId});
        }
        else{
            // AN else statement to handle when there are Copies of a book in the library
            book.Copies = book.Copies - 1;
            _db.Entry(book).State = EntityState.Modified;
            _db.SaveChanges();
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUser = await _userManager.FindByIdAsync(userId);
            var checkout = new Checkout{BookId = bookId, User = currentUser, Date = DateTime.Now, Returned = false}; // setting properties of a newly instantiated Checkout object
            _db.Checkouts.Add(checkout); // Adding the checkout into the database so it can be created
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }

    // AllCheckouts() action
    // [Authorize(Roles = "Librarian")]
    public IActionResult AllCheckouts()
    {
        ViewBag.ErrorMessage = "";
        var model = _db.Checkouts
                        .Include(checkout => checkout.Book)
                        .Include(checkout => checkout.User)
                        .OrderBy(checkout => checkout.Returned)
                        .ToList();

        return View(model);
    }

    // The Search() Action so users can search all of the books that they have previously checked out
    [HttpPost]
    public IActionResult Search(string userName)
    {
        var model = _db.Checkouts
                        .Include(checkout => checkout.Book)
                        .Include(checkout => checkout.User)
                        .Where(ck => ck.User.FirstName.Contains(userName.ToLower()) || ck.User.LastName.Contains(userName.ToLower()))
                        .OrderBy(ck => ck.Returned)
                        .ToList();

        return View("AllCheckouts", model);
    }


    // [Authorize(Roles  = "Librarian")]
    public IActionResult ReturnBook(int checkoutId, int bookId)
    {
        var book = _db.Books.FirstOrDefault(bk => bk.BookId == bookId);
        book.Copies = book.Copies + 1;
        _db.Entry(book).State = EntityState.Modified;
        _db.SaveChanges();
        var checkout = _db.Checkouts.FirstOrDefault(ck => ck.CheckoutId == checkoutId);
        checkout.Returned = true;
        _db.Entry(checkout).State = EntityState.Modified;
        _db.SaveChanges();
        return RedirectToAction("AllCheckouts");
    }

    // [Authorize(Roles = "Librarian")]
    public IActionResult Delete(int checkoutId)
    {
        var checkout = _db.Checkouts.FirstOrDefault(ck => ck.CheckoutId == checkoutId);
        if(checkout.Returned != true)
        {
            // Allowing users delete a book only when they have returned such book 
            ViewBag.ErrorMessage = "You cannot delete  a book without returning it!";
            var model = _db.Checkouts
                            .Include(ck => ck.Book)
                            .Include(ck => ck.User)
                            .OrderBy(ck => ck.Returned)
                            .ToList();
            return View("AllCheckouts", model);
        }
        else
        {
            _db.Checkouts.Remove(checkout);
            _db.SaveChanges();
            return RedirectToAction("AllCheckouts");
        }

    }




}