
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalogue.Models;

public class LibraryCatalogueContext : IdentityDbContext<ApplicationUser>
{
    // Creating the database tables for my application
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }

    public LibraryCatalogueContext(DbContextOptions<LibraryCatalogueContext> options) 
        : base(options) 
    {
        
    }

    // Additional configurations if necessary
}



/* using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalogue.Models;

public class LibraryCatalogueContext : IdentityDbContext<ApplicationUser>
{
    // Creating the databasase tables for my application
    // Checkouts table is a join table between patrons and copies
    // For copies table, a book should be able to have many copies
    // public DbSet<Librarian>  Librarians { get; set; }  
    // public DbSet<Patron> Patrons { get; set; }
    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    // public DbSet<Copy> Copies { get; set; }
    // public DbSet<Checkout> Checkouts { get; set; }

    public LibraryCatalogueContext(DbContextOptions options ) : base(options) {}

    // With this, I can now do dotnet migrations so all my tables get created, but first; I have to create the database itself or dotnet migrations help me in creating it explicitly
    

}

*/
