using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Business.Base;
using Repository.Base;
using MD = Model.Models;
using Repo = Repository;

namespace Business
{
    public class Log : BaseBussines<Log>
    {
        private Log()
        {
        }

        public ResponseObject GetAllLogs(ref PaginationObject pagination, string name)
        {
            Expression<Func<MD._Log.TableLog, bool>> filter;
            if (!String.IsNullOrEmpty(name))
                filter = a => !a.IsRemoved && (a.Level.Contains(name) || a.Logger.Contains(name) || a.Message.Contains(name));
            else
                filter = a => !a.IsRemoved;
            var obj = Repo._Log.TableLog.Instance.GetAllActive(ref pagination, filter, a=>a.CreatedBy).Select(a => (object)a).ToList();
            var resp = new ResponseObject
            {
                Objects = obj,
                Pagination = pagination
            };
            return resp;
        }
    }
}
