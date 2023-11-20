function AuthorsPagerClick(index) {
    document.getElementById("pageNo").value = index;
}

function SelectAuthorsSubmit(event) {
    $.ajax({
        url: $(event.target).attr('action'),
        type: 'POST',
        data: $(event.target).serialize(),
        success: function (response) {
            HandleSuccessResponse(response);
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}
function HandleSuccessResponse(response) {
    if (response.startsWith("<form")) {
        document.querySelector("#partialModal .modal-header > .modal-title").innerText = "Select authors";
        $("#partialModal").find(".modal-body").html(response);
        $("#partialModal").modal('show');
    }
    else {
        $("#partialModal").modal('hide');
        document.getElementById("authorsDiv").innerText = response;
    }
}

function CloseModal() {
    document.getElementById("closeModal").value = true;
}