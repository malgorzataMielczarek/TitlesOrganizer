document.addEventListener("submit", (event) => function () {
    Check("authors");
    Check("genres");
});

var year = document.getElementById("Year");
year.addEventListener("click", (event) => YearFirstClicked());
year.addEventListener("focus", (event) => YearFirstClicked());

function YearFirstClicked() {
    if (!year.value) {
        year.value = year.max;
    }
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