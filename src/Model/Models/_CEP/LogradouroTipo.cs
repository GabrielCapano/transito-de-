using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._CEP
{
    [Table("LogradouroTipo", Schema = "CEP")]
    public class LogradouroTipo : BaseModel
    {
        [MaxLength(30)]
        public string Description { get; set; }

        [MaxLength(10)]
        public string Abreviattion { get; set; }
    }
}
