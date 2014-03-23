using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Md = Model;
using BO = Business;

namespace Web.Areas.User.Models
{
    public class UserViewModel : Md.Base.BaseModel
    {
        public UserViewModel()
        {
            AuthLevels = BO.User.Instance.GetActiveAuthLevel().Select(a => new AuthLevelViewModel{Name = a.Name, Id = a.Id}).ToList();
        }

        [DisplayName("Nome"), Required(ErrorMessage = "O campo nome é requerido"),
        MaxLength(100, ErrorMessage = "O nome não deve exceder 100 caracteres.")]
        public string Name { get; set; }

        [DisplayName("Email"), Required(ErrorMessage = "O campo email é requerido"),
        EmailAddress(ErrorMessage = "Email inválido."), StringLength(250, ErrorMessage = "O email não deve exceder 250 caracteres.")]
        public string Email { get; set; }

        [DisplayName("Senha"),DataType(DataType.Password), Compare("ConfirmPassword", ErrorMessage = "A Senha deve ser igual a confirmação de senha.")]
        public string Password { get; set; }

        [DisplayName("Confirmação de Senha"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        [DisplayName("Telefone"), MaxLength(20, ErrorMessage = "O Telefone não deve ter mais do que 20 caracteres.")]
        public string Phone { get; set; }

        [InverseProperty("Users")]
        public List<AuthLevelViewModel> AuthLevels { get; set; }
    }

    public class AuthLevelViewModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
    }
}