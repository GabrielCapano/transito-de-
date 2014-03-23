using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using Business.Base;
using Repository.Base;
using Models = Model.Models._User;
using Repo = Repository._User;

namespace Business
{
    public class User : BaseBussines<User>
    {
        private User()
        {
        }

        public Models.AuthLevel GetAuthById(int id)
        {
            return Repo.AuthLevel.Instance.GetById(id);
        }

        public static byte[] GetSHA1Token(string email, string password)
        {
            var sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(Encoding.ASCII.GetBytes(email + password));
        }

        public Models.User GetUserByToken(byte[] hashedPassword)
        {
            var user = Repo.User.Instance.GetUserByToken(hashedPassword);
            return user;
        }

        public Models.User GetUserByToken(string hashedPassword)
        {
            var token = Util.StringToByteArray(hashedPassword);
            var user = Repo.User.Instance.GetUserByToken(token);
            return user;
        }

        public Models.User GetActiveUserByToken(byte[] hashedPassword)
        {
            var user = Repo.User.Instance.GetActiveUserByToken(hashedPassword);
            return user;
        }

        public void Logout()
        {
            HttpContext.Current.Session.Abandon();
            HttpContext.Current.Response.Cookies.Clear();
            FormsAuthentication.SignOut();
        }

        public ResponseObject CreateUser(Models.User user, string password)
        {
            ResponseObject response;

            var hash = GetSHA1Token(user.Email, password);
            user.Token = hash;


            if (user.Id == 0 && String.IsNullOrEmpty(user.Password))
            {
                return new ResponseObject(false, "A Senha é requerida");
            }

            if (user.Id == 0)
            {
                user.Score = 0;
                response = Repo.User.Instance.Insert(ref user, Util.GetUserLanguageInformation());
            }
            else
                response = Repo.User.Instance.Update(ref user, Util.GetUserLanguageInformation());

            return response;
        }

        public void GenerateAuthenticationTicket(string id, string roles)
        {
            var authenticationTicket = new FormsAuthenticationTicket(1, id, DateTime.Now, DateTime.Now.AddHours(3),
                false, roles, FormsAuthentication.FormsCookiePath);

            string encryptTicket = FormsAuthentication.Encrypt(authenticationTicket);
            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptTicket);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }



        public Models.User GetLoggedUser(HttpCookie cookie)
        {
            HttpCookie authCookie = cookie;

            if (authCookie == null || authCookie.Value == "")
                return null;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if (authTicket != null)
            {
                return Repo.User.Instance.GetById(Convert.ToInt32(authTicket.Name), "AuthLevels");
            }

            return null;
        }

        public Models.User GetLoggedUser()
        {

            HttpCookie authCookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null || authCookie.Value == "")
                return null;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            if (authTicket != null)
            {
                return Repo.User.Instance.GetById(Convert.ToInt32(authTicket.Name), "AuthLevels");
            }

            return null;
        }

        public Models.User GetByEmail(string email)
        {
            return Repo.User.Instance.GetAll(a => a.Email == email).SingleOrDefault();
        }

        public ResponseObject ForgotPassword(Model.Models._User.User user)
        {
            var token = Util.ByteArrayToString(user.Token);
            return Email.Instance.SendEmail(user.Email,
                "Redefinição de Senha",
                (int)Model.Models._Email.Identification.RedefinirSenha,
                new Dictionary<string, string>
                {
                    {"email", user.Email},
                    {"token", token}
                });
        }

        public ResponseObject CheckLogin(string email, string password, bool onlyUser = false)
        {
            byte[] token = GetSHA1Token(email, password);
            var user = GetActiveUserByToken(token);

            var response = new ResponseObject();
            if (onlyUser)
            {
                if (user != null)
                {
                    response.Object = user;
                    response.Messages.Add(LanguageController.GetValueByKey("Usuário logado com sucesso!", Util.GetUserLanguageInformation()));
                }
                else
                {
                    response.Status = false;
                    response.Messages.Add(LanguageController.GetValueByKey("Usuário não encontrado.", Util.GetUserLanguageInformation()));
                }
                return response;
            }


            if (user != null)
            {
                // Form Authentication
                GenerateAuthenticationTicket(user.Id.ToString(), string.Join(";", user.AuthLevels.Select(a => a.Name).ToArray()));

                if (HttpContext.Current.Session != null)
                    HttpContext.Current.Session.Add("Language", (int)ELanguage.PtBr);

                response.Messages.Add(LanguageController.GetValueByKey("Usuário logado com sucesso!", Util.GetUserLanguageInformation()));
            }
            else
            {
                response.Status = false;
                response.Messages.Add(LanguageController.GetValueByKey("Usuário não encontrado.", Util.GetUserLanguageInformation()));
            }

            return response;
        }

        public List<SelectObject> GetAllActiveUsersToSelect()
        {
            return Repo.User.Instance.GetAllActive().Select(a => new SelectObject(a.Name, a.Id.ToString())).ToList();
        }

        public ResponseObject GetAllActiveUsers(ref PaginationObject pagination, string name)
        {
            Expression<Func<Models.User, bool>> filter;
            if (!String.IsNullOrEmpty(name))
                filter = a => (a.Name.Contains(name) || a.Email.Contains(name));
            else
                filter = a => true;
            var obj = Repo.User.Instance.GetAllActive(ref pagination, filter, a => a.CreatedBy).Select(a => (object)a).ToList();
            var resp = new ResponseObject
                {
                    Objects = obj,
                    Pagination = pagination
                };
            return resp;
        }

        public List<Models.AuthLevel> GetActiveAuthLevel()
        {
            return Repo.AuthLevel.Instance.GetAllActive().ToList();
        }

        public Models.User GetById(int id)
        {
            return Repo.User.Instance.GetById(id, "AuthLevels");
        }

        public static ResponseObject SaveUser(Models.User user)
        {
            var resp = new ResponseObject();

            var originalUser = Repo.User.Instance.GetById(user.Id);

            if (!String.IsNullOrEmpty(user.Password) || user.Id == 0)
            {
                user.Token = GetSHA1Token(user.Email, user.Password);
            }

            if (user.Id != 0)
            {
                user.Email = originalUser.Email;
                if (String.IsNullOrEmpty(user.Password))
                    user.Token = originalUser.Token;
                resp = Repo.User.Instance.Update(ref user, Util.GetUserLanguageInformation());
                Repo.User.Instance.UpdateAuthLevels(ref user);
            }
            else
            {
                var email = user.Email;
                if (Repo.User.Instance.GetAll(a => a.Email == email && !a.IsRemoved).Any())
                {
                    resp.Status = false;
                    resp.Messages.Add(LanguageController.GetValueByKey("O email informado já está sendo utilizado", Util.GetUserLanguageInformation()));
                    return resp;
                }
                var auths = new List<Models.AuthLevel>(user.AuthLevels);
                user.AuthLevels = null;

                resp = Repo.User.Instance.Insert(ref user, Util.GetUserLanguageInformation());
                user.AuthLevels = auths;
                Repo.User.Instance.UpdateAuthLevels(ref user);
            }

            return resp;
        }

        public static ResponseObject Remove(params int[] id)
        {
            return Repo.User.Instance.Remove(Util.GetUserLanguageInformation(), id);
        }

        public List<Models.AuthLevel> GetAuthsNotIn(int[] ids)
        {
            return Repo.AuthLevel.Instance.GetNotIn(ids);
        }

        public bool CheckUserLoginByToken(byte[] token)
        {
            return Repo.User.Instance.GetUserByToken(token) != null;
        }
    }
}

