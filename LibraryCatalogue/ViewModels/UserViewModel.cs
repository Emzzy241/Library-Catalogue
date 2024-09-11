// There was an error whenn passing in RegisterViewModel tthe ManageUsers.cshtml... RegisterViewModel does not contain a definition for id
// To delete a User we need to pass in the user's Id to the form so it can be posted back to the DeleteUser action in the AdminController

namespace LibraryCatalogue.ViewModels;

public class UserViewModel
{
    public string Id { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    
    
}