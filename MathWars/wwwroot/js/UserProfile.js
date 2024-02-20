$(document).ready(function () {
    var validator = $("form").validate({
        rules: {
            ImageFile: {
                required: true
            }
        },
        messages: {
            ImageFile: {
                required: "Prosz\u0119 wybra\u0107 plik obrazu."
            }
        },
        errorPlacement: function (error, element) {
            error.appendTo(element.parent());
        },
        submitHandler: function (form) {
            form.submit();
        }
    });

    $('#aModal').on('hidden.bs.modal', function () {
        validator.resetForm();
        $('form')[0].reset();
    });
});
