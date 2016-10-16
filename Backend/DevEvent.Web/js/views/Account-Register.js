(function ($) {
    'use strict'

    if ($('#register-form').length > 0) {
        $('#register-form').validate({
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
                email: {
                    required: true,
                    email: true,
                    remote: {
                        url: "/account/validemail",
                        type: "post",
                        data: {
                            email: function () {
                                return $("#inputEmail").val();
                            }
                        }
                    }
                },
                password: {
                    required: true,
                    minlength: 5
                },
                passwordconfirm: {
                    minlength: 5,
                    equalTo: "#inputPassword"
                },
                name: {
                    required: true,
                },
            },
            messages: {
                email: {
                    required: "이메일을 입력해주세요",
                    email: "정확한 이메일 주소를 입력해주세요",
                    remote: "이미 사용중인 이메일 주소 입니다"
                },
                password: {
                    required: "비밀번호를 입력해주세요",
                    minlength: "비밀번호는 5글자 이상 입력해주세요"

                },
                passwordconfirm: {
                    minlength: "비밀번호는 5글자 이상 입력해주세요",
                    equalTo: "입력된 비밀번호가 서로 다릅니다",
                },
                name: {
                    required: "이름을 입력해주세요",
                },
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