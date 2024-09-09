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
    private readonly UserManager<LibraryUser> _userManager;
    public AdminController(UserManager<LibraryUser> userManager)
    {
        _userManager = userManager;
    }
    // App Fully builds but I keep running into an Unhandled Error anytime I run dotnet watch rumn
    // TODO: Fix  the error related to dotnet watch run

    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var model = new List<RegisterViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var roleList = roles.ToList(); // Explicit conversion from IList<string> to List<string>
            model.Add(new RegisterViewModel
            {
                Email = user.Email,
                Roles = roleList
            });
        }

        return View(model);
    }

    // Optionally: actions to add or remove roles, delete users, etc.
}