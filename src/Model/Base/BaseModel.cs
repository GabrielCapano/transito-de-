using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Model.Base
{
    public class BaseModel
    {
        public BaseModel()
        {
            CreatedBy = 0;
            LastUpdated = DateTime.Now;
            CreatedDate = DateTime.Now;
            UpdatedBy = 0;
            IsRemoved = false;
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [ScaffoldColumn(false)]
        public int Id { get; set; }

        [DisplayName("Data de Criação")]
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Data da Última Edição")]
        [ScaffoldColumn(false)]
        public DateTime LastUpdated { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Atualizado Por")]
        public int UpdatedBy { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("Criado Por")]
        public int CreatedBy { get; set; }

        [ScaffoldColumn(false)]
        public bool IsRemoved { get; set; }
    }

}