(function ($) {
    'use strict'
    $('#startdate').datetimepicker({ format: 'YYYY-MM-DD h:mm:ss a', sideBySide: true });
    $('#enddate').datetimepicker({ format: 'YYYY-MM-DD h:mm:ss a', sideBySide: true });
    // enddate > startdate

    var geocoder;
    var markers = [];
    var map;
    geocoder = new google.maps.Geocoder();
    map = new google.maps.Map(document.getElementById("map"), { zoom: 16, center: { lat: 37.566535, lng: 126.97796919999996 }, scrollwheel: false });
    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    $('#showonmap').click(function () {
        setMapOnAll(null);
        var address = $('#address').val();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                map.setCenter(results[0].geometry.location);
                
                var marker = new google.maps.Marker({
                    map: map,
                    position: results[0].geometry.location
                });
                markers.push(marker);
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    });
})(this.jQuery);