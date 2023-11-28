document.addEventListener("submit", (event) => function () {
    Check("authors");
    Check("genres");
});

var year = document.getElementById("Year");
year.addEventListener("click", (event) => YearFirstClicked());
year.addEventListener("focus", (event) => YearFirstClicked());

function Check(elementId) {
    var element = document.getElementById(elementId).innerText;
    var errorSelactor = "#" + elementId + "Error";
    if (element.trim().length > 0) {
        $(errorSelactor).hide();
    }
    else {
        $(errorSelactor).show();
    }
}

function YearFirstClicked() {
    if (!year.value) {
        year.value = year.max;
    }
}

function SelectForBook(entityT) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var title = document.getElementById("Title").value;
    var id = document.getElementById("Id").value;
    if (!entityT.endsWith('s')) {
        entityT += 's';
    }

    var url = "/Books/Select" + entityT + "ForBook";
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: url,
            data: { "title": title, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select " + entityT.toLowerCase());
                document.getElementById("Id").value = document.getElementById("bookId").value;
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

function SubmitSelectForBook(event, entityT) {
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
                var result = '';
                if (response.length > 0) {
                    if (document.getElementById(elementId + "Error")) {
                        $("#" + elementId + "Error").hide();
                    }

                    result = response;
                }

                document.getElementById(elementId).innerText = result;
            }
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function AddNewAuthorForBook(event) {
    var name = document.getElementById("newAuthorName").value;
    var lastName = document.getElementById("newAuthorLastName").value;
    if (name.trim().length <= 0 && lastName.trim().length <= 0) {
        ShowNewAuthorError("Enter name or/and last name of the author.")
        return;
    }

    HideNewAuthorError();
    $.ajax({
        url: "/Books/AddNewAuthorForBook",
        type: 'PUT',
        data: $(document.forms.selectAuthors).serialize(),
        success: function (response) {
            LoadToModal(response, "Select authors");
            fhToastr.success("New author added");
        },
        error: function (response) {
            ShowNewAuthorError(response.responseText);
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function HideNewAuthorError() {
    $("#newAuthorError").hide();
}

function ShowNewAuthorError(message) {
    document.getElementById("newAuthorError").innerText = message;
    $("#newAuthorError").show();
}

function AddNewGenreForBook(event) {
    var name = document.getElementById("newGenreName").value;
    if (name.trim().length <= 0) {
        ShowNewAuthorError("Enter name of the genre.")
        return;
    }

    HideNewGenreError();
    $.ajax({
        url: "/Books/AddNewGenreForBook",
        type: 'PUT',
        data: $(document.forms.selectGenres).serialize(),
        success: function (response) {
            LoadToModal(response, "Select genres");
            fhToastr.success("New genre added");
        },
        error: function (response) {
            ShowNewGenreError(response.responseText);
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function HideNewGenreError() {
    $("#newGenreError").hide();
}

function ShowNewGenreError(message) {
    document.getElementById("newGenreError").innerText = message;
    $("#newGenreError").show();
}

function AddNewSeriesForBook(event) {
    var title = document.getElementById("newSeriesTitle").value;
    if (title.trim().length <= 0) {
        ShowNewAuthorError("Enter title of the book series.")
        return;
    }

    HideNewSeriesError();
    $.ajax({
        url: "/Books/AddNewSeriesForBook",
        type: 'PUT',
        data: $(document.forms.selectSeries).serialize(),
        success: function (response) {
            LoadToModal(response, "Select book series");
            fhToastr.success("New book series added");
        },
        error: function (response) {
            ShowNewSeriesError(response.responseText);
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function HideNewSeriesError() {
    $("#newSeriesError").hide();
}

function ShowNewSeriesError(message) {
    document.getElementById("newSeriesError").innerText = message;
    $("#newSeriesError").show();
}

function SeriesCheckChange(element) {
    if (element.checked) {
        element.parentElement.parentElement.querySelectorAll("input:checked").forEach(el => el.checked = false);
        element.checked = true;
    }
}