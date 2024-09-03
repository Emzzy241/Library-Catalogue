
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LibraryCatalogue.Models;

namespace LibraryCatalogue.Models;

public class LibraryCatalogueContext : IdentityDbContext<ApplicationUser>
{
    // Creating the database tables for my application
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<AuthorBook> AuthorBooks { get; set; }

    public LibraryCatalogueContext(DbContextOptions<LibraryCatalogueContext> options) 
        : base(options) 
    {
        
    }

}


