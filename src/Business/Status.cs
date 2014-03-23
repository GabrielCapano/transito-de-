using Business.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repo = Repository._CET;

namespace Business
{
    public class Status : BaseBussines<Status>
    {
        private Status()
        {

        }

        public List<SelectObject> GetAllActiveToSelect()
        {
            return Repo.Status.Instance.GetAllActive().Select(a => new SelectObject(a.Description.ToString(), a.Id.ToString())).ToList();
        }
    }
}
