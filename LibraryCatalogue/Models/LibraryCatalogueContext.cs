
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryCatalogue.Models;
using Microsoft.AspNetCore.Identity;

namespace LibraryCatalogue.Models;

public class LibraryCatalogueContext : IdentityDbContext<LibraryUser, IdentityRole, string>
{
    // Creating the database tables for my application
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }
    public DbSet<Checkout> Checkouts { get; set; }

    // Adding this new entity in a bid to be able to allow users create any account of this 3: Librarian, Patron, and Admin
    public DbSet<AccountType> AccountTypes { get; set; }

    public LibraryCatalogueContext(DbContextOptions<LibraryCatalogueContext> options) 
        : base(options) 
    {
        
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseLazyLoadingProxies();
    // }

}


