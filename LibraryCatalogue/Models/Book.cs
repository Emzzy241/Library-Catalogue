using LibraryCatalogue.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LibraryCatalogue.Models;
public class Book
{
    public int BookId { get; set; }

    // adding a Book name property
    // Using Annotations to tell users that a name for any book is needed
    [Required(ErrorMessage = "The book's Name cannot be empty!")]
    public string Name { get; set; }
    public string Description { get; set; }
    
    

    // Now its time for the Author for each book
    [Range(1, int.MaxValue, ErrorMessage = "You must add an author to your book. Have you created an author yet?")]
    public int AuthorId { get; set; }
    public Author Author { get; set; }
    // public List<BookTag> JoinEntities { get; set; }
    public ApplicationUser User { get; set; }

    // No constructor needed
}