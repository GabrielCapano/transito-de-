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
    /// Tipos de ocorrência
    /// </summary>
    public class TipoOcorrenciaController : ApiController
    {
        /// <summary>
        /// Pega os tipos de ocorrência
        /// </summary>
        /// <returns>
        /// Listagem de tipos de Ocorrência
        /// </returns>
        public IEnumerable<Models.TipoOcorrencia> Get()
        {
            return BO.Ocorrencia.Instance.GetTipoOcorrenciasList();
        }
    }
}