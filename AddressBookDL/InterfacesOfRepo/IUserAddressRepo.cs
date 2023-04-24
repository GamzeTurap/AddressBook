using AddressBookEL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookDL.InterfacesOfRepo
{
    public interface IUserAddressRepo:IRepository<UserAddress,int>
    {
    }
}
