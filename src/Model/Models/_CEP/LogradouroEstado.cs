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
    [Table("LogradouroEstado", Schema = "CEP")]
    public class LogradouroEstado : BaseModel
    {
        [MaxLength(30)]
        public string Description { get; set; }
        
        [MaxLength(2)]
        public string Acronym { get; set; }

        public int FkLogradouroPais { get; set; }
        [ForeignKey("FkLogradouroPais")]
        public LogradouroPais LogradouroPais { get; set; }
    }
}
