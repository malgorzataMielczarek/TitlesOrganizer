function SelectForGenre(entityT) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var name = document.getElementById("Name").value;
    var id = document.getElementById("Id").value;
    if (!entityT.endsWith('s')) {
        entityT += 's';
    }

    var url = "/Books/Select" + entityT + "ForGenre";
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: url,
            data: { "name": name, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select " + entityT.toLowerCase());
                document.getElementById("Id").value = document.getElementById("genreId").value;
            }
        });
    }
    else {
        jqxhr = $.ajax({
            method: "POST",
            url: url,
            data: { "id": id, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select " + entityT.toLowerCase());
            }
        });
    }

    jqxhr.fail(function (response) {
        fhToastr.error(response.responseText);
    });
}

function SubmitSelectForGenre(event, entityT) {
    var elementId = entityT.toLowerCase();
    if (!entityT.endsWith('s')) {
        elementId += "s";
    }

    $.ajax({
        url: $(event.target).attr('action'),
        type: 'POST',
        data: $(event.target).serialize(),
        success: function (response) {
            if (!Array.isArray(response) && response.trim().startsWith("<form")) {
                LoadToModal(response, "Select " + elementId);
            }
            else {
                $("#partialModal").modal('hide');
                document.querySelector("#partialModal .modal-header > .modal-title").innerText = '';
                $("#partialModal").find(".modal-body").remove("form");
                if (response.length > 0) {
                    if (document.getElementById(elementId + "Error")) {
                        $("#" + elementId + "Error").hide();
                    }
                }

                Reload("Book");
            }
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function AddNewBookForGenre(event) {
    var title = document.getElementById("newBookTitle").value;
    if (title.trim().length <= 0) {
        ShowNewBookError("Enter title of the book.");
        return;
    }

    HideNewBookError();
    $.ajax({
        url: "/Books/AddNewBookForGenre",
        type: 'PUT',
        data: $(document.forms.selectBooks).serialize(),
        success: function (response) {
            LoadToModal(response, "Select books");
            fhToastr.success("New book added");
        },
        error: function (response) {
            ShowNewBookError(response.responseText);
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function HideNewBookError() {
    $("#newBookError").hide();
}

function ShowNewBookError(message) {
    document.getElementById("newBookError").innerText = message;
    $("#newBookError").show();
}