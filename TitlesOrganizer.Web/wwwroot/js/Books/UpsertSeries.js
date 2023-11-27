document.addEventListener("submit", (event) => function () {
    CheckBooks();
});

function CheckBooks() {
    var element = $("#booksPartial");
    var errorSelactor = "#booksError";
    if (element.find("div.card-body").length == 0) {
        $(errorSelactor).hide();
    }
    else {
        $(errorSelactor).show();
    }
}

function SelectForSeries(entityT) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var title = document.getElementById("Title").value;
    var id = document.getElementById("Id").value;
    if (!entityT.endsWith('s')) {
        entityT += 's';
    }

    var url = "/Books/Select" + entityT + "ForSeries";
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: url,
            data: { "title": title, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select " + entityT.toLowerCase());
                document.getElementById("Id").value = document.getElementById("seriesId").value;
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

function SubmitSelectForSeries(event, entityT) {
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

function AddNewBookForSeries(event) {
    var title = document.getElementById("newBookTitle").value;
    if (title.trim().length <= 0) {
        ShowNewBookError("Enter title of the book.");
        return;
    }

    HideNewBookError();
    $.ajax({
        url: "/Books/AddNewBookForSeries",
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