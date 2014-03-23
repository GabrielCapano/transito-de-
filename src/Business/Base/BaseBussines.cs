using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Base;

namespace Business.Base
{
    public abstract class BaseBussines<TBusiness> : Singleton<TBusiness>
        where TBusiness : class
    {
    }
}
