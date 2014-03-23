using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Model.Base;

namespace Model.Models._Address
{
    [Table("Addresses", Schema = "Address")]
    public class Address : BaseModel
    {
        [Required(ErrorMessage = "O logradouro do endereço é requerida."),
         MaxLength(400, ErrorMessage = "O logradouro não deve exceder 400 caracteres.")]
        public String Street { get; set; }

        [Required(ErrorMessage = "O CEP do endereço é requerido."),
         MaxLength(10, ErrorMessage = "O CEP do endereço não deve exceder 10 caracteres.")]
        public String CEP { get; set; }

        public int? FkDistrict { get; set; }
        [ForeignKey("FkDistrict")]
        public District District { get; set; }


        public int FkCity { get; set; }
        [ForeignKey("FkCity")]
        public City City { get; set; }

        [Required(ErrorMessage = "O número do endereço é requerido.")]
        public int Number { get; set; }

        [MaxLength(400, ErrorMessage = "O complemento do endereço não deve exceder 400 caracteres.")]
        public String Complement { get; set; }
    }
}