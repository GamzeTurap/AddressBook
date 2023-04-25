using AddressBookDL;
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
    options.User.AllowedUserNameCharacters = "qwertyuopasdfghjklizxcvbnm0123456789-_";

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
//buraya geri donecegiz

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



app.Run(); //uygulamayi calistirir.
