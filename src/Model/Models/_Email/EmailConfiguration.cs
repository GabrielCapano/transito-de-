using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._Email
{
    public class EmailConfiguration : BaseModel
    {
        public string From { get; set; }
        public string Name { get; set; }
        
        public string Login { get; set; }
        
        public string Password { get; set; }
        
        public string Port { get; set; }
        
        public string SMTP { get; set; }

        public string Url { get; set; }

        public ConfigurationType ConfigurationType { get; set; }

        public bool IsHabilited { get; set; }

        public string FromName { get; set; }
    }


    public enum ConfigurationType
    {
        SendGridWeb,
        SendGridSMTP,
        SMTP
    }
}
