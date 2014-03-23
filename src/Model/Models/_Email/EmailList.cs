using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._Email
{
    public class EmailList : BaseModel
    {
        public EmailList()
        {
        }

        public EmailList(string body, string to, string title, int fkTemplate, bool sended)
        {
            Body = body;
            To = to;
            Title = title;
            FkTemplate = fkTemplate;
            IsSended = sended;
        }

        public String Body { get; set; }
        public String To { get; set; }
        public String Title { get; set; }

        public int FkTemplate { get; set; }
        [ForeignKey("FkTemplate")]
        public Template Template { get; set; }

        public bool IsSended { get; set; }
    
    }
}
