using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CET
{
    public class Step : BaseModel
    {
        public DbGeography PosicaoGeografica { get; set; }
        public Int32 FkLentidaoConsolidado { get; set; }
        
        [ForeignKey("FkLentidaoConsolidado")]
        public virtual LentidaoConsolidado LentidaoConsolidado { get; set; }
    }
}
