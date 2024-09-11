using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LibraryCatalogue.Models;
public class LibraryUser : IdentityUser
{
    public LibraryUser()
    {
        this.Checkouts  = new HashSet<Checkout>();
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public string FullName{
        get {
            return LastName + " " + FirstName;
            }

    }

    public virtual ICollection<Checkout> Checkouts { get; set; }

    // Implementing the soft delete
    // If we don’t want to delete the user and their associated data, we can implement a "soft delete" by adding a property like IsDeleted to your user entity. Instead of deleting the user, we would mark them as deleted
    public bool IsDeleted { get; set; }
        
        
}