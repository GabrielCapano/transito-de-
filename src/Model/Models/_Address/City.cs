using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models._Address
{
    [Table("Cities", Schema = "Address")]
    public class City : BaseModel
    {
        [MaxLength(400, ErrorMessage = "O Nome da cidade deve conter até 400 caracteres."),
         Required(ErrorMessage = "O nome da cidade é requerido.")]
        public String Name { get; set; }

        [Required(ErrorMessage = "O Estado é requerido!")]
        public int FkState { get; set; }

        [ForeignKey("FkState")]
        public State State { get; set; }
    }
}