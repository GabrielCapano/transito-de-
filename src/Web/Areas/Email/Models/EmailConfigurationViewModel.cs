using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using Md = Model;

namespace Web.Areas.Email.Models
{
    public class EmailConfigurationViewModel : Md.Base.BaseModel
    {
        public EmailConfigurationViewModel()
        {
            ConfigurationTypeList = new List<SelectListItem>
            {
                new SelectListItem { Text = "SMTP", Value = "SMTP", Selected = true },
                new SelectListItem { Text = "SendGridWeb", Value = "SendGridWeb" },
                new SelectListItem { Text = "SendGridSMTP", Value = "SendGridSMTP" }
            };
        }

        [DisplayName("Email do remetente")]
        public string From { get; set; }

        [DisplayName("Nome do remetente")]
        public string FromName { get; set; }

        [DisplayName("Nome da Configuração")]
        public string Name { get; set; }

        [DisplayName("Usuário")]
        public string Login { get; set; }
        
        [DisplayName("Senha")]
        public string Password { get; set; }

        [DisplayName("Porta")]
        public string Port { get; set; }

        [DisplayName("SMTP")]
        public string SMTP { get; set; }

        [DisplayName("Url")]
        public string Url { get; set; }
        
        [DisplayName("Manter esta configuração habilitada")]
        public bool IsHabilited { get; set; }
        
        [DisplayName("Tipo de configuração")]
        public string ConfigurationType { get; set; }

        public IEnumerable<SelectListItem> ConfigurationTypeList { get; set; }
    }

    public enum ConfigurationType
    {
        SendGridWeb,
        SendGridSMTP,
        SMTP
    }

}