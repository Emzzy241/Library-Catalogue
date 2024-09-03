// Entity used to describe relationship between author and book

using LibraryCatalogue.Models;
using System.Collections.Generic;


namespace LibraryCatalogue.Models;
public class AuthorBook
{
    public int AuthorBookId { get; set; }
    public int AuthorId { get; set; }
    public int BookId { get; set; }
    public virtual Author Author { get; set; }
    public virtual Book Book { get; set; }
    
    
    
}