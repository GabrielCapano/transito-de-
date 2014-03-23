using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._Language
{
    public class LanguageKey : BaseModel 
    {
        public LanguageKey()
        {
        }

        public LanguageKey(string key, string value, Language lang)
        {
            Key = key;
            Value = value;
            Language = lang;
            FkLanguage = lang.Id;
        }

        public String Key { get; set; }
        public String Value { get; set; }

        public int FkLanguage { get; set; }
        [ForeignKey("FkLanguage")]
        public Language Language { get; set; }
    }
}
