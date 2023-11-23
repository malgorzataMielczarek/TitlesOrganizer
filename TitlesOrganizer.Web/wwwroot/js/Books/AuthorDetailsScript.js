function Delete(id) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "/Books/AuthorDelete",
        type: 'DELETE',
        data: { "id": id, "__RequestVerificationToken": token },
        success: function (response) {
            fhToastr.success(response);
            setTimeout(() => {
                location.replace("/Books/Authors");
            }, 1500);
        },
        error: function (response) {
            fhToastr.error(response);
        }
    });
}

function BooksPagerClick(index) {
    document.getElementById("booksPageNo").value = index;
    ReloadBooks();
}

function ReloadBooks() {
    var authorId = document.getElementById("authorId").value;
    var booksPageSize = document.getElementById("booksPageSize").value;
    var booksPageNo = document.getElementById("booksPageNo").value;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/BooksPartial", {
        "booksPageSize": booksPageSize,
        "booksPageNo": booksPageNo,
        "authorId": authorId,
        "__RequestVerificationToken": token
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
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/GenresPartial", {
        "genresPageSize": genresPageSize,
        "genresPageNo": genresPageNo,
        "authorId": authorId,
        "__RequestVerificationToken": token
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
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/SeriesPartial", {
        "seriesPageSize": seriesPageSize,
        "seriesPageNo": seriesPageNo,
        "authorId": authorId,
        "__RequestVerificationToken": token
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