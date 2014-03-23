using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Repository.Base;
using BO = Business;
using Models = Model.Models._CET;


namespace Service.Controllers
{   
    /// <summary>
    /// Controle de código de ocorrencias
    /// </summary>
    public class CodigoOcorrenciaController : ApiController
    {
        /// <summary>
        /// Pega os Códigos de ocorrência disponíveis para o tipo de ocorrência
        /// </summary>
        /// <param name="IdTipoOcorrencia">Chave primária do tipo de ocorrência</param>
        /// <returns>Lista de Códigos de Ocorrência</returns>
        [ActionName("Default")]
        public List<Models.CodigoOcorrencia> Get([FromUri]int IdTipoOcorrencia)
        {
            return BO.Ocorrencia.Instance.GetCodigoOcorrenciaByTipo(IdTipoOcorrencia);
        }
    }
}