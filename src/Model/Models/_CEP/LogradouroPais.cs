using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CEP
{
    [Table("LogradouroPais", Schema = "CEP")]
    public class LogradouroPais : BaseModel
    {
        [MaxLength(30)]
        public string Description { get; set; }


        public int AreaCode { get; set; }
    }
}
