(function ($) {
    'use strict'

    // Check Page validation
    if ($('#page-form').length > 0) {
        $('#page-form').validate({
            submitHandler: function (form) {
                $('#submit-button').button("loading");
                var pagenumber = $('#pagenumber').val();
                var limit = $('#limit').val();
                var offset = limit * (pagenumber - 1);
                $('#offset').val(offset);

                form.submit();
            },
            errorPlacement: function (error, element) {
            },
            errorClass: "text-danger",
            onkeyup: function (element) { $(element).valid() },
            //onclick: function (element) { $(element).valid() },
            rules: {
                pagenumber: {
                    min: 1,
                    max: maxpagenumber
                }
            },
            highlight: function (element) {
                $(element).parent().removeClass("has-success").addClass("has-error");
                //$(element).siblings("label").addClass("hide");
            },
            success: function (element) {
                $(element).parent().removeClass("has-error").addClass("has-success");
                //$(element).siblings("label").removeClass("hide");
            }
        });
    };
})(this.jQuery);