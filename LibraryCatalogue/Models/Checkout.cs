using System;
using System.Collections.Generic;
using LibraryCatalogue.Models;

namespace LibraryCatalogue.Models;
public class Checkout
{
    public int CheckoutId { get; set; }
    public int BookId { get; set; }
    public DateTime Date { get; set; }
    public bool Returned { get; set; }
    public virtual Book Book { get; set; }
    public virtual LibraryUser User {  get; set; }
    public string UserId { get; set; } // Foregign key to pass to AspNetUsers 
    
    
    
    // All books must be returned after/before one week of checkout
    // Implementing an IsDue() method to determine when a book has been due for a return after it was previously checked out
    public bool IsDue()
    {
        bool answer = false;
        int dateConfirm = DateTime.Now.CompareTo(this.Date.AddDays(7));
        if(dateConfirm > 0)
        {
            answer = true;
        }
        return answer;
    }

    
    
 
}