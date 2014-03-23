using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._Language
{
    public class Language : BaseModel 
    {
        public Language()
        {
        }

        public Language(string name, string abreviattion)
        {
            Name = name;
            Abreviattion = abreviattion;
        }

        [Required, MaxLength(100)]
        public String Name { get; set; }
        [Required, MaxLength(6)]
        public String Abreviattion { get; set; }

        public ICollection<LanguageKey> LanguageKeys { get; set; }
    }
}
