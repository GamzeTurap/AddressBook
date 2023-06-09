﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookEL.ViewModels
{
    public class PostalCodeApiVM
    {
        //https://api.ubilisim.com/postakodu/il/34
        //yukaridaki apiden donen json icindeki "postakodu" arrayi bu class ile karsilayabiliriz.

        //  "postakodu": [
        //{
        //    "plaka": 34,
        //    "il": "İSTANBUL",
        //    "ilce": "ADALAR",
        //    "semt_bucak_belde": "BURGAZADA",
        //    "mahalle": "BURGAZADA MAH",
        //    "pk": "34975"
        //}

        public int plaka { get; set; }
        public string il { get; set; }
        public string ilce { get; set; }
        public string semt_bucak_belde { get; set; }
        public string mahalle { get; set; }
        public string pk { get; set; }


    }
}
