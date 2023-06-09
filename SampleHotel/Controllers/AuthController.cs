using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SampleHotel.Models;
using SampleHotel.ViewModels;

namespace SampleHotel.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AuthViewModel model, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/Panel/Profile");

            ModelState.Clear();
            if (!TryValidateModel(model.LoginModel, nameof(LoginViewModel)))
            {                
                return View(model);
            }            

            var user = await _userManager.FindByNameAsync(model.LoginModel.Username);
            if (user == null)
            {
                ModelState.AddModelError("1", "user not found");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user,
                          model.LoginModel.Password, true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();

        }

        [Route("/Auth/Register", Name = "Register")]
        public async Task<IActionResult> Register(AuthViewModel model, string returnUrl)
        {
            returnUrl = returnUrl ?? Url.Content("~/Panel/Profile");

            ModelState.Clear();
            if (!TryValidateModel(model.RegisterModel, nameof(RegisterViewModel)))
            {
                return View("Login",model);
            }

            var user = await _userManager.FindByNameAsync(model.RegisterModel.Username);
            if (user != null)
            {
                ModelState.AddModelError("1", "user exist");
                return View("Login", model);
            }

            user = new ApplicationUser
            {
                UserName = model.RegisterModel.Username,
                FullName = model.RegisterModel.FullName,
                EmailConfirmed = true,
                Country = model.RegisterModel.Country,
                City = model.RegisterModel.City,
                Street = model.RegisterModel.Street,
                No = model.RegisterModel.No,
                Email = model.RegisterModel.Email,
                PhoneNumber = model.RegisterModel.PhoneNumber,
            };


            var result =  await _userManager.CreateAsync(user, model.RegisterModel.Password);
            
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, 
                    model.RegisterModel.UserTypeId == UserType.Admin.Id ? "Admin" : "Client"
                    );

                await _signInManager.SignInAsync(user,true);
                return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError(string.Empty, "Unable to create new account");
            return View("Login", model);

        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
