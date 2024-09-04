// Using data Annotations like [DataType.Password], [Display], [EmailAddress], [Compare]
// The major reason for ViewModels is because of the Confirm password feature(collecting user's password details a second time)
// To avoid those type of code in our Models, we create ViewModels

using System.ComponentModel.DataAnnotations;

namespace LibraryCatalogue.ViewModels;

public class RegisterViewModel
{

    [Required(ErrorMessage = "All Fields Must be inputted")]
    [DataType(DataType.Text)]
    [Display(Name = " First Name:  ")]
    public string FirstName { get; set; }


    [Required(ErrorMessage = "All Fields Must be inputted")]
    [DataType(DataType.Text)]
    [Display(Name = " Last Name: ")]
    public string LastName { get; set; }


    [Required(ErrorMessage = "All Fields Must be inputted")]
    [EmailAddress]
    [Display(Name = "Email Address: ")]
    public string Email { get; set; }

    [Required(ErrorMessage = "All Fields Must be inputted")]
    [DataType(DataType.Password)]
    // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    // After I push to production, I will use this: [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    public string Password { get; set; }

    [Required(ErrorMessage = "All Fields Must be inputted")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password: ")]
    [Compare("Password", ErrorMessage = "The two passwords you enterred do not match.")]
    public string ConfirmPassword { get; set; }
}