using LibraryCatalogue.Models;
using System;
using System.Collections.Generic;

namespace LibraryCatalogue.Models;

public class Author
{
    public Author()
    {
        this.AuthorBooks = new HashSet<AuthorBook>();
    }
    public string Name { get; set; }
    
    public int AuthorId { get; set; }

    public virtual ICollection<AuthorBook> AuthorBooks { get; set; }
    


    // No longer used the navigation property
    // public List<Book> Books { get; set; } // This is the navigation property, specifically a 'collection navigation property'

    // A navigaton property is a property on one entity(like Author) that includes a reference to a related entity (like Book)
    // Ef core uses navigation property to recognize a relationship between an Author and Book
    // In this case, EF core sees that the Books property has the type List<Book> which references another entity Book within our project
    // And because of that it is able to understand that there is a relatinship between Author and Book
    // The Books property is more specifically categorized as a collection navigation property because is includes multiple entries.
    // In this case, we have a collection(List<>) of multiple Book objects
    // Notably, navigatioin property are never saved in our database, they are populated in our projects EF core from the data in the database 

}