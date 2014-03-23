using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models._Address
{
    [Table("States", Schema = "Address")]
    public class State : BaseModel
    {
        [MaxLength(400, ErrorMessage = "O nome do estado deve conter até 400 caracteres."),
         Required(ErrorMessage = "O nome do estado é requerido.")]
        public String Name { get; set; }

        [MaxLength(3, ErrorMessage = "A sigla do estado deve conter até 3 caracteres."),
         Required(ErrorMessage = "A sigla do estado é requerida.")]
        public String Acronym { get; set; }

        public int FkCountry { get; set; }

        [ForeignKey("FkCountry")]
        public Country Country { get; set; }
    }
}