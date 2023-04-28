
using AddressBookBL.EmailSenderBusiness;
using AddressBookBL.InterfacesOfManagers;
using AddressBookEL.IdentityModels;
using AddressBookEL.ViewModels;
using AddressBookPL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace AddressBookPL.Controllers
{
    public class HomeController : Controller
    {

        private readonly ICityManager _cityManager;
        private readonly IDistrictManager _districtManager;
        private readonly INeighbourhoodManager _neighbourhoodManager;
        private readonly IUserAddressManager _userAddressManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailManager;

        public HomeController(ICityManager cityManager, IDistrictManager districtManager, INeighbourhoodManager neighbourhoodManager, IUserAddressManager userAddressManager, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IEmailSender emailManager)
        {
            _cityManager = cityManager;
            _districtManager = districtManager;
            _neighbourhoodManager = neighbourhoodManager;
            _userAddressManager = userAddressManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailManager = emailManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //Identity'i kullandigimiz icin Autorize icine role eklenebilir.
        [Authorize(Roles="Customer,Guest")]
        public IActionResult AddAddress()
        {
            try
            {
                ViewBag.Cities = _cityManager.GetAll(x => !x.IsRemoved).Data;

                var user = _userManager.FindByNameAsync(HttpContext.User.Identity?.Name).Result;
                UserAddressVM model = new()
                {
                    UserId = user.Id
                };

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.Cities = new List<CityVM>();
                return View();

            }
        }
        [HttpGet]
        public JsonResult GetCityDistricts(int id)
        {
            try
            {
                var data = _districtManager.GetAll(x => x.CityId == id && !x.IsRemoved).Data;
                if (data == null)
                {

                    return Json(new { issuccess = false, message = "ilceler buunamadi!" });
                }


                return Json(new { issuccess = true, message = "ilceler geldi", data });
            }
            catch (Exception ex)
            {

                return Json(new { issuccess = false, message = ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetDistrictsNeigh(int id)
        {
            try
            {
                var data = _neighbourhoodManager.GetAll(x => x.DistrictId == id && !x.IsRemoved).Data;
                if (data == null)
                {

                    return Json(new { issuccess = false, message = "mahalleler bulunamadi!" });
                }


                return Json(new { issuccess = true, message = "mahalleler geldi", data });
            }
            catch (Exception ex)
            {

                return Json(new { issuccess = false, message = ex.Message });
            }
        }
        [HttpPost]
        [Authorize(Roles="Customer")]
        public JsonResult SaveAddress([FromBody]UserAddressVM model)
        {
            try
            {
                //StringBuilder sb = new StringBuilder();
                //sb.AppendLine("Verileri eksiksiz girdiğinize emin olun!");

                //foreach (var modelState in ViewData.ModelState.Values)
                //{
                //    foreach (ModelError error in modelState.Errors)
                //    {
                //        sb.AppendLine(error.ErrorMessage);
                //    }
                //}
                //return Json(new { issuccess = false, msg = sb.ToString() });

                //2. yöntem 
                string messages = string.Join("\n ", ModelState.Values
                                    .SelectMany(x => x.Errors)
                                    .Select(x => x.ErrorMessage));

                return Json(new { issuccess = false, msg = messages });
                model.CreatedDate = DateTime.Now;
                //yeni gelen adres varsayilan mi? Evet ise veritabanindaki diger varsayilani KALDIR!!

                if (model.IsDefaultAddress)
                {
                    var prevDefault = _userAddressManager.GetByConditions(x => x.IsDefaultAddress && !x.IsRemoved).Data;
                    if (prevDefault!=null)
                    {
                        prevDefault.IsDefaultAddress = false;
                        _userAddressManager.Update(prevDefault);

                    }
                }
                var result = _userAddressManager.Add(model);
                if (result.IsSuccess)
                {
                    return Json(new { issuccess = true, msg = "Yeni adres eklendi." });
                }
                return Json(new { issuccess = false, msg = "Ekleme Basarisiz" });
            }
            catch (Exception ex)
            {
                return Json(new {issuccess=false,msg=ex.Message});
            }

        }
        [HttpGet]
        public JsonResult GetNeighbourhoodPostalCode(int cityid,int districtid,int neighbourid)
        {
            try
            {
                var district = _districtManager.GetByConditions(
                    x => x.Id == districtid).Data;
                var neighbourhood = _neighbourhoodManager.GetByConditions(
                    x=> x.Id== neighbourid).Data;

                string url = "https://api.ubilisim.com/postakodu/il/" + cityid;

                using (WebClient client = new WebClient()) // HttpClient WebClient
                {
                    var response = client.DownloadString(url);
                    var dataAll = JsonConvert.DeserializeObject<ApiVM>(response);

                    var data = dataAll.postakodu.FirstOrDefault(
                    x => x.ilce.ToLower() == district.Name.ToLower()
                    && x.mahalle.ToLower() == neighbourhood.Name.ToLower());

                    if (data != null)
                    {
                        neighbourhood.PlateCode = data.pk;
                        _neighbourhoodManager.Update(neighbourhood);

                        return Json(new
                        {
                            issuccess = true,
                            msg = "Posta kodu bulundu",
                            data = data.pk
                        });

                    }
                    return Json(new { issuccess = false, msg = "Posta kodu bulunamadı!" });
                }

            }
            catch (Exception ex)
            {
                return Json(new { issuccess = false, msg = ex.Message });
            }
        }

    }
}