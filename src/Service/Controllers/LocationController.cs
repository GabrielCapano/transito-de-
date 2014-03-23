using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using GoogleMapsApi.Engine;
using GoogleMapsApi.Entities.Geocoding.Request;
using WebGrease.Css.Extensions;

namespace Service.Controllers
{
    public class LocationController : ApiController
    {

        /// <summary>
        /// Pega o lat/lng conforme o local
        /// </summary>  
        /// <param name="local">Endereço</param>
        /// <returns></returns>
        public LatLng Get(string local)
        {
            var geocode = new GeocodingRequest
            {
                Address = local
            };

            var ret = GoogleMapsApi.GoogleMaps.Geocode.Query(geocode);
            var lat = new LatLng();
            foreach (var result in ret.Results)
            {
                lat.Latitude = result.Geometry.Location.Latitude;
                lat.Longitude = result.Geometry.Location.Longitude;
            }

            return lat;
        }
    }

    public class LatLng     
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}