using Identity.Entities;
using Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        public HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SignUp()
        {
            return View(new AppUserCreateModel());
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(AppUserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser()
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Gender = model.Gender
                };          
                var identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index");
                }
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description); 
                }
            }
            return View(model);
        }
    
        public IActionResult SignIn(string returnUrl)
        {
            return View(new AppUserSignInModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(AppUserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (signInResult.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.UserName);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        if(!string.IsNullOrEmpty(model.ReturnUrl))
                            return Redirect(model.ReturnUrl);
                        return RedirectToAction("AdminPanel");
                    }
                    else
                        return RedirectToAction("Panel");
                }
                else
                    ModelState.AddModelError("", "Username or Password are wrong");
            }
            return View(model);
        }
    
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserInformation()
        {
            var userName = User.Identity.Name;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            string roleValue = role.Value;
            return View();
        }

        [Authorize(Roles = "Member,Admin")]
        public IActionResult Panel()
        {

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminPanel()
        {

            return View();
        }

        [Authorize]
        public IActionResult ChangeLoggedInUser()
        {
            return View();
        }

        public async Task<IActionResult> Signout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
