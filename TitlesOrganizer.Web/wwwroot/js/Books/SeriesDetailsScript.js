function BooksPagerClick(index) {
    document.getElementById("booksPageNo").value = index;
    ReloadBooks();
}

function ReloadBooks() {
    var seriesId = document.getElementById("seriesId").value;
    var booksPageSize = document.getElementById("booksPageSize").value;
    var booksPageNo = document.getElementById("booksPageNo").value;
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.post("/Books/BooksPartial", {
        "booksPageSize": booksPageSize,
        "booksPageNo": booksPageNo,
        "seriesId": seriesId,
        "__RequestVerificationToken": token
    })
        .done(function (response) {
            $("#seriesDetails").find("#booksPartial").html(response);
            if (document.getElementById("partialModal")) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            alert(response.responseText);
        });
}