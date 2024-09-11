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

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    // Injecting the DbContext into the AdminController; we need to add a consructor to the AdminController that injects the DbContext and assigns it to the _context variable
    private readonly LibraryCatalogueContext _context;
    private readonly UserManager<LibraryUser> _userManager;
    public AdminController(LibraryCatalogueContext context, UserManager<LibraryUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    // App Fully builds but I keep running into an Unhandled Error anytime I run dotnet watch rumn
    // TODO: Fix  the error related to dotnet watch run
    // Update the ManageUsers action to pass a list of users with their Id,Email, and Roles to the view
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ManageUsers()
    {
        // var users = _userManager.Users.ToList();, Since soft deletehas been implemented, we need to make room for it
        //  Fetch users who are not soft-deleted
        // Filter Non-Deleted Users: The key update is the filtering of users where IsDeleted is false, meaning only active (non-deleted) users will be displayed
        var users = _userManager.Users.Where(u => !u.IsDeleted).ToList();
        var model = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound(); // Handle user not found
        }

        // Check if the user is an admin
        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Admin"))
        {
            /*
                Backend Validation (Server-Side Check)
                Why it's necessary: Even if the frontend disables the delete button for admin accounts, someone could still attempt to bypass the UI (e.g., using tools like Postman or browser developer tools) and try to delete an admin via direct API calls.
                Best practice: The backend should always perform security checks and enforce restrictions because it's the source of truth for user permissions and business logic. The logic to prevent the deletion of an admin is done here, ensuring security.
            */
            TempData["ErrorMessage"] = "Admin accounts cannot be deleted.";
            return RedirectToAction("ManageUsers");
        }

        // Proceed with the deletion
        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "User deleted successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Error deleting the user.";
        }

        return RedirectToAction("ManageUsers");
    }

    [HttpPost]
    public async Task<IActionResult> SoftDeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        
        if (user == null)
        {
            return NotFound();
        }

        if (await _userManager.IsInRoleAsync(user, "Admin"))
        {
            return BadRequest("Cannot delete an Admin account.");
        }

        // Mark user as deleted
        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);

        return RedirectToAction("ManageUsers");
    }

    // An extra route for admins to view SoftDeleted users and even restore them 
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ViewDeletedUsers()
    {
        var users = await _userManager.Users.Where(u => u.IsDeleted).ToListAsync();
        var model = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.ToList()
            });
        }

        return View(model);
    }

    // Another action for admins that can restore a Soft-deleted user
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RestoreUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user != null && user.IsDeleted)
        {
            user.IsDeleted = false;
            await _userManager.UpdateAsync(user);

            // Set a success message in TempData
            TempData["SuccessMessage"] = "User account has been successfully restored.";
        }

        return RedirectToAction("ViewDeletedUsers");
    }










    // Optionally: actions to add or remove roles, delete users, etc.
}