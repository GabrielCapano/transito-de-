using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;
using Model.Models._Document;
using Model.Models._User;
using Newtonsoft.Json;
using System.Data.Entity.Spatial;

namespace Model.Models._CET
{
    public class Ocorrencia : BaseModel
    {
        public String Local { get; set; }
        public int Numero { get; set; }
        public String Sentido { get; set; }
        public String Pista { get; set; }
        public String Faixa { get; set; }

        public String Descricao { get; set; }
        public String Resumo { get; set; }
        public String RespostaPrestador { get; set; }
        public String Protocolo { get; set; }

        public DateTime InicioOcorrencia { get; set; }

        public int TotalFaixas { get; set; }

        public int FkUser { get; set; }
        [ForeignKey("FkUser"), JsonIgnore]
        public User User { get; set; }

        public int? FkStatus { get; set; }
        [ForeignKey("FkStatus"), JsonIgnore]
        public Status Status { get; set; }
        
        [NotMapped]
        public double? Latitude { get; set; }
        [NotMapped]
        public double? Longitude { get; set; }
       
        [JsonIgnore]
        public DbGeography PosicaoGeografica { get; set; }
        public double? Rate { get; set; }

        public int? FkPhotoDocument { get; set; }
        [ForeignKey("FkPhotoDocument")]
        public Document PhotoDocument { get; set; }

        public int FkCodigoOcorrencia { get; set; }
        [ForeignKey("FkCodigoOcorrencia"), JsonIgnore]
        public CodigoOcorrencia CodigoOcorrencia { get; set; }

        [NotMapped]
        public String TipoOcorrenciaCor
        {
            get
            {
                if (CodigoOcorrencia != null && CodigoOcorrencia.TipoOcorrencia != null)
                    return CodigoOcorrencia.TipoOcorrencia.Cor;
                
                return "";
            }
        }
    }
}
