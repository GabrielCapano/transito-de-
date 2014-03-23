using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CEP
{
    [Table("LogradouroMunicipioCep", Schema = "CEP")]
    public class LogradouroMunicipioCep : BaseModel
    {
        public int FkLogradouroMunicipio { get; set; }
        [ForeignKey("FkLogradouroMunicipio")]
        public LogradouroMunicipio LogradouroMunicipio { get; set; }

        public int InitialCEP { get; set; }
        public int EndCEP { get; set; }
    }
}
