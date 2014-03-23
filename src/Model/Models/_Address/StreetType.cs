using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models._Address
{
    [Table("StreetTypes", Schema = "Address")]
    public class StreetType : BaseModel
    {
        [Required(ErrorMessage = "A sigla do tipo de logradouro é requerido."),
         MaxLength(3, ErrorMessage = "A sigla não deve exceder 3 caracteres.")]
        public String Acronym { get; set; }

        [Required(ErrorMessage = "O nome é requerido."),
         MaxLength(400, ErrorMessage = "O nome não deve exceder 400 caracteres.")]
        public String Name { get; set; }
    }
}