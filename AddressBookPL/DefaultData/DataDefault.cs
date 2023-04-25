using AddressBookEL.IdentityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AddressBookPL.DefaultData
{
    public static class DataDefault
    {
        public static IApplicationBuilder Data(this IApplicationBuilder app) //extension metot
        {
            //Managerlar'i kullaabilmek icin EXTANTION

            using var scopedServices = app.ApplicationServices.CreateScope();

            var serviceProvider = scopedServices.ServiceProvider;

            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            CheckAndCreateRoles(roleManager); // roleManager olustu ve simdi rolleri olusturan metodu cagirrabiliriz
            return app;
        }
        public static void  CheckAndCreateRoles(RoleManager<AppRole> roleManager)
        {
            try
            {
                //admin //customer //misafir vb...
                string[] roles = new string[] { "admin", "customer", "guest" };

                //rolleri tek tek donup sisteme olup olmadigina bakacagiz. Yoksa ekleyecegiz.
                foreach (var item in roles)
                {
                    //rolden yok mu?
                    if (!roleManager.RoleExistsAsync(item).Result)
                    {
                        //rolden yokmus ekleyelim
                        AppRole role = new AppRole()
                        {
                            Name = item
                        };
                        var result = roleManager.CreateAsync(role).Result;
                    }
                }
            }
            catch (Exception ex)
            {

                //ex loglanabilir
                //yazilimciya acil baslikli email gonderilebilir.
            }
        }
    }
}
