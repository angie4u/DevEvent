(function ($) {
    'use strict'
    $('#startdate').datetimepicker({ format: 'YYYY-MM-DD a h:mm:ss', sideBySide: true });
    $('#enddate').datetimepicker({ format: 'YYYY-MM-DD a h:mm:ss', sideBySide: true });
    // TODO: enddate > startdate

    var geocoder = new google.maps.Geocoder();
    var markers = [];
    var map = new google.maps.Map(document.getElementById("map"), { zoom: 16,  scrollwheel: false });
    

    
    function setLocation(loc) {
        map.setCenter(loc);
        var marker = new google.maps.Marker({
            map: map,
            position: loc
        });
        markers.push(marker);
    }
    
    function setMapOnAll(map) {
        for (var i = 0; i < markers.length; i++) {
            markers[i].setMap(map);
        }
    }

    // Translate from address to map
    $('#showonmap').click(function () {
        setMapOnAll(null);
        var address = $('#address').val();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status === google.maps.GeocoderStatus.OK) {
                var loc = results[0].geometry.location;
                // set input value
                $('#latitude').val(loc.lat());
                $('#longitude').val(loc.lng());
                
                setLocation(loc);
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    });

    $('#delete').click(function () {
        if(confirm("Are you sure?")) {

            $.ajax({
                type: 'POST',
                url: "/adminevent/delete/",
                data: {id: $('#id').val()},
                async: false,
                success: function () {
                    window.location.href = "/adminevent";
                }
            });

        } else {
            return false;
        }
    });

    var location;
    if ($('#latitude').val() && $('#longitude').val()) {
        location = new google.maps.LatLng($('#latitude').val(),  $('#longitude').val());
    } else {
        location = new google.maps.LatLng(37.566535,  126.97796919999996); 
    }
    setLocation(location);

})(this.jQuery);