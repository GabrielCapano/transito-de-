using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Base
{
    public static class Extension
    {
        public static double ConvertToRadians(this double angle)
        {
            return (Math.PI / 180) * angle;
        }
    }
}
