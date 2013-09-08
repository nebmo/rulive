//https://developers.google.com/maps/articles/toomanymarkers
//https://github.com/Leaflet/Leaflet.markercluster


var map = new google.maps.Map(document.getElementById('map'), {
    zoom: 5,
    center: new google.maps.LatLng(55, 11),
    mapTypeId: google.maps.MapTypeId.ROADMAP
});

//google.maps.event.addListener(map, 'idle', showMarkers);

//function showMarkers() {
//    var bounds = map.getBounds();

//    // Call you server with ajax passing it the bounds
//    points([]);
//    ko.utils.arrayForEach(markers(), function (item) {
//        item.setMap(null);
//    });
//    markers([]);
//    loadTrucks();
//}




var markers = ko.observableDictionary();
var points = ko.observableArray();
var tracks = ko.observableDictionary();
var icon_url = "http://labs.google.com/ridefinder/images/";
var mapViewmodel;
//var markerCluster;

function Track(name, path) {
    me = this;
    this.name = name;
    this.path = ko.observable(path);
    this.following = ko.observable(false);
    
    this.followCommand = function () {
        this.following = !this.following;
       
        if(this.following) {
            mapViewmodel.following = this.key();
        } else {
            mapViewmodel.following = '';
        }
        this.value().follow(this.following);
        //mapViewmodel.zoomToLatLng(new google.maps.LatLng(tracks.get('nebmo')().getPath().b[tracks.get('nebmo')().getPath().b.length - 1].pb, tracks.get('nebmo')().getPath().b[tracks.get('nebmo')().getPath().b.length - 1].ob));
    };

    this.follow = function (following) {
        var bounds = new google.maps.LatLngBounds();
        var myLatLng = null;
        if (following) {
            //for (var i = 0; i < this.path().getPath().b.length; i++) {

            //    myLatLng = new google.maps.LatLng(this.path().getPath().b[i].ob, this.path().getPath().b[i].pb);

            //    bounds.extend(myLatLng);
            //}
            myLatLng = new google.maps.LatLng(this.path().getPath().b[this.path().getPath().b.length - 1].ob, this.path().getPath().b[this.path().getPath().b.length - 1].pb);
            
        } else {
            myLatLng = new google.maps.LatLng(55, 11);
        }
        
        if (map.getBounds().contains(myLatLng) && map.getZoom() == 15)
            return;
        
        bounds.extend(myLatLng);
        map.fitBounds(bounds);

        var follow = following;
        var zoomListener = google.maps.event.addListener(map, 'bounds_changed',
        function () {
            //map.setCenter(myLatLng);
            if (follow) {
                map.setZoom(15);
                //map.setCenter(myLatLng);
            } else {
                map.setZoom(5);
            }
            google.maps.event.removeListener(zoomListener);
        });
    };
}

function Point(name, lat, long, pointType, distance, timespent, pace) {
    this.name = name;
    this.lat = ko.observable(lat);
    this.long = ko.observable(long);
    this.pointType = ko.observable(pointType);
    this.distance = ko.observable(distance);
    this.timespent = ko.observable(timespent);
    this.pace = ko.observable(pace);
    
    points.push(this);
    var g = google.maps;
    var imageName = "mm_20_green.png";
    if (pointType == 2)
        imageName = "mm_20_red.png";
    var image = new g.MarkerImage(icon_url + imageName,
       new g.Size(12, 20),
       new g.Point(0, 0),
       new g.Point(6, 20));

    var shadow = new g.MarkerImage(icon_url + "mm_20_shadow.png",
      new g.Size(22, 20),
      new g.Point(0, 0),
      new g.Point(6, 20));

    //var marker = new g.Marker({
    //    position: new google.maps.LatLng(lat, long), map: map,
    //    icon: image, shadow: shadow,
    //    tooltip: name
    //});

    //var marker = new google.maps.Marker({
    //    position: new google.maps.LatLng(lat, long),
    //    title: name,
    //    map: map,
    //    draggable: false
    //});
    
    var marker = new MarkerWithLabel({
        position: new google.maps.LatLng(lat, long),
        draggable: false,
        raiseOnDrag: false,
        map: map,
        icon: image, shadow: shadow,
        labelContent: name + '<br/>' + distance + '<br/>' + timespent + '<br/>' + pace + ' min/km',
        labelAnchor: new google.maps.Point(50,0),
        labelClass: "markerlabel", // the CSS class for the label
        labelStyle: { opacity: 1 }
    });
    
    var listMarkers = markers.get(name);
    if(listMarkers() == null) 
    {
        listMarkers = [marker];
        markers.push(name,listMarkers);
    } 
    else 
    {
        var remove = listMarkers()[listMarkers().length - 1];
        remove.setMap(null);
        listMarkers().pop();
        listMarkers().push(marker);
    }
    
    
   

    
    var infowindow = new google.maps.InfoWindow({
        content: 'Name: '+name+'<br/>distance:'+distance+'<br/>LatLng:'+lat + ','+long
    });
    google.maps.event.addListener(marker, 'click', function () {
        // Calling the open method of the infoWindow 
        infowindow.open(map, marker);
    }.bind(this));
}

function MapViewModel() {
    mapViewmodel = this;
    this.following = ko.observable();
    loadTrucks();
    
    this.addPosition = function (runningEvent) {
        
        mapViewmodel.addPositionEvent(runningEvent.UserName, runningEvent.Lat, runningEvent.Long, runningEvent.RunningEventType, runningEvent.TotalDistance, runningEvent.TotalTime, runningEvent.Pace);
    };

    this.addPositionEvent = function (username, lat, long, eventtype, distance, timespent, pace) {
        new Point(username, lat, long, eventtype, distance, timespent, pace);

        var userData = tracks.get(username);
        var path = null;
        var point = new google.maps.LatLng(lat, long);
        if (userData() == null)
        {
            
            poly = new google.maps.Polyline({
                path: [point],
                strokeColor: "#006600",
                strokeOpacity: 1.0,
                strokeWeight: 2
            });
            tracks.push(username,new Track(username, poly));
            poly.setMap(map);
        }
        else
        {
            var path = userData().path().getPath();
            path.push(point);
            if (username == this.following) {
                userData().follow(true);
            }
        }
        
        //if(tracks.items().length == 1) {
        //    map.zoom = 15;
        //    map.center = new google.maps.LatLng(lat, long);
        //}

    };
    
    

    this.zoomToLatLng = function (latlng) {
        map.zoom = 14;
        map.center = latlng;
    };
}


//function drawLines(name, polylineCoordinates) {
    
//    var polyline = new google.maps.Polyline({
//        path: polylineCoordinates,
//        strokeColor: "#006600",
//        strokeOpacity: 1.0,
//        strokeWeight: 2
//    });
//    tracks.push(name, polyline);
//    polyline.setMap(map);
    
//}

function loadTrucks() {
    $.ajax({
        type: 'GET',
        url: '../api/Resource/',
        dataType: 'json',
        contentType: "application/json",
        success: function (result) {
            
            result.forEach(function (item) {
                
                if (item != null)
                {
                    //tmp.push(new Point(item.registrationNumber, item.positions[0].lat, item.positions[0].long, item.positions[0].runningEventType));
                    //if (item.positions.length > 1) {
                    //    tmp.push(new Point(item.registrationNumber, item.positions[item.positions.length - 1].lat, item.positions[item.positions.length - 1].long, item.positions[item.positions.length - 1].runningEventType));
                    //}
                    item.positions.forEach(function(position) {
                        //positions.push(new google.maps.LatLng(position.lat, position.long));
                        mapViewmodel.addPositionEvent(item.registrationNumber, position.lat, position.long, position.runningEventType, position.totalDistance, position.totalTime, position.pace);
                    });
                    //drawLines(item.registrationNumber,positions);
                }
            });
            
            //if(markerCluster == null)
                //initCluster();

        }
    });
}

//function initCluster() {
//    markerCluster = new MarkerClusterer(map, markers(), {
//         gridSize: 40,
//         minimumClusterSize: 4,
//         calculator: function (markers, numStyles) {
//             // Custom style can be returned here
//             return {
//                 text: markers.length,
//                 index: numStyles
//             };
//         }
//     });
    
//}
