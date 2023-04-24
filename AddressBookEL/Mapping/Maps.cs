using AddressBookEL.Models;
using AddressBookEL.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.Mapping
{
    public class Maps:Profile
    {
        public Maps()
        {
            //hangi dto'nun hangi modele donusecegini ayarliyoruz.
            CreateMap<City, CityVM>().ReverseMap();
            CreateMap<District, DistrictVM>().ReverseMap();
            CreateMap<Neighbourhood, NeighbourhoodVM>().ReverseMap();
            CreateMap<UserAddress, UserAddressVM>().ReverseMap();
        }
    }
}
