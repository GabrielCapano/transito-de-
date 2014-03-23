using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Web;
using Business.Base;
using Repository.Base;
using SendGridMail;
using SendGridMail.Transport;
using Repo = Repository._Email;
using Models = Model.Models._Email;

namespace Business
{
    public class Email : BaseBussines<Email>
    {
        private static Models.EmailConfiguration _emailConfiguration;

        private Email()
        {
        }

        public Model.Models._Email.EmailConfiguration GetById(int id)
        {
            var model = Repository._Email.EmailConfiguration.Instance.GetById(id);

            return model;
        }

        public Models.EmailConfiguration GetCurrentConfiguration()
        {
            var model = Repository._Email.EmailConfiguration.Instance.GetAll(e => e.IsHabilited).SingleOrDefault() ??
                        new Models.EmailConfiguration();

            return model;
        }


        public ResponseObject SaveEmailConfiguration(Models.EmailConfiguration model)
        {
            ResponseObject resp;

            if (model.Id != 0)
            {
                resp = Repo.EmailConfiguration.Instance.Update(ref model, Util.GetUserLanguageInformation());
            }
            else
            {
                resp = Repo.EmailConfiguration.Instance.Insert(ref model, Util.GetUserLanguageInformation());
            }

            if (model.IsHabilited)
            {
                var email = Repository._Email.EmailConfiguration.Instance.GetAll(e => e.IsHabilited && e.Id != model.Id).SingleOrDefault();

                if (email != null)
                {
                    email.IsHabilited = false;
                    Repo.EmailConfiguration.Instance.Update(ref email, Util.GetUserLanguageInformation());
                }
            }

            return resp;
        }

        public ResponseObject GetAllEmailConfiguration(ref PaginationObject pagination, string name)
        {
            Expression<Func<Models.EmailConfiguration, bool>> filter;
            if (!String.IsNullOrEmpty(name))
                filter = a => !a.IsRemoved && (a.From.Contains(name) || a.FromName.Contains(name));
            else
                filter = a => !a.IsRemoved;

            var obj = Repo.EmailConfiguration.Instance.GetAllActive(ref pagination, filter, a => a.CreatedBy).Select(a => (object)a).ToList();
            var resp = new ResponseObject
            {
                Objects = obj,
                Pagination = pagination
            };

            return resp;
        }

        public static ResponseObject RemoveEmailConfiguration(params int[] id)
        {
            return Repository._Email.EmailConfiguration.Instance.Remove(Util.GetUserLanguageInformation(), id);
        }

        public ResponseObject SendEmail(string to, string title, int templateIdentification,
            Dictionary<String, String> values)
        {
            var template = Repo.Template.Instance.GetAll(a => !a.IsRemoved && a.IdentificationId == templateIdentification).First();
            var body = template.Body;
            var request = HttpContext.Current.Request;
            string baseUrl = request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/";
            values.Add("urlBase", baseUrl);

            body = values.Aggregate(body, (current, value) => current.Replace("{" + value.Key + "}", value.Value));

            var email = new Models.EmailList(body, to, title, template.Id, true);

            Repo.EmailList.Instance.Insert(ref email, Util.GetUserLanguageInformation());

            return SendEmail(email);
        }

        private ResponseObject SendEmail(Models.EmailList email)
        {
            if (_emailConfiguration == null)
            {
                _emailConfiguration = Repo.EmailConfiguration.Instance.GetAll(a => a.IsHabilited && !a.IsRemoved).First();
            }
            var response = new ResponseObject();
            try
            {
                var credentials = new NetworkCredential(_emailConfiguration.Login, _emailConfiguration.Password);
                var from = new MailAddress(_emailConfiguration.From, _emailConfiguration.FromName);

                if (_emailConfiguration.ConfigurationType == Models.ConfigurationType.SendGridWeb ||
                    _emailConfiguration.ConfigurationType == Models.ConfigurationType.SendGridSMTP)
                {

                    SendGrid message =
                        SendGrid.GetInstance();
                    message.AddTo(email.To);
                    message.From = from;
                    message.Subject = email.Title;

                    if (_emailConfiguration.ConfigurationType == Models.ConfigurationType.SendGridWeb)
                    {
                        var transport = Web.GetInstance(credentials);

                        transport.Deliver(message);
                    }
                    else
                    {
                        var transport = SMTP.GetInstance(credentials);

                        transport.Deliver(message);
                    }

                }
                else
                {
                    var smtpServer = new SmtpClient
                    {
                        Host = _emailConfiguration.SMTP,
                        Port = Convert.ToInt32(_emailConfiguration.Port),
                        Credentials = credentials,
                        UseDefaultCredentials = false,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        EnableSsl = true
                    };
                    smtpServer.Credentials = credentials;

                    var msg = new MailMessage();
                    msg.Body = email.Body;
                    msg.IsBodyHtml = true;
                    msg.From = from;
                    msg.To.Add(email.To);
                    msg.Subject = email.Title;

                    smtpServer.Send(msg);

                }
                response.Messages.Add(LanguageController.GetValueByKey("Mensagem enviada com sucesso!", Util.GetUserLanguageInformation()));
                response.Status = true;

                return response;
            }
            catch (Exception exception)
            {
                response.Status = false;
                response.Messages.Add(LanguageController.GetValueByKey("Ocorreu um erro ao enviar a mensagem, verifique as configurações de envio.", Util.GetUserLanguageInformation()));
                return response;
            }

        }
    }
}
