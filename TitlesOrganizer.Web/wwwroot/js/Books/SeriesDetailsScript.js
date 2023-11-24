function Delete(id) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    $.ajax({
        url: "/Books/SeriesDelete",
        type: 'DELETE',
        data: { "id": id, "__RequestVerificationToken": token },
        success: function (response) {
            fhToastr.success(response);
            setTimeout(() => {
                location.replace("/Books/Series");
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
            fhToastr.error(response.responseText);
        });
}