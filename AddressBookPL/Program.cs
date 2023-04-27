using AddressBookBL.EmailSenderBusiness;
using AddressBookBL.ImplementationsOfManagers;
using AddressBookBL.InterfacesOfManagers;
using AddressBookDL;
using AddressBookDL.ImplementationsOfRepo;
using AddressBookDL.InterfacesOfRepo;
using AddressBookEL.IdentityModels;
using AddressBookEL.Mapping;
using AddressBookPL.DefaultData;
using AutoMapper.Extensions.ExpressionMapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Local"));
});

var lockoutOptions = new LockoutOptions()
{
    AllowedForNewUsers = true,
    DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1), //sure
    MaxFailedAccessAttempts = 2 //ne kadar fail olacak
};

//identity ayarlari
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    //ayarlar eklenecek 
    options.Password.RequiredLength = 4;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false; //@ / () [] {} ? : ; karakter
    options.Password.RequireDigit = false;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "qwertyuopasdfghjklizxcvbnm0123456789-_QWERTYUIOPASDFGHJKLZXCVBNM";
    options.Lockout = lockoutOptions;

}).AddDefaultTokenProviders().AddEntityFrameworkStores<MyContext>();

//auto mapper ayarlari
builder.Services.AddAutoMapper(x =>
{
    x.AddExpressionMapping();
    x.AddProfile(typeof(Maps));
});

// Add services to the container.
builder.Services.AddControllersWithViews();

//interfacelerin DI icin yasam donguleri (AddScoped)
builder.Services.AddScoped<ICityRepo, CityRepo>();
builder.Services.AddScoped<ICityManager, CityManager>();

builder.Services.AddScoped<IDistrictRepo, DistrictRepo>();
builder.Services.AddScoped<IDistrictManager, DistrictManager>();

builder.Services.AddScoped<INeighbourhoodRepo, NeighbourhoodRepo>();
builder.Services.AddScoped<INeighbourhoodManager, NeighbourhoodManager>();

builder.Services.AddScoped<IUserAddressRepo, UserAddressRepo>();
builder.Services.AddScoped<IUserAddressManager, UserAddressManager>();

builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles(); //wwwroot

app.UseRouting();

app.UseAuthentication(); //login logout
app.UseAuthorization(); //yetki

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//proje ilk calisacagi zaman default olarak eklenmesini istedigimiz verileri ya da baska islemleri yazdigimiz classi burada cagirmaliyiz.


//app.Data(); //extantion metot olarak cagirmak
//DataDefault.Data(app); //harici cagirmak

//Xihan Shen ablanin yonteminden yapalim boylece Erdener'in static olmasin istedigini yerine getirelim.


using (var scope = app.Services.CreateScope())
{
    //Resolve ASP .NET Core Identity with DI help
    var userManager = (UserManager<AppUser>?)scope.ServiceProvider.GetService(typeof(UserManager<AppUser>));
    var roleManager = (RoleManager<AppRole>?)scope.ServiceProvider.GetService(typeof(RoleManager<AppRole>));
    // do you things here

    var cityManager = (ICityManager?)scope.ServiceProvider.GetService(typeof(ICityManager));
    var districtManager = (IDistrictManager?)scope.ServiceProvider.GetService(typeof(IDistrictManager));
    var neighbourhoodManager = (INeighbourhoodManager?)scope.ServiceProvider.GetService(typeof(INeighbourhoodManager));

    DataDefaultXihan d = new DataDefaultXihan();

    d.CheckAndCreateRoles(roleManager);
    d.CreateAllCities(cityManager);
    d.CreateAllDistricts(districtManager);
    d.CreateSomeNeighbourhood(neighbourhoodManager, cityManager, districtManager);
}
app.Run(); //uygulamayi calistirir.
