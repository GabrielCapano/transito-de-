using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Base;

namespace Model.Models._User
{
    public class AuthLevel : BaseModel
    {
        public AuthLevel()
        {
        }

        public AuthLevel(string name, string label, int levelNumber, params User[] users)
        {
            Name = name;
            LevelNumber = levelNumber;
            Label = label;
            Users = users;
        }

        [MaxLength(200)]
        public String Name { get; set; }

        public string Label { get; set; }

        public int LevelNumber { get; set; }

        [InverseProperty("AuthLevels")]
        public ICollection<User> Users { get; set; }
    }

    public enum EAuthLevel
    {
        DiretorExecutivo = 1,
        GerenteFinanceiro = 2,
        GerenteOperacoes = 4,
    }
}
