using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CET
{
    public class CodigoOcorrencia : BaseModel
    {
        public CodigoOcorrencia()
        {
        }

        public CodigoOcorrencia(string name, TipoOcorrencia tipo)
        {
            Nome = name;
            FkTipoOcorrencia = tipo.Id;
        }

        public String Nome { get; set; }
        public int FkTipoOcorrencia { get; set; }
        [ForeignKey("FkTipoOcorrencia")]
        public TipoOcorrencia TipoOcorrencia { get; set; }

    }
}
