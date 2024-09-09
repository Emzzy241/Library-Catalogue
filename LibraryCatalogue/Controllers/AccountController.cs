using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering; // namespace for importing the SelectList()
using LibraryCatalogue.Models;
using LibraryCatalogue.ViewModels;
using System.Threading.Tasks;

namespace LibraryCatalogue.Controllers;
public class AccountController : Controller
{
    private readonly LibraryCatalogueContext _db;
    private readonly UserManager<LibraryUser> _userManager;
    private readonly SignInManager<LibraryUser> _signInManager;
    // Implementing the RoleManager From Identity to help define Roles that my users can perform
    private readonly RoleManager<IdentityRole> _roleManager;

    // No need to make everything authorized only with the ApplicationUser role, because now we have written our own roles: LibraryUser and Patrons
    public AccountController(LibraryCatalogueContext db, UserManager<LibraryUser> userManager, SignInManager<LibraryUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _db = db;
        _roleManager = roleManager;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    // Looking at the Register() now,
    // The method's signature: This method is an async Task because creating user accounts will be an asynchronous action. Our Register() action doesn't return an ActionResult. Instead, it returns a Task containing an ActionResult. Remember, the built-in Task<TResult> class represents asynchronous actions

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        else
        {
            // ApplicationUser user = new ApplicationUser { UserName = model.Email };
            
            var user = new LibraryUser{ UserName = model.Email, FirstName = model.FirstName, LastName = model.LastName };
            IdentityResult result = await _userManager.CreateAsync(user, model.Password);
            if(result.Succeeded)
            {
                // await _userManager.AddToRoleAsync(user, "Patron");
                // return RedirectToAction("Login");
                
                // Checking if its the admin account
                if(user.Email == "admin@library.com")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
                else{
                    // Once the inputted email is not the admin's email, We assign role based on the option selected from the dropdown by user
                    await _userManager.AddToRoleAsync(user, model.Role);

                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");


            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
        }

         // If we got this far, something failed; redisplay form
        return View(model);
    }

    // THe HttpPost for user's logging in
    // We don't need to specify its a get request; by default Asp.Net knows
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        else
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("", "You have enterred an incorrect email or password. Please check both and try again");
                return View(model);
            }
        }
    }

    public async Task<IActionResult> LogOff()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index");
    }
}