var alerts = window.alerts = {
    init: function() {
        alerts.load.signalR(alerts.load.events);
    },
    
    load: {
        signalR: function (callback) {
            alerts.hub = $.connection.alertHub;
            alerts.hub.client.emmitAlert = alerts.emmitAlert;
            $.connection.hub.start().done(function() {
                alerts.hub.server.startConnection().done(callback);
            });
        },
        events: function (resp) {
            alerts.checkMessages();
        }
    },

    hub: null,

    emmitAlert: function (resp) {
        $.gritter.add({
            title: resp.Title,
            text: resp.Message,
            sticky: true,
            class_name: 'id-' + resp.Id,
            before_close: function (e, a) {
                var classe = e.attr('class');
                var id = classe.split('id-')[1].split(' ')[0];
                alerts.hub.server.endAlert(id);
            }
        });
    },
    
    checkMessages: function() {
        alerts.hub.server.checkForAlerts().done(function (resp) {
            for (var i = 0; i < resp.length; i++) {
                $.gritter.add({
                    title: resp[i].Title,
                    text: resp[i].Message,
                    sticky: true,
                    class_name: 'id-' + resp[i].Id, 
                    before_close: function (e, a) {
                        var classe = e.attr('class');
                        var id = classe.split('id-')[1].split(' ')[0];
                        alerts.hub.server.endAlert(id);
                    }
                });
            }
        });
    }
};
$(document).ready(alerts.init);