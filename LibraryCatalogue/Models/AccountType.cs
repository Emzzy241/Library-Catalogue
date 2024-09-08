using Microsoft.AspNetCore.EntityFrameworkCore;
using Microsoft.Identity;
using System;
using LibraryCatalogue.Models;

namespace LibraryCatalogue.Models;
public class AccountType : IdentityUser
{
    public string Librarian { get; set; }
    public string Patron { get; set; }
    public string Admin { get; set; }    
    
}