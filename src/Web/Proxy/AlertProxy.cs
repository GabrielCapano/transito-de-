using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Business;
using Business.Base;
using Microsoft.AspNet.SignalR;
using Repository.Base;
using Web.Hubs;
using Models = Model.Models._Alert;

namespace Web.Proxy
{
    public class AlertProxy
    {
        public void NewAlert(Models.Alert alert)
        {
            ResponseObject response = Business.Alert.Instance.NewAlert(alert);

            if (response.Status)
            {
                EmmitAlert(alert);
            }
        }

        private void EmmitAlert(Models.Alert alert)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<AlertHub>();
            context.Clients.All.emmitAlert(alert);
        }

        public void EmmitActiveAlerts()
        {
            var alerts = Business.Alert.Instance.GetAllActiveAlertsForUser();

            foreach (var alert in alerts)
            {
                EmmitAlert(alert);
            }
        } 
    }
}