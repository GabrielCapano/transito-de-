function GoogleMaps() {

    this.map = null;
    this.marker = null;
    this.markers = [];
    this.input = null;
    this.directionsDisplay = null;
    this.infoWindow = null;
    this.directionsService = null;
    this.initialPosition = null;
    this.bootstrapEnabled = (typeof $().modal == 'function');

    this.calcRoute = function (gmInstance) {

        if (gmInstance == null)
            gmInstance = this;

        var first = $('.address-holder:first');
        var start = gmInstance.getLatLng(first);
        var end = gmInstance.getLatLng($('.address-holder:last'));
        var waypts = [];
        var arr = $('.address-holder').not(':first, :last');

        arr.each(function () {
            waypts.push({
                location: gmInstance.getLatLng($(this)),
                stopover: true
            });
        });

        var request = {
            origin: start,
            destination: end,
            waypoints: waypts,
            optimizeWaypoints: false,
            travelMode: window.google.maps.TravelMode.DRIVING
        };

        var a = this.directionsDisplay;
        this.directionsService.route(request, function (response, status) {
            if (status == window.google.maps.DirectionsStatus.OK) {
                a.setDirections(response);
            }
        });
    },

    this.getLatLng = function (obj) {
        var lat = $(obj).find('[data-dgd-name=Latitude]').val();
        var lng = $(obj).find('[data-dgd-name=Longitude]').val();

        return new window.google.maps.LatLng(lat, lng);
    },

    this.setMapSize = function (mapId, width, height) {
        if (width != null && height != null) {
            $("#" + mapId).css({
                'width': width,
                'height': height
            });
        }
        else {
            $("#" + mapId).css({
                'width': '500px',
                'height': '500px'
            });
        }
    },

    this.setStyle = function (mapId, className) {
        $('#' + mapId).wrap('<div class="' + className + '"></div>');
    },

    this.setInitialPosition = function (latitude, longitude) {
        if (latitude != null && longitude != null)
            this.initialPosition = new window.google.maps.LatLng(latitude, longitude);
        else
            this.initialPosition = new window.google.maps.LatLng(-23.5258546, -46.73222659999999);
    },

    this.clearMap = function () {

        window.google.maps.event.trigger(this.map, "resize");

        for (var i = 0; i < this.markers.length; i++)
            this.markers[i].setMap(null);

        this.markers = new Array();
    },

    this.configModal = function (modalId) {

        if (modalId != null) {

            var gmInstance = this;

            $('#' + modalId).on('shown', function () {
                gmInstance.clearMap();
                $('#' + modalId + ' input[type=text]').val('');
                $('#' + modalId + ' .map-holder').css('display', 'none');
                $('.PointName').html('');

            });

            $('#' + modalId).on('hidden', function () {
                gmInstance.clearMap();
                $('#' + modalId + ' input[type=text]').val('');
                $('#' + modalId + ' .map-holder').css('display', 'none');
                $('.PointName').html('');
            });
        }

    },

    this.createMap = function (mapId, width, height, modalId, latitude, longitude) {

        this.setInitialPosition(latitude, longitude);
        this.map = new window.google.maps.Map($("#" + mapId)[0], this.getMapOptions());
        if (width != 0 && height != 0)
            this.setMapSize(mapId, width, height);

        this.directionsDisplay = new window.google.maps.DirectionsRenderer();
        this.directionsDisplay.setMap(this.map);
        this.directionsService = new window.google.maps.DirectionsService();
        this.infoWindow = new window.google.maps.InfoWindow({ maxWidth: 320 });
        this.marker = new window.google.maps.Marker({ map: this.map });
        this.configModal(modalId);
        this.setStyle(mapId, 'map-wrap');
        if (this.bootstrapEnabled) $("<style type='text/css'>#" + mapId + " img { max-width: none; }</style>").appendTo("head");
    },

    this.getMapOptions = function () {

        var mapOptions = {
            center: this.initialPosition,
            zoom: 13,
            mapTypeId: window.google.maps.MapTypeId.ROADMAP,
            mapTypeControl: true,
            mapTypeControlOptions: {
                style: window.google.maps.MapTypeControlStyle.DROPDOWN_MENU
            }
        };

        return mapOptions;
    },

    this.createMarker = function (position, urlIcon, width, heigth) {

        var marker = new window.google.maps.Marker({
            position: position,
            map: this.map
        });

        marker.setIcon({
            url: urlIcon
        });

        marker.setPosition(position);
        this.markers.push(marker);

        return marker;
    },

    this.disableAllMarkerAnimation = function () {
        for (var i = 0; i < this.markers.length; i++)
            this.markers[i].setAnimation(null);
    },

    this.addMarkerAnimation = function (marker) {
        marker.setAnimation(window.google.maps.Animation.BOUNCE);
    },

    this.setInfoWindow = function (marker, place) {

        var gmInstance = this;

        window.google.maps.event.addListener(marker, 'click', function () {

            gmInstance.disableAllMarkerAnimation();
            gmInstance.addMarkerAnimation(marker);

            var closes = [];

            for (var h = 0; h < place.ClosestPoints; h++)
                closes.push(place.ClosestPoints[h]);

            closes.push(place);

            var htm = [];

            for (var j = 0; j < closes.length; j++) {
                htm.push('<a class="select-marker" id="marker-' + j +
                    '" point="' + closes[j].Id + '"hour="' + closes[j].Hour.substr(0, 5) + '" pointname="' + closes[j].Name + '" href="#">' + closes[j].Hour.substr(0, 5) + '</a>');
            }

            var boxInfo = '<div><h4 style="color:#350101;">Selecione o horário em que o usuário pegará o transporte.</h4></div>' +
                '<div  class="info-name-point">' + place.Name + '</div>' +
                '<div><strong>Horários:</strong> ' + htm.join(' / ') + '</div>';

            gmInstance.infoWindow.setContent(boxInfo);
            gmInstance.infoWindow.maxWidth = 320;
            gmInstance.infoWindow.open(gmInstance.map, marker);

            window.google.maps.event.addListener(gmInstance.infoWindow, 'closeclick', function () {
                gmInstance.disableAllMarkerAnimation();
            });
        });

    },

    this.cleanMarkers = function () {
        for (var i = 0; i < this.markers.length; i++)
            this.markers[i].setMap(null);
    },

    this.autoComplete = null,

    this.getLocationCloser = function (input, serviceId, callback) {

        var gmInstance = this;

        this.autoComplete = new window.google.maps.places.Autocomplete($("#" + input)[0]);
        this.autoComplete.bindTo('bounds', gmInstance.map);

        var autocomplete = this.autoComplete;

        window.google.maps.event.addListener(autocomplete, 'place_changed', function () {

            var place = autocomplete.getPlace();
            var latitude = place.geometry.location.lat();
            var longitude = place.geometry.location.lng();
            var data = {
                latitude: (latitude + '').replace('.', ','),
                longitude: (longitude + '').replace('.', ','),
                serviceId: serviceId,
                routeId: $('.route-btn.active').attr('data-routeId')
            };

            $("#" + input).closest('.active').find('.map-holder').show();
            window.google.maps.event.trigger(gmInstance.map, "resize");

            if (!place.geometry) return;

            if (place.geometry.viewport) {
                gmInstance.map.fitBounds(place.geometry.viewport);
            } else {
                gmInstance.map.setCenter(place.geometry.location);
            }



            gmInstance.cleanMarkers();


            setTimeout(function () {
                gmInstance.marker = gmInstance.createMarker(place.geometry.location, "/Content/images/icons/marker_person.png", 71, 71);
                gmInstance.map.setCenter(place.geometry.location);
                gmInstance.map.setZoom(16);
            }, 200);

            $.ajax({
                data: data,
                type: 'POST',
                url: '/RouteUser/GetLocationCloser',
                dataType: 'json',
                success: function (places) {

                    for (var i = 0; i < places.length; i++) {
                        var position = new window.google.maps.LatLng(places[i].Latitude, places[i].Longitude);
                        var marker = gmInstance.createMarker(position, "/Content/images/icons/marker_bus.png", 71, 71);
                        gmInstance.setInfoWindow(marker, places[i]);
                    }


                    if (typeof callback == "function")
                        callback();


                }
            });
        });
    },

    this.setRouteInformations = function (place, gmInstance) {
        var request = {
            origin: gmInstance.getLatLng($('.address-holder:last')),
            destination: place.geometry.location,
            travelMode: window.google.maps.TravelMode.DRIVING
        };

        gmInstance.directionsService.route(request, function (response, status) {

            if (status == window.google.maps.DirectionsStatus.OK) {

                var lastTime = $('.address-holder:last').find('[data-dgd-name=Hour]').val();
                var hour = +lastTime.split(':')[0];
                var minutes = +lastTime.split(':')[1];

                var totalMinutes = minutes - Math.ceil(+response.routes[0].legs[0].duration.value / 60);

                var totalHour = Math.floor(totalMinutes / 60);
                hour = +hour + totalHour;
                minutes = totalMinutes - (totalHour * 60);
                if (hour >= 24)
                    hour = hour - 24;

                if (hour < 10)
                    hour = "0" + hour;

                if (minutes < 10)
                    minutes = "0" + minutes;

                $('#Horário').val(hour + ":" + minutes);
            }
        });
    
        var placeData = {
            latitude: place.geometry.location.lat(),
            longitude: place.geometry.location.lng()
        };

        for (i = 0; i < place.address_components.length; ++i) {
            var component = place.address_components[i];
            if (component.types.indexOf("neighborhood") > -1)
                placeData.district = component.long_name;
            else if (component.types.indexOf("administrative_area_level_1") > -1)
                placeData.state = component.short_name;
            else if (component.types.indexOf("route") > -1)
                placeData.address = component.long_name;
            else if (component.types.indexOf("locality") > -1)
                placeData.city = component.long_name;
        }
     
        $('#Latitude').val(placeData.latitude);
        $('#Longitude').val(placeData.longitude);
        $('#Address').val(placeData.address);
        $('#State').val(placeData.state);
        $('#City').val(placeData.city);
        $('#District').val(placeData.district);

        $('#Horário').focus();
    },

    this.setWayPoint = function (input) {

        var gmInstance = this;

        setTimeout(gmInstance.calcRoute(gmInstance), 800);

        var autocomplete = new window.google.maps.places.Autocomplete($("#" + input)[0]);
        autocomplete.bindTo('bounds', gmInstance.map);

        window.google.maps.event.addListener(autocomplete, 'place_changed', function () {

            gmInstance.marker.setVisible(false);

            var place = autocomplete.getPlace();

            if (!place.geometry) return;

            if (place.geometry.viewport) {
                gmInstance.map.fitBounds(place.geometry.viewport);
            } else {
                gmInstance.map.setCenter(place.geometry.location);
                gmInstance.map.setZoom(17);
            }

            gmInstance.marker = gmInstance.createMarker(place.geometry.location, "/Content/images/icons/marker_person.png", 71, 71);
            gmInstance.marker.setDraggable(true);
            gmInstance.map.setZoom(16);

            gmInstance.setRouteInformations(place, gmInstance);


            google.maps.event.addListener(gmInstance.marker, 'dragend', function (event, a) {

                var geocoder = new google.maps.Geocoder();
                geocoder.geocode({
                    latLng: gmInstance.marker.getPosition()
                }, function (responses) {
                    if (responses && responses.length > 0) {
                        $('#pac-input').val(responses[0].formatted_address);
                        gmInstance.marker.setPosition(responses[0].geometry.location);
                        gmInstance.setRouteInformations(responses[0], gmInstance);
                    }
                });
            });

        });

    };
};
