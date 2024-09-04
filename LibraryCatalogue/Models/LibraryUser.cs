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


        
        
}