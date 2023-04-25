using Microsoft.AspNetCore.Mvc;

namespace AddressBookPL.Controllers
{
    public class AccountController1 : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
    }
}
