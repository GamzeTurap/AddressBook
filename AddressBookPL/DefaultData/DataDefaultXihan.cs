using AddressBookBL.InterfacesOfManagers;
using AddressBookEL.IdentityModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;

namespace AddressBookPL.DefaultData
{
    public class DataDefaultXihan
    {
        public void CheckAndCreateRoles(RoleManager<AppRole> roleManager)
        {
            try
            {
                //admin // customer  // misafir vb...
                string[] roles = new string[] { "Admin", "Customer", "Guest" };

                // rolleri tek tek dönüp sisteme olup olmadığına bakacağız. Yoksa ekleyeceğiz.
                foreach (var item in roles)
                {
                    // ROLDEN YOK MU? 
                    if (!roleManager.RoleExistsAsync(item.ToLower()).Result)
                    {
                        //rolden yokmus ekleyelim
                        AppRole role = new AppRole()
                        {
                            Name = item,
                            ConcurrencyStamp=Guid.NewGuid().ToString()
                        };
                        var result = roleManager.CreateAsync(role).Result;
                    }
                }
            }
            catch (Exception ex)
            {
                // ex loglanabilİr
                // yazılımcıya acil başlıklı email gönderilebilir
            }
        }
        public void CreateAllCities(ICityManager cityManager)
        {
            try
            {
                //1) veritabanindaki illeri listeye alalim
                //2)Exceli acip satir satir okuyup
                //2.5) olmayan ili veritabanina ekleyelim.

                var cityList = cityManager.GetAll(x => !x.IsRemoved).Data;//1
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Excels");
                string fileName = Path.GetFileName("Cities.xlsx"); //?
                string filePath = Path.Combine(path, fileName);

                using (var excelBook=new XLWorkbook(filePath))//C:Users/../wwwroot/Excels/Cities.xlsx
                {
                    var rows = excelBook.Worksheet(1).RowsUsed();//82 satir var
                }
            }
            catch (Exception ex)
            {
                //loglanacak
            }
        }

    }
}