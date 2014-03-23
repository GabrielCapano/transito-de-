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
    [Table("LogradouroPrefixo", Schema = "CEP")]
    public class LogradouroPrefixo : BaseModel
    {
        [MaxLength(20)]
        public string Description { get; set; }

        [MaxLength(10)]
        public string Prefix { get; set; }

        public decimal NumberFactor { get; set; }
    }
}
