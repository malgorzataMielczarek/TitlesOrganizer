function BooksPagerClick(index) {
    document.getElementById("booksPageNo").value = index;
    ReloadBooks();
}

function ReloadBooks() {
    var genreId = document.getElementById("genreId").value;
    var booksPageSize = document.getElementById("booksPageSize").value;
    var booksPageNo = document.getElementById("booksPageNo").value;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/BooksPartial", {
        "booksPageSize": booksPageSize,
        "booksPageNo": booksPageNo,
        "genreId": genreId,
        "__RequestVerificationToken": token
    })
        .done(function (response) {
            $("#genreDetails").find("#booksPartial").html(response);
            if (document.getElementById("partialModal")) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            alert(response.responseText);
        });
}

function AuthorsPagerClick(index) {
    document.getElementById("authorsPageNo").value = index;
    ReloadAuthors();
}

function ReloadAuthors() {
    var genreId = document.getElementById("genreId").value;
    var authorsPageSize = document.getElementById("authorsPageSize").value;
    var authorsPageNo = document.getElementById("authorsPageNo").value;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/AuthorsPartial", {
        "authorsPageSize": authorsPageSize,
        "authorsPageNo": authorsPageNo,
        "genreId": genreId,
        "__RequestVerificationToken": token
    })
        .done(function (response) {
            $("#genreDetails").find("#authorsPartial").html(response);
            if (document.getElementById("partialModal")) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            alert(response.responseText);
        });
}

function SeriesPagerClick(index) {
    document.getElementById("seriesPageNo").value = index;
    ReloadSeries();
}

function ReloadSeries() {
    var genreId = document.getElementById("genreId").value;
    var seriesPageSize = document.getElementById("seriesPageSize").value;
    var seriesPageNo = document.getElementById("seriesPageNo").value;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/SeriesPartial", {
        "seriesPageSize": seriesPageSize,
        "seriesPageNo": seriesPageNo,
        "genreId": genreId,
        "__RequestVerificationToken": token
    })
        .done(function (response) {
            $("#genreDetails").find("#seriesPartial").html(response);
            if (document.getElementById("partialModal")) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            alert(response.responseText);
        });
}