using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using Business.Base;
using Repository.Base;
using Models = Model.Models._CET;
using BO = Business;

namespace Service.Controllers
{
    /// <summary>
    /// Controle de Ocorrências
    /// </summary>
    public class OcorrenciaController : ApiController
    {

        /// <summary>
        /// Lista todas as ocorrências
        /// </summary>
        /// <returns>Lista de ocorrências</returns>
        public List<Models.Ocorrencia> Get()
        {
            return BO.Ocorrencia.Instance.GetAllOcorrencias();
        }

        /// <summary>
        /// Lista as ocorrências do usuário em específico
        /// </summary>
        /// <param name="userId">ID do usuário</param>
        /// <returns>Lista de ocorrências</returns>
        public List<Models.Ocorrencia> Get(int userId)
        {
            return BO.Ocorrencia.Instance.GetAllOcorrencias(userId);
        }

        /// <summary>
        /// Insere uma nova Ocorrência
        /// </summary>
        /// <param name="model">Entidade da ocorrência</param>
        /// <param name="token">Token do usuário</param>
        /// <returns>
        /// Retorna um objeto de resposta padrão
        /// </returns>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseObject Post([FromBody]Models.Ocorrencia model, [FromUri]string token)
        {
            if (BO.User.Instance.CheckUserLoginByToken(Convert.FromBase64String(token)))
            {
                return BO.Ocorrencia.Instance.SaveOcorrencia(model);
            }

            return new ResponseObject(false, "Usuário inexistente");

        }

        /// <summary>
        /// Deleta uma ocorrência (Soft delete)
        /// </summary>
        /// <param name="id">id da ocorrência</param>
        /// <param name="token">Token do usuário</param>
        /// <returns>
        /// Retorna um objeto de resposta padrão
        /// </returns>
        public ResponseObject Delete([FromUri]int id, [FromUri]string token)
        {
            if (BO.User.Instance.CheckUserLoginByToken(Convert.FromBase64String(token)))
            {
                return BO.Ocorrencia.Instance.DeleteOcorrencia(id, token);
            }

            return new ResponseObject(false, "Usuário inexistente");

        }


        /// <summary>
        /// Atualiza uma Ocorrência
        /// </summary>
        /// <param name="model">Entidade da ocorrência (com o id)</param>
        /// <param name="token">Token do usuário</param>
        /// <returns>
        /// Retorna um objeto de resposta padrão
        /// </returns>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseObject Put([FromBody]Models.Ocorrencia model, [FromUri]string token)
        {
            if (BO.User.Instance.CheckUserLoginByToken(Convert.FromBase64String(token)))
                return BO.Ocorrencia.Instance.SaveOcorrencia(model);

            return new ResponseObject(false, "Usuário inexistente");

        }
    }
}