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
    [Table("Logradouro", Schema = "CEP")]
    public class Logradouro : BaseModel
    {
        [MaxLength(100)]
        public string Description { get; set; }

        public int CEP { get; set; }

        public int InitialNumber { get; set; }
        public int FinalNumber { get; set; }
        public bool? EvenOrOdd { get; set; }

        [MaxLength(20)]
        public string CodigoDNE { get; set; }
        
        [MaxLength(9)]
        public string CEPPresentation { get; set; }

        [MaxLength(8)]
        public string CEPText { get; set; }

        public int? FkLogradouroTipo { get; set; }
        [ForeignKey("FkLogradouroTipo")]
        public LogradouroTipo LogradouroTipo { get; set; }

        public int? FkLogradouroPrefixo { get; set; }
        [ForeignKey("FkLogradouroPrefixo")]
        public LogradouroPrefixo LogradouroPrefixo { get; set; }

        public int? FkLogradouroBairro { get; set; }
        [ForeignKey("FkLogradouroBairro")]
        public LogradouroBairro LogradouroBairro { get; set; }
    }
}
