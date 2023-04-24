using AddressBookEL.IdentityModels;
using AddressBookEL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookDL
{
    public class MyContext : IdentityDbContext<AppUser, AppRole, string>
    {
        //bir onceki projede DbContext kalitim aldik. Bu projede ise Identity'ye ait tablolari kullanabilmek icin IdentityContextten kalitim aldik.
        //IdentityDbContext'in generic yapisini kullandik. Because Appuser araciligiyla microsoft AspNetUser tablosuna kendş şstedigimiz kolonlari ekledik.
        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
        }//ctor

        public DbSet<City> CityTable { get; set; }
        public DbSet<District> DistrictTable { get; set; }
        public DbSet<Neighbourhood> NeighbourhoodTable { get; set; }
        public DbSet<UserAddress> UserAddressTable { get; set; }
        

    }
}
