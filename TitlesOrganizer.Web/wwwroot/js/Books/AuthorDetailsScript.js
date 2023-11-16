function BooksPagerClick(index) {
    document.getElementById("booksPageNo").value = index;
    ReloadBooks();
}

function ReloadBooks() {
    var authorId = document.getElementById("authorId").value;
    var booksPageSize = document.getElementById("booksPageSize").value;
    var booksPageNo = document.getElementById("booksPageNo").value;
    $.post("/Books/BooksPartial", {
        "booksPageSize": booksPageSize,
        "booksPageNo": booksPageNo,
        "authorId": authorId
    })
        .done(function (response) {
            $("#authorDetails").find("#booksPartial").html(response);
            if (document.getElementById("partialModal")) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            alert(response.responseText);
        });
}

function GenresPagerClick(index) {
    document.getElementById("genresPageNo").value = index;
    ReloadGenres();
}

function ReloadGenres() {
    var authorId = document.getElementById("authorId").value;
    var genresPageSize = document.getElementById("genresPageSize").value;
    var genresPageNo = document.getElementById("genresPageNo").value;
    $.post("/Books/GenresPartial", {
        "genresPageSize": genresPageSize,
        "genresPageNo": genresPageNo,
        "authorId": authorId
    })
        .done(function (response) {
            $("#authorDetails").find("#genresPartial").html(response);
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
    var authorId = document.getElementById("authorId").value;
    var seriesPageSize = document.getElementById("seriesPageSize").value;
    var seriesPageNo = document.getElementById("seriesPageNo").value;
    $.post("/Books/SeriesPartial", {
        "seriesPageSize": seriesPageSize,
        "seriesPageNo": seriesPageNo,
        "authorId": authorId
    })
        .done(function (response) {
            $("#authorDetails").find("#seriesPartial").html(response);
            if (document.getElementById("partialModal")) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            alert(response.responseText);
        });
}