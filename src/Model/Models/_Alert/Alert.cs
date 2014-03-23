using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;
using Model.Models._User;

namespace Model.Models._Alert
{
    public class Alert : BaseModel
    {
        public Alert()
        {
        }

        public Alert(string title, string message, AlertType alertType)
        {
            Message = message;
            Title = title;
            AlertType = alertType;
        }
        public bool IsSended { get; set; }

        [MaxLength(50)]
        public String Title { get; set; }

        public DateTime? StartDate { get; set; }

        [MaxLength(300)]
        public String Message { get; set; }

        public AlertType AlertType { get; set; }

        public int? FkUser { get; set; }
        [ForeignKey("FkUser")] 
        public User User { get; set; }

        public int? FkAuthLevel { get; set; }
        [ForeignKey("FkAuthLevel")]
        public AuthLevel AuthLevel { get; set; }

        public bool IsClosed { get; set; }
    }

    public enum AlertType 
    {
        Warning,
        Error,
        Info,
        Message,
        Deadline,
        Birthday,
    }
}
