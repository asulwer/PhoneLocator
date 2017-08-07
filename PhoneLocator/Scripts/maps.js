$(document).ready(function($) {
    'use strict';

    var notifications = $.connection.locationHub;
    notifications.client.receiveNotification = function (msg) {
        if (msg != 'initial') {
            getLocations();
        }
    };

    $.connection.hub.start().done(function () {
        notifications.server.sendNotifications('initial');
    }).fail(function (e) {
        alert(e);
    });

    var map = document.getElementById('map-canvas');
    var gpsTrackerMap = setup();
    getLocations();
        
    function setup() {
        if (map.id != 'map-canvas') {
            return null;
        }

        // clear any old map objects
        document.getElementById('map-canvas').outerHTML = "<div id='map-canvas'></div>";

        // use leaflet (http://leafletjs.com/) to create our map and map layers
        var gpsTrackerMap = new L.map('map-canvas');

        var openStreetMapsLayer = new L.TileLayer(('https:' == document.location.protocol ? 'https://' : 'http://') + '{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
            attribution: '&copy;2014 <a href="http://openstreetmap.org">OpenStreetMap</a> contributors'
        });

        var mapBoxMapsLayer = new L.TileLayer('https://api.tiles.mapbox.com/v4/{id}/{z}/{x}/{y}.png?access_token={accessToken}', {
            attribution: 'Map data &copy; <a href="http://openstreetmap.org">OpenStreetMap</a> contributors, <a href="http://creativecommons.org/licenses/by-sa/2.0/">CC-BY-SA</a>, Imagery © <a href="http://mapbox.com">Mapbox</a>',
            id: 'mapbox.streets',
            accessToken: 'pk.eyJ1IjoiYXN1bHdlciIsImEiOiJjajV3bzZ6a2QwZndhMnZxaXFwMTAwNGQ2In0.wswTWfC5qv6Nk9M-LjiyRw'
        });

        // this sets which map layer will first be displayed
        gpsTrackerMap.addLayer(openStreetMapsLayer);

        // this is the switcher control to switch between map types
        gpsTrackerMap.addControl(new L.Control.Layers({
            'MapBox': mapBoxMapsLayer,
            'OpenStreetMaps': openStreetMapsLayer
        }, {}));

        return gpsTrackerMap;
    }    
    function getLocations() {
        $.ajax({
            async: true,
            url: '/api/locations',
            type: 'GET',
            contents: 'application/json; charset=utf-8',
            dataType: 'json',
            success: function (data) {
                if (gpsTrackerMap != null) {
                    loadGPSLocations(data, gpsTrackerMap);
                }
            },
            error: function (xhr, status, errorThrown) {
                console.log("error status: " + status);
                console.log("errorThrown: " + errorThrown);
            }
        });
    }
    function loadGPSLocations(json, gpsTrackerMap) {
        var finalLocation = false;
        var locationArray = [];
                
        // iterate through the locations and create map markers for each location
        $(json).each(function(i,item){
            var latitude = item.Latitude;
            var longitude = item.Longitude;
            var tempLocation = new L.LatLng(latitude, longitude);
                    
            locationArray.push(tempLocation);                    
        
            // want to set the map center on the last location
            if ((json.length-1) === i) {
                finalLocation = true;
            }

            var marker = createMarker(
                latitude,
                longitude,
                item.Speed,
                item.Heading,
                item.Updated,
                item.User,
                gpsTrackerMap, finalLocation);
        });

        if (locationArray.length > 0) {
            // fit markers within window
            var bounds = new L.LatLngBounds(locationArray);
            gpsTrackerMap.fitBounds(bounds);
        }
    }
    function createMarker(latitude, longitude, speed, heading, gpsTime, userName, map, finalLocation) {
        var iconUrl;

        if (finalLocation) {
            iconUrl = 'images/coolred_small.png';
        }
        else {
            iconUrl = 'images/coolgreen2_small.png';
        }

        var markerIcon = new L.Icon({
            iconUrl: iconUrl,
            shadowUrl: 'images/coolshadow_small.png',
            iconSize: [12, 20],
            shadowSize: [22, 20],
            iconAnchor: [6, 20],
            shadowAnchor: [6, 20],
            popupAnchor: [-3, -25]
        });

        var lastMarker = "</td></tr>";

        // when a user clicks on last marker, let them know it's final one
        if (finalLocation) {
            lastMarker = "</td></tr><tr><td align=left>&nbsp;</td><td><b>Final location</b></td></tr>";
        }

        // convert from meters to feet
        var popupWindowText = "<table border=0 style=\"font-size:95%;font-family:arial,helvetica,sans-serif;color:#000;\">" +
            "<tr><td align=right>Direction:&nbsp;</td><td><img src=images/" + getCompassImage(heading) + ".jpg alt= />" + lastMarker + "</td></tr>" +
            "<tr><td align=right>Speed:&nbsp;</td><td>" + speed +  " mph</td></tr>" +
            "<tr><td align=right>Time:&nbsp;</td><td>" + gpsTime + "</td></tr>" +
            "<tr><td align=right>Name:&nbsp;</td><td>" + userName + "</td></tr></table>";

        var gpstrackerMarker;
        var title = userName + " - " + gpsTime;

        // make sure the final red marker always displays on top 
        if (finalLocation) {
            gpstrackerMarker = new L.marker(new L.LatLng(latitude, longitude), {title: title, icon: markerIcon, zIndexOffset: 999}).bindPopup(popupWindowText).addTo(map);
        }
        else {
            gpstrackerMarker = new L.marker(new L.LatLng(latitude, longitude), {title: title, icon: markerIcon}).bindPopup(popupWindowText).addTo(map);
        }
    }
    function getCompassImage(azimuth) {
        if ((azimuth >= 337 && azimuth <= 360) || (azimuth >= 0 && azimuth < 23))
            return "compassN";
        if (azimuth >= 23 && azimuth < 68)
            return "compassNE";
        if (azimuth >= 68 && azimuth < 113)
            return "compassE";
        if (azimuth >= 113 && azimuth < 158)
            return "compassSE";
        if (azimuth >= 158 && azimuth < 203)
            return "compassS";
        if (azimuth >= 203 && azimuth < 248)
            return "compassSW";
        if (azimuth >= 248 && azimuth < 293)
            return "compassW";
        if (azimuth >= 293 && azimuth < 337)
            return "compassNW";

        return "";
    }
});
$(window).on('beforeunload', function() {
    $.connection.hub.stop();
});

