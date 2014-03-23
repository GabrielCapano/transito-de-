using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Model.Base;

namespace Model.Models._Document
{
    public class Document : BaseModel
    {
        [DisplayName("Nome do Arquivo")]
        public String Name { get; set; }
        [DisplayName("Caminho"), Required(ErrorMessage = "O caminho do arquivo é requerido!")]
        public String Path { get; set; }

        public byte[] FileStream { get; set; }
        [DisplayName("Data de Expiração")]
        public DateTime? ExpirationDate { get; set; }
        [Required(ErrorMessage = "O tipo de documento é requerido.")]
        public String DocumentType { get; set; }
        [Required(ErrorMessage = "A extensão do documento é requerida."), MaxLength(4)]
        public String DocumentExtension { get; set; }
    }
}