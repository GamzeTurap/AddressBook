using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookBL.EmailSenderBusiness
{
    public class EmailMessage
    {
        public string[] To { get; set; } //kime
        public string Subject { get; set; } //konu
        public string Body { get; set; } //içerik
        public string[] CC { get; set; }//Bilgilendirmek istedigimiz kisi
        public string[] BCC { get; set; }//Gizlice bilgilendirmek istedigimiz kisi

    }
}
