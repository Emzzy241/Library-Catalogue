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

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly UserManager<LibraryUser> _userManager;
    public AdminController(UserManager<LibraryUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> ManageUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var model = new List<UserViewModel>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            model.Add(new UserViewModel
            {
                Email = user.Email,
                Roles = roles
            });
        }

        return View(model);
    }

    // Optionally: actions to add or remove roles, delete users, etc.
}