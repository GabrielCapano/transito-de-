using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CEP
{
    [Table("LogradouroMunicipio", Schema = "CEP")]
    public class LogradouroMunicipio : BaseModel
    {
        [MaxLength(30)]
        public string Description { get; set; }

        public int FkLogradouroEstado { get; set; }
        [ForeignKey("FkLogradouroEstado")]
        public LogradouroEstado LogradouroEstado { get; set; }

        public int? AreaCode { get; set; }
    }
}
