using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._Address
{
    [Table("Districts", Schema = "Address")]
    public class District : BaseModel
    {
        public District()
        {
        }

        public District(string name)
        {
            Name = name;
        }
        [Required(ErrorMessage = "O bairro do endereço é requerido."),
         MaxLength(400, ErrorMessage = "O bairro do endereço não deve exceder 400 caracteres.")]
        public String Name { get; set; }

        public ICollection<Address> Addresses { get; set; }

    }
}
