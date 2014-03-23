using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CET
{
    [Table("Lentidoes")]
    public class Lentidao : BaseModel
    {
        public DateTime? DataHora { get; set; }
        public int? IdCorredor { get; set; }
        public string Corredor { get; set; }
        public string Sentido { get; set; }
        public string Pista { get; set; }
        public int? ExtensaoPista { get; set; }
        public string InicioLentidao { get; set; }
        public string TerminoLentidao { get; set; }
        public int? ReferenciaNumericaInicioLentidao { get; set; }
        public int? ExensaoLentidao { get; set; }
        public string Regiao { get; set; }
    }
}
