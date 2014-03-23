using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using Microsoft.AspNet.SignalR;
using Repository.Base;
using Utilities.DataTypes.ExtensionMethods;
using Models = Model.Models._Alert;

namespace Web.Hubs
{
	

	[Authorize]
	public class AlertHub : Hub
	{
		public AlertHub()
		{
		}

		public bool StartConnection()
		{
			return true;
		}

		public IEnumerable<Models.Alert> CheckForAlerts()
		{
			var id = Context.User.Identity;
			var userId = id.Name;
			var alerts = Business.Alert
				.Instance.GetAllActiveAlertsForUser(Convert.ToInt32(userId));

			return alerts;
		}


		public void EndAlert(int id)
		{
			var userId = Convert.ToInt32(Context.User.Identity.Name);
			Business.Alert.Instance.SetAlertAsClosed(id, userId);
		}


		public void EmmitAlert(Models.Alert alert)
		{
			Clients.All.emmitAlert(alert);
		}

		public static void EmmitAlert(IHubContext context, Models.Alert alert)
		{
			context.Clients.All.emmitAlert(alert);
		}
	}
}