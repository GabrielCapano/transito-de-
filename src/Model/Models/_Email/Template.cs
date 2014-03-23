using System;
using System.Collections.Generic;
using Model.Base;

namespace Model.Models._Email
{
    public class Template : BaseModel
    {
        public String Name { get; set; }
        public String Body { get; set; }

        public ICollection<EmailList> Emails { get; set; }
        public int IdentificationId { get; set; }
    }

    public enum Identification
    {
        ConvidarClienteRota = 1,
        RedefinirSenha = 2
    }
}
