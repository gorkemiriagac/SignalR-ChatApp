using Microsoft.AspNetCore.Identity;

namespace SampleProject.Models
{
    public class AppAddFullUserName:IdentityUser
    {
        public string FullName { get; set; }


    }
}
