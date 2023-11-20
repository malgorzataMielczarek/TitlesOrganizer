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
    if (!Array.isArray(response) && response.startsWith("<form")) {
        document.querySelector("#partialModal .modal-header > .modal-title").innerText = "Select authors";
        $("#partialModal").find(".modal-body").html(response);
        $("#partialModal").modal('show');
    }
    else {
        $("#partialModal").modal('hide');
        document.getElementById("Authors").value = response;
        var authors = '';
        for (var i = 0; i < response.length - 1; i++) {
            authors += response[i].description + ", ";
        }
        authors += response[response.length - 1].description;
        document.getElementById("authorsDiv").innerText = authors;
    }
}

function CloseModal() {
    document.getElementById("closeModal").value = true;
}