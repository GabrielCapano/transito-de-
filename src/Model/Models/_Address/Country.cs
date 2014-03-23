using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models._Address
{
    [Table("Countries", Schema = "Address")]
    public class Country : BaseModel
    {
        [Required(ErrorMessage = "O Nome do país é requerido!"),
         MaxLength(400, ErrorMessage = "O Nome do país deve conter até 400 caracteres.")]
        public String Name { get; set; }

        [MaxLength(3, ErrorMessage = "A sigla do pais deve conter até 3 caracteres."),
         Required(ErrorMessage = "A sigla do pais é requerida.")]
        public String Acronym { get; set; }
    }
}