using AddressBookEL.IdentityModels;
using AddressBookPL.Models;
using Azure.Identity;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace AddressBookPL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                //ayni userName'den varsa hata versin
                var sameUser = _userManager.FindByNameAsync(model.UserName).Result; //async bir metodun sonuna .Result yazarsak method senkron calisir.
                if (sameUser != null)
                {
                    ModelState.AddModelError("", "Bu kullanici sistemde mevcuttur! Farkli kullanici adi deneyiniz.");
                }
                //ayni userName'den varsa hata versin.
                sameUser = _userManager.FindByEmailAsync(model.UserName).Result; //async bir metodun sonuna .Result yazarsak method senkron calisir.
                if (sameUser != null)
                {
                    ModelState.AddModelError("", "Bu kullanici sistemde mevcuttur! Farkli kullanici adi deneyiniz.");
                }
                //Artik sisteme kayit olabilir.
                AppUser user = new AppUser()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Phone,
                    Email = model.Email,
                    UserName = model.UserName,
                    CreatedDate = DateTime.Now,
                    EmailConfirmed = true, //dogrula yapmis kabul ettik.
                    IsPassive = false
                };
                if (model.Birthdate != null)
                {
                    user.Birthdate = model.Birthdate;
                }//user kaydedilsin
                var Result = _userManager.CreateAsync(user, model.Password).Result;
                if (Result.Succeeded)
                {
                    //kullaniciya customer rolunu atayalim.
                    var roleResult = _userManager.AddToRoleAsync(user, "Customer").Result;
                    if (roleResult.Succeeded)
                    {
                        TempData["RegisterSuccessMsg"] = "Kayit basarili";
                    }
                    else
                    {
                        TempData["RegisterWarningMsg"] = "Kullanici olustu! Ancak rolu atanamadi.Sistem yoneticisine ulasarakrol atamasi yapilmalidir.!";
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Ekleme basarisiz!");
                    foreach (var item in Result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik hata olustu!" + ex.Message);
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var user = _userManager.FindByNameAsync(model.UserNameorEmail).Result;
                if (user == null)
                {
                    user = _userManager.FindByEmailAsync(model.UserNameorEmail).Result;
                }
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanici adi/Email ya da sifreniz hatalidir!");
                    return View(model);
                }
                //user'i bulduk
                var signinResult = _signInManager.PasswordSignInAsync(user, model.Password, true, true).Result;
                if (signinResult.Succeeded)
                {
                    //yonlendirme yapacak

                    if (_userManager.IsInRoleAsync(user, "Customer").Result)
                    {
                        TempData["LoggedInUserName"] = user.UserName;
                        return RedirectToAction("Index", "Home");
                    }
                    else if (_userManager.IsInRoleAsync(user, "Admin").Result)
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else if (signinResult.IsLockedOut)
                {
                    ModelState.AddModelError("", $"2 defa yanlis işlem yaptiginiz icin {user.LockoutEnd.Value.ToString("HH:mm:ss")} den sonra giris yapabilirsiniz.");
                    return View(model);

                }
                else
                {
                    ModelState.AddModelError("", "Giris basarısızdır!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Beklenmedik bir hata olustu!" + ex.Message);
                return View(model);
            }
        }
    }
}
