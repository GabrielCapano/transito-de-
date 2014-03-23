using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CET
{
	public class LentidaoConsolidado : BaseModel
	{
        public string TerminoLentidao { get; set; }
        public string InicioLentidao { get; set; }
		public int? ReferenciaNumericaInicioLentidao { get; set; }
        public string Regiao { get; set; }
		public int Total { get; set; }

		public ICollection<Step> Steps { get; set; } 
	}
}
