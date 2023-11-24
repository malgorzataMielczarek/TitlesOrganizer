function PagerClick(index) {
    document.getElementById("pageNo").value = index;
    Submit();
}

function Submit() {
    document.forms[0].submit();
}

function ShowDetails(id, entityType) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = "/Books/" + entityType + "DetailsPartial";
    $.post(url, {
        "id": id,
        "__RequestVerificationToken": token
    })
        .done(function (response) {
            $("#partialModal").find(".modal-body").html(response);
            $("#partialModal").modal('show');
        })
        .fail(function (response) {
            fhToastr.error(response.responseText);
        });
}