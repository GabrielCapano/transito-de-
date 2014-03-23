using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Repository.Base;
using BO = Business;
using Models = Model.Models._User;

namespace Service.Controllers
{
    /// <summary>
    /// Usuários, login e cadastro.
    /// </summary>
    public class UserController : ApiController
    {
        /// <summary>
        /// Fazer o login do usuário (verificar se vai funcionar no WS, pois ele usa cookie..)
        /// </summary>
        /// <param name="name">Username (email)</param>
        /// <param name="password">Senha</param>
        /// <returns>Um objeto de resposta padrão</returns>
        public ResponseObject GetLogin([FromUri]string name, [FromUri]string password)
        {
            return BO.User.Instance.CheckLogin(name, password, true);
        }

        /// <summary>
        /// Criar novo usuário
        /// </summary>
        /// <param name="model">Usuário</param>
        /// <returns>Objeto de resposta padrão.</returns>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public ResponseObject PostUser([FromBody]Models.User model)
        {
            return BO.User.Instance.CreateUser(model, model.Password);
        }

    }
}