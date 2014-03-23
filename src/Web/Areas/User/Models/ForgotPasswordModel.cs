using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.User.Models
{
    public class ForgotPasswordModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "O campo senha é requerido."), DisplayName("Nova senha"), DataType(DataType.Password), Compare("ConfirmPassword", ErrorMessage = "As senhas não coincidem.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "O campo de confirmação da senha é requerido."), DisplayName("Confirmar senha"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}