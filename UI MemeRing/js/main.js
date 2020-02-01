
$(function () {
    "use strict";

    /*[ Focus input ]*/
    $('.input100').each(function () {
        $(this).on('blur', function () {
            if ($(this).val().trim() != "") {
                $(this).addClass('has-val');
            }
            else {
                $(this).removeClass('has-val');
            }
        })
    })


    /*[ Validate ]*/
    var inputDoc = $('.validate-input .input100');

    $('#submitLogin').on('click', function () {
        var check = true;

        for (var i = 0; i < inputDoc.length; i++) {
            if (validate(inputDoc[i]) == false) {
                showValidate(inputDoc[i]);
                check = false;
            }
        }
        if (inputDoc.length == 2) {
            if (check == false) {
                return false;
            }
            else {
                login();
                return true;
            }
        }
        else {
            if (check == false) {
                return false;
            }
            else {
                register();
                return true;
            }
        }
    });


    $('#uploadFile').on('click', function () {
        let check = true;
        const item = document.getElementById("Description");

        if (validate(item) == false) {
            showValidate(item);
            check = false;
        }
        if (check == false) {
            return false;
        }
        else {
            upload();
            return true;
        }
    })

    $('#updateFile').on('click', function () {
        let check = true;
        const item = document.getElementById("descriptionUpdate");

        if (validate(item) == false) {
            showValidate(item);
            check = false;
        }
        if (check == false) {
            return false;
        }
        else {
            update();
            return true;
        }
    })


    $('.validate-form .input100').each(function () {
        $(this).focus(function () {
            hideValidate(this);
        });
    });

    function validate(input) {
        if ($(input).attr('type') == 'username' || $(input).attr('name') == 'username') {
            if ($(input).val().length < 6 || $(input).val().length > 50) {
                return false;
            }
        }
        else if ($(input).attr('type') == 'confirmPassword' || $(input).attr('name') == 'confirmPassword') {
            const password = $(inputDoc[1]).val();
            if ($(input).val() != password || $(input).val().length < 6 || $(input).val().length > 50) {
                return false;
            }
        }
        else if ($(input).attr('name') == 'uploadFileText') {
            if ($(input).val().length == 0 || $(input).val().length > 30) {
                return false;
            }
        }
        else if ($(input).attr('name') == 'updateFileText') {
            if ($(input).val().length == 0 || $(input).val().length > 30) {
                return false;
            }
        }
        else {
            if ($(input).val().length < 6 || $(input).val().length > 50) {
                return false;
            }
        }
    }

    function showValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).addClass('alert-validate');
    }

    function hideValidate(input) {
        var thisAlert = $(input).parent();

        $(thisAlert).removeClass('alert-validate');
    }

    /*[ Show pass ]*/
    var showPass = 0;
    $('.btn-show-pass').on('click', function () {
        if (showPass == 0) {
            $(this).next('input').attr('type', 'text');
            $(this).find('i').removeClass('zmdi-eye');
            $(this).find('i').addClass('zmdi-eye-off');
            showPass = 1;
        }
        else {
            $(this).next('input').attr('type', 'password');
            $(this).find('i').addClass('zmdi-eye');
            $(this).find('i').removeClass('zmdi-eye-off');
            showPass = 0;
        }

    });

});