using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using Model.Base;
using Newtonsoft.Json;

namespace Model.Models._User
{
    public class User : BaseModel
    {
        public User()
        {
        }

        public User(string name, string password, string email, params AuthLevel[] levels)
        {
            Name = name;
            Email = email;
            var sha = new SHA1CryptoServiceProvider();
            Token = sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(email + password));
            if (levels.Length > 0)
                AuthLevels = levels;
        }

        [DisplayName("Nome"), Required(ErrorMessage = "O campo nome é requerido"),
        MaxLength(100, ErrorMessage = "O nome não deve exceder 100 caracteres.")]
        public string Name { get; set; }

        [DisplayName("Email"), Required(ErrorMessage = "O campo email é requerido"),
        DataType(DataType.EmailAddress, ErrorMessage = "Email inválido."), MaxLength(250, ErrorMessage = "O email não deve exceder 250 caracteres.")]
        public string Email { get; set; }

        [DisplayName("Telefone"), MaxLength(20, ErrorMessage = "O Telefone não deve ter mais do que 20 caracteres.")]
        public string Phone { get; set; }

        public byte[] Token { get; set; }

        public int? Score { get; set; }

        [InverseProperty("Users"), JsonIgnore]
        public ICollection<AuthLevel> AuthLevels { get; set; }

        [NotMapped]
        public string Password { get; set; }


    }


}