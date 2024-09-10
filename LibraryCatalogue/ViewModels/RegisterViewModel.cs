using System.ComponentModel.DataAnnotations;

namespace LibraryCatalogue.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "All Fields must be inputted")]
    [DataType(DataType.Text)]
    [Display(Name = "First Name: ")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "All Fields must be inputted")]
    [DataType(DataType.Text)]
    [Display(Name = "Last Name: ")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "All Fields must be inputted")]
    [EmailAddress]
    [Display(Name = "Email Address: ")]
    public string Email { get; set; }

    [Required(ErrorMessage = "All Fields must be inputted")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "All Fields must be inputted")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password: ")]
    [Compare("Password", ErrorMessage = "The two passwords you entered do not match.")]
    public string ConfirmPassword { get; set; }

    // Update the AccountType to support selecting from predefined roles
    [Required(ErrorMessage = "Please select an account type. Note, regular users can't select admin account")]
    [Display(Name = "Select the type of account you would like to create: ")]
    public string SelectedRole { get; set; }

    // Provide a list of roles to choose from
    public List<string> Roles { get; set; } = new List<string> { "Librarian", "Patron", "Admin" };
}
