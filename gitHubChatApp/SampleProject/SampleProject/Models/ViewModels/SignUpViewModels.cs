using System.ComponentModel.DataAnnotations;

namespace SampleProject.Models.ViewModels
{
    public record SignUpViewModels(string FullName,[Required] string Email, [Required] string Password, [Required] string ConfirmPassword);
    
}
