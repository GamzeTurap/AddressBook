using AddressBookEL.IdentityModels;
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

    }
}