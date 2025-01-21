using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SampleProject.Models;
using SampleProject.Models.ViewModels;

namespace SampleProject.Controllers
{
    public class HomeController(ILogger<HomeController> logger,UserManager<AppAddFullUserName> userManager,SignInManager<AppAddFullUserName>signInManager,AppDbContext context) : Controller
    {
       

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> SignUp(SignUpViewModels model)
        {
            if (!ModelState.IsValid) return View(model);

            var userToCreate = new AppAddFullUserName()
            {
                UserName = model.Email,
                Email = model.Email,
                FullName=model.FullName
            };
            var result= await userManager.CreateAsync(userToCreate,model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }



            return RedirectToAction(nameof(SignIn));
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task <IActionResult> SignIn(SignInViewModels model)
        {
            if (!ModelState.IsValid) return View(model);

            var hasUser= await userManager.FindByEmailAsync(model.Email);

            if(hasUser is null)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong");
                return View(model);
            }


            var result= await signInManager.PasswordSignInAsync(hasUser,model.Password,true,false); // 1.true remember me ben ihatýrla olsun mu
            //2.true ise þifre 5 kere yanlýs girince kitleme olsun mu

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Email or Password is wrong");
                return View(model);
            }
            return RedirectToAction(nameof(Index));
        }
        
        public async Task <IActionResult> SignOut()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));

        }
       




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
