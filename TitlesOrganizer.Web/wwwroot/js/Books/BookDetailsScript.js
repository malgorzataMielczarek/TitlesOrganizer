function Delete(id) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "/Books/BookDelete",
        type: 'DELETE',
        data: { "id": id, "__RequestVerificationToken": token },
        success: function (response) {
            fhToastr.success(response);
            setTimeout(() => {
                location.replace("/Books/Books");
            }, 1500);
        },
        error: function (response) {
            fhToastr.error(response);
        }
    });
}