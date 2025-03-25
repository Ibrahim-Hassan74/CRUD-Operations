using IdentityEntities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Threading.Tasks;

namespace CRUDExample.Controllers
{
    //[Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Authorize("NotAuthorized")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                ViewBag.Errors = error;
                return View(registerDTO);
            }

            ApplicationUser user = new ApplicationUser()
            {
                PersonName = registerDTO.PersonName,
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.Phone
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                if (registerDTO.UserType == ServiceContracts.Enums.UserTypeOptions.Admin)
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.Admin.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.Admin.ToString() });
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.Admin.ToString());
                }
                else
                {
                    if (await _roleManager.FindByNameAsync(UserTypeOptions.User.ToString()) is null)
                    {
                        await _roleManager.CreateAsync(new ApplicationRole() { Name = UserTypeOptions.User.ToString() });
                    }
                    await _userManager.AddToRoleAsync(user, UserTypeOptions.User.ToString());
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Persons");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Register", error.Description);
            }
            return View(registerDTO);
        }

        [Authorize("NotAuthorized")]
        public IActionResult Login()
        {
            return View();
        }

        [Authorize("NotAuthorized")]
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(loginDTO);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDTO.Email, loginDTO.Password, isPersistent: false, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);

                if (await _userManager.IsInRoleAsync(user, UserTypeOptions.Admin.ToString()))
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                {
                    return LocalRedirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Persons");
            }

            if (result.IsLockedOut)
            {
                ApplicationUser user = await _userManager.FindByEmailAsync(loginDTO.Email);
                var lockoutEnd = await _userManager.GetLockoutEndDateAsync(user);
                if (lockoutEnd.HasValue)
                {
                    TimeZoneInfo egyptTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
                    DateTime lockoutEndLocal = TimeZoneInfo.ConvertTimeFromUtc(lockoutEnd.Value.UtcDateTime, egyptTimeZone);
                    var remainingTime = lockoutEndLocal - DateTime.Now;

                    ModelState.AddModelError("Login",
                        $"Account is locked out until {lockoutEndLocal:HH:mm:ss} (Egypt Time). " +
                        $"Try again in {remainingTime.Minutes} minutes.");
                }
                else
                {
                    ModelState.AddModelError("Login", "Account is locked out. Try again later.");
                }
                return View(loginDTO);
            }


            ModelState.AddModelError("Login", "Invalid Email or password");

            return View(loginDTO);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return Json(result == null);
        }
    }
}
