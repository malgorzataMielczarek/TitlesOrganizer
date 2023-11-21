function AuthorsPagerClick(index) {
    document.getElementById("pageNo").value = index;
}

function SelectAuthorsSubmit(event) {
    $.ajax({
        url: $(event.target).attr('action'),
        type: 'POST',
        data: $(event.target).serialize(),
        success: function (response) {
            HandleSuccessResponse(response);
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function AddNewAuthor(event) {
    //var bookId = document.getElementById("bookId").value;
    var name = document.getElementById("newAuthorName").value;
    var lastName = document.getElementById("newAuthorLastName").value;
    //var sortBy = $('select[name="sortBy"]').val();
    //var pageSize = $('select[name="pageSize"]').val();
    //var pageNo = document.getElementById("pageNo").value;
    //var search = document.getElementById("searchString").value;
    //var token = $('input[name="__RequestVerificationToken"]').val();
    if (name.trim().length <= 0 && lastName.trim().length <= 0) {
        $("#newAuthorError").show();
        return;
    }

    HideNewAuthorError();
    $.ajax({
        url: "/Books/AddNewAuthor",
        type: 'PUT',
        data: $(document.forms.selectAuthors).serialize(),
        success: function (response) {
            LoadToModal(response);
            fhToastr.success("New author added");
        },
        error: function (response) {
            fhToastr.error(response.responseText);
        }
    });

    event.preventDefault();
}

function HideNewAuthorError() {
    $("#newAuthorError").hide();
}

function HandleSuccessResponse(response) {
    if (!Array.isArray(response) && response.startsWith("<form")) {
        LoadToModal(response);
    }
    else {
        $("#partialModal").modal('hide');
        var authors = '';
        if (response.length > 0) {
            for (var i = 0; i < response.length - 1; i++) {
                authors += response[i].description + ", ";
            }

            authors += response[response.length - 1].description;
            if (document.getElementById("authorsError")) {
                $("#authorsError").hide();
            }
        }

        document.getElementById("authors").innerText = authors;
    }
}

function LoadToModal(response) {
    document.querySelector("#partialModal .modal-header > .modal-title").innerText = "Select authors";
    $("#partialModal").find(".modal-body").html(response);
    $("#partialModal").modal('show');
}

function CloseModal() {
    document.getElementById("closeModal").value = true;
}