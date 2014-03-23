using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using UserModel = Model.Models._User;

namespace Web.Areas.User.Models
{
    public class LoginViewModel : UserModel.User
    {
        [DisplayName("Password"), Required(ErrorMessage = "O campo senha é requerido"),
        DataType(DataType.Password, ErrorMessage = "Senha inválida."),
        MaxLength(15, ErrorMessage = "A senha não deve exceder 15 caracteres.")]
        public string Password { get; set; }
    }
}