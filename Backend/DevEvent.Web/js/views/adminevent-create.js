(function ($) {
    'use strict'
    $('#startdate').datetimepicker({ format: 'YYYY-MM-DD a h:mm:ss', sideBySide: true });
    $('#enddate').datetimepicker({ format: 'YYYY-MM-DD a h:mm:ss', sideBySide: true });
    // TODO: enddate > startdate

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
    // Translate from address to map
    $('#showonmap').click(function () {
        setMapOnAll(null);
        var address = $('#address').val();
        geocoder.geocode({ 'address': address }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {
                var loc = results[0].geometry.location;
                map.setCenter(loc);
                // set input value
                $('#latitude').val(loc.lat());
                $('#longitude').val(loc.lng());
                
                var marker = new google.maps.Marker({
                    map: map,
                    position: loc
                });
                markers.push(marker);
            } else {
                alert("Geocode was not successful for the following reason: " + status);
            }
        });
    });

    // Check validation
    if ($('#newevent-form').length > 0) {
        $('#newevent-form').validate({
            submitHandler: function (form) {
                $('#submit-button').button("loading");
                form.submit();
            },
            errorPlacement: function (error, element) {
                error.insertAfter(element);
            },
            errorClass: "text-danger",
            onkeyup: function (element) { $(element).valid() },
            onclick: function (element) { $(element).valid() },
            rules: {
                title: {
                    required: true
                },
                startdate: {
                    required: true
                },
                enddate: {
                    required: true
                },
                featuredimagefile: {
                    required: true
                },
                registrationurl: {
                    url: true
                }
            },
            errorElement: "div",
            highlight: function (element) {
                $(element).parent().removeClass("has-success").addClass("has-error");
                $(element).siblings("label").addClass("hide");
            },
            success: function (element) {
                $(element).parent().removeClass("has-error").addClass("has-success");
                $(element).siblings("label").removeClass("hide");
            }
        });
    };
})(this.jQuery);