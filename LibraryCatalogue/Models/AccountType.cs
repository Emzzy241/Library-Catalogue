using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using LibraryCatalogue.Models;

namespace LibraryCatalogue.Models;
public class AccountType : IdentityUser
{
    // Giving the properties a default value to ensure the AccountType entity represents the roles(Admin, Librarian, Patron)
    public string Librarian { get; set; } = "Librarian";
    public string Patron { get; set; } = "Patron";
    public string Admin { get; set; } = "Admin";
    
}