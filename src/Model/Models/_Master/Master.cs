using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Model.Base;

namespace Model.Models._Master
{
    public class Master : BaseModel
    {
        [Required, MaxLength(100, ErrorMessage = "O nome é requerido"), DisplayName("Nome")]
        public string Name { get; set; }
    }
}