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
    [Table("LogradouroBairro", Schema = "CEP")]
    public class LogradouroBairro : BaseModel
    {
        public int FkLogradouroMunicipio { get; set; }
        [ForeignKey("FkLogradouroMunicipio")]
        public LogradouroMunicipio LogradouroMunicipio { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }
    }
}
