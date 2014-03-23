using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;
using Model.Base;
using Web.Extensions;

namespace Web.Areas.Ocorrencias.Models
{
    public class OcorrenciaViewModel : BaseModel
    {
        public OcorrenciaViewModel()
        {
            StatusList = Business.Status.Instance.GetAllActiveToSelect().Select(a => a.ConvertToSelectList());
        }

        public String Local { get; set; }

        public int Numero { get; set; }

        public String Sentido { get; set; }

        public String Pista { get; set; }

        public String Faixa { get; set; }
        
        [DisplayName("Descrição")]
        public String Descricao { get; set; }

        public String Resumo { get; set; }

        [DisplayName("Resposta"), Required(ErrorMessage = "O campo reposta é requerido")]
        public String RespostaPrestador { get; set; }

        [DisplayName("Protocolo"), Required(ErrorMessage = "O campo protocolo é requerido")]
        public String Protocolo { get; set; }

        [DisplayName("Ínicio da Ocorrência")]
        public DateTime InicioOcorrencia { get; set; }
        [DisplayName("Total de Faixas")]
        public int TotalFaixas { get; set; }

        public int FkUser { get; set; }

        public Model.Models._User.User User { get; set; }

        [DisplayName("Status"), Required(ErrorMessage = "O campo de Status é requerido")]
        public int? FkStatus { get; set; }

        public Model.Models._CET.Status Status { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public double? Rate { get; set; }

        public String CodigoOcorrenciaNome { get; set; }

        public int? FkPhotoDocument { get; set; }

        public Model.Models._Document.Document PhotoDocument { get; set; }

        public int FkCodigoOcorrencia { get; set; }

        public Model.Models._CET.CodigoOcorrencia CodigoOcorrencia { get; set; }

        public IEnumerable<SelectListItem> StatusList { get; set; }
    }
}