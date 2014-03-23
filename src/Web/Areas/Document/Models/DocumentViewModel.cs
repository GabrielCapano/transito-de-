using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Md = Model;

namespace Web.Areas.Document.Models
{
    public class DocumentViewModel : Md.Base.BaseModel
    {
        public DocumentViewModel()
        {    
        }

        public DocumentViewModel(string reference)
        {
            Reference = reference;
        }

        public DocumentViewModel(string reference, int referenceId)
        {
            Reference = reference;
            ReferenceId = referenceId;
        }

        [DisplayName("Nome do Arquivo")]
        public String Name { get; set; }


        public int? ReferenceId { get; set; }
        public string Reference { get; set; }
        public String Path { get; set; }

        [DisplayName("Data de Expiração")]
        public DateTime? ExpirationDate { get; set; }

        public String DocumentType { get; set; }

        public String DocumentExtension { get; set; }
        public byte[] FileStream { get; set; }
    }
}