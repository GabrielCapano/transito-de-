using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Xml.Linq;
using Business.Base;
using Repository.Base;
using Utilities.DataTypes.ExtensionMethods;
using Repo = Repository._Alert;

namespace Business
{
    public class Alert : BaseBussines<Alert>
    {
        private Alert()
        {
        }



        public ResponseObject NewAlert(Model.Models._Alert.Alert alert)
        {
            return Repo.Alert.Instance.Insert(ref alert, Util.GetUserLanguageInformation());
        }

        public List<Model.Models._Alert.Alert> GetAllActiveAlertsForUser(HttpCookie cookie)
        {
            var user = User.Instance.GetLoggedUser(cookie);
            return Repo.Alert.Instance.GetAll(a => !a.IsRemoved && !a.IsSended && 
                ((a.FkUser == user.Id || user.AuthLevels.Select(b=>b.Id).Contains(a.FkAuthLevel.Value)) || (a.FkUser == null && a.FkAuthLevel == null))).ToList();
        }

        public void SetAlertsAsSeen(List<Model.Models._Alert.Alert> alerts, HttpCookie cookie)
        {
            foreach (var alert in alerts)
            {
                alert.IsSended = true;
            }

            Repo.Alert.Instance.Update(ref alerts, Util.GetUserLanguageInformation(cookie));
        }

        public IEnumerable<Model.Models._Alert.Alert> GetAllActiveAlertsForUser()
        {
            var user = User.Instance.GetLoggedUser();
            return Repo.Alert.Instance.GetAll(a => !a.IsRemoved && !a.IsSended &&
                ((a.FkUser == user.Id || user.AuthLevels.Select(b => b.Id).Contains(a.FkAuthLevel.Value)) || (a.FkUser == null && a.FkAuthLevel == null)), "AuthLevel").ToList();
        }

        public IEnumerable<Model.Models._Alert.Alert> GetAllActiveAlertsForUser(int userId)
        {
            var user = User.Instance.GetById(userId);
            var alerts = Repo.Alert.Instance.GetAll(a => !a.IsRemoved 
                && a.StartDate <= DateTime.Now
                && !a.IsClosed, "AuthLevel")
                .Where(a => ((a.FkUser == user.Id 
                    || user.AuthLevels.Select(b => b.Id).Contains((a.FkAuthLevel != null  ? a.FkAuthLevel.Value : 0)) 
                    || (a.FkUser == null && a.FkAuthLevel == null)))).ToList();

            foreach (var alert in alerts)
            {
                alert.IsSended = true;
            }

            Repo.Alert.Instance.Update(ref alerts, Util.GetUserLanguageInformation(userId));
            return alerts;
        }

        public void SetAlertAsClosed(int id, int userId)
        {
            var alert = Repo.Alert.Instance.GetById(id);

            alert.IsClosed = true;

            Repo.Alert.Instance.Update(ref alert, infos: Util.GetUserLanguageInformation(userId));
        }
    }
}
