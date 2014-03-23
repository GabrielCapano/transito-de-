using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CET
{
    public class TipoOcorrencia : BaseModel
    {
        public TipoOcorrencia()
        {
        }

        public TipoOcorrencia(string name, string descricao, string imagem, string cor)
        {
            Nome = name;
            Descricao = descricao;
            Imagem = imagem;
            Cor = cor;
        }


        [DisplayName("Tipo da Ocorrência")]
        public String Nome { get; set; }

        public String Descricao { get; set; }
        public String Imagem { get; set; }
        public String Cor { get; set; }
    }
}
