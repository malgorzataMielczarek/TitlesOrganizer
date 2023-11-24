function Delete(id, entityType) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = "/Books/" + entityType + "Delete";
    var redirectUrl = "/Books/" + entityType;
    if (!entityType.endsWith('s')) {
        redirectUrl += 's';
    }

    $.ajax({
        url: url,
        type: 'DELETE',
        data: { "id": id, "__RequestVerificationToken": token },
        success: function (response) {
            fhToastr.success(response);
            setTimeout(() => {
                location.replace(redirectUrl);
            }, 1500);
        },
        error: function (response) {
            fhToastr.error(response);
        }
    });
}