document.addEventListener("submit", (event) => function () {
    CheckAuthors();
    CheckGenres();
});

var year = document.getElementById("Year");
year.addEventListener("click", (event) => YearFirstClicked());
year.addEventListener("focus", (event) => YearFirstClicked());

function CheckAuthors() {
    var authors = document.getElementById("authors").innerText;
    if (authors.trim().length > 0) {
        $("#authorsError").hide();
    }
    else {
        $("#authorsError").show();
    }
}

function CheckGenres() {
    var genres = document.getElementById("genres").innerText;
    if (genres.trim().length > 0) {
        $("#genresError").hide();
    }
    else {
        $("#genresError").show();
    }
}

function YearFirstClicked() {
    if (!year.value) {
        year.value = year.max;
    }
}

function SelectAuthors() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var title = document.getElementById("Title").value;
    var id = document.getElementById("Id").value;
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: "/Books/SelectAuthorsForBook",
            data: { "title": title, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select authors");
                document.getElementById("Id").value = document.getElementById("bookId").value;
            }
        });
    }
    else {
        jqxhr = $.ajax({
            method: "POST",
            url: "/Books/SelectAuthorsForBook",
            data: { "id": id, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select authors");
            }
        });
    }

    jqxhr.fail(function (response) {
        fhToastr.error(response.responseText);
    });
}

function SelectGenres() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var title = document.getElementById("Title").value;
    var id = document.getElementById("Id").value;
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: "/Books/SelectGenresForBook",
            data: { "title": title, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select genres");
                document.getElementById("Id").value = document.getElementById("bookId").value;
            }
        });
    }
    else {
        jqxhr = $.ajax({
            method: "POST",
            url: "/Books/SelectGenresForBook",
            data: { "id": id, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select genres");
            }
        });
    }

    jqxhr.fail(function (response) {
        fhToastr.error(response.responseText);
    });
}

function SelectSeries() {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var title = document.getElementById("Title").value;
    var id = document.getElementById("Id").value;
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: "/Books/SelectSeriesForBook",
            data: { "title": title, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select book series");
                document.getElementById("Id").value = document.getElementById("bookId").value;
            }
        });
    }
    else {
        jqxhr = $.ajax({
            method: "POST",
            url: "/Books/SelectSeriesForBook",
            data: { "id": id, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select book series");
            }
        });
    }

    jqxhr.fail(function (response) {
        fhToastr.error(response.responseText);
    });
}

function LoadToModal(response, title) {
    document.querySelector("#partialModal .modal-header > .modal-title").innerText = title;
    $("#partialModal").find(".modal-body").remove("form");
    $("#partialModal").find(".modal-body").html(response);
    $("#partialModal").modal('show');
}

function FormSubmit(event) {
    var element = event.currentTarget;
    $(element.form).trigger('submit');
    event.preventDefault();
    event.stopImmediatePropagation();
}

function PagerClick(index) {
    document.getElementById("pageNo").value = index;
}

function CloseModal() {
    document.getElementById("closeModal").value = true;
}

function SelectAuthorsSubmit(event) {
    $.ajax({
        url: $(event.target).attr('action'),
        type: 'POST',
        data: $(event.target).serialize(),
        success: function (response) {
            if (!Array.isArray(response) && response.startsWith("<form")) {
                LoadToModal(response, "Select authors");
            }
            else {
                $("#partialModal").modal('hide');
                document.querySelector("#partialModal .modal-header > .modal-title").innerText = '';
                $("#partialModal").find(".modal-body").remove("form");
                var authors = '';
                if (response.length > 0) {
                    if (document.getElementById("authorsError")) {
                        $("#authorsError").hide();
                    }

                    authors = response;
                }

                document.getElementById("authors").innerText = authors;
            }
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function AddNewAuthor(event) {
    var name = document.getElementById("newAuthorName").value;
    var lastName = document.getElementById("newAuthorLastName").value;
    if (name.trim().length <= 0 && lastName.trim().length <= 0) {
        ShowNewAuthorError("Enter name or/and last name of the author.")
        return;
    }

    HideNewAuthorError();
    $.ajax({
        url: "/Books/AddNewAuthor",
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

function SelectGenresSubmit(event) {
    $.ajax({
        url: $(event.target).attr('action'),
        type: 'POST',
        data: $(event.target).serialize(),
        success: function (response) {
            if (!Array.isArray(response) && response.startsWith("<form")) {
                LoadToModal(response, "Select genres");
            }
            else {
                $("#partialModal").modal('hide');
                document.querySelector("#partialModal .modal-header > .modal-title").innerText = '';
                $("#partialModal").find(".modal-body").remove("form");
                var genres = '';
                if (response.length > 0) {
                    if (document.getElementById("genresError")) {
                        $("#genresError").hide();
                    }

                    genres = response;
                }

                document.getElementById("genres").innerText = genres;
            }
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function AddNewGenre(event) {
    var name = document.getElementById("newGenreName").value;
    if (name.trim().length <= 0) {
        ShowNewAuthorError("Enter name of the genre.")
        return;
    }

    HideNewGenreError();
    $.ajax({
        url: "/Books/AddNewGenre",
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

function SelectSeriesSubmit(event) {
    $.ajax({
        url: $(event.target).attr('action'),
        type: 'POST',
        data: $(event.target).serialize(),
        success: function (response) {
            if (!Array.isArray(response) && response.startsWith("<form")) {
                LoadToModal(response, "Select book series");
            }
            else {
                $("#partialModal").modal('hide');
                document.querySelector("#partialModal .modal-header > .modal-title").innerText = '';
                $("#partialModal").find(".modal-body").remove("form");
                var series = '';
                if (response.length > 0) {
                    series = response;
                }

                document.getElementById("series").innerText = series;
            }
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function AddNewSeries(event) {
    var title = document.getElementById("newSeriesTitle").value;
    if (title.trim().length <= 0) {
        ShowNewAuthorError("Enter title of the book series.")
        return;
    }

    HideNewSeriesError();
    $.ajax({
        url: "/Books/AddNewSeries",
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