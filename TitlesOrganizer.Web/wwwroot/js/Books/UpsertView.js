function Check(elementId) {
    var authors = document.getElementById(elementId).innerText;
    var errorSelactor = "#" + elementId + "Error";
    if (authors.trim().length > 0) {
        $(errorSelactor).hide();
    }
    else {
        $(errorSelactor).show();
    }
}
function Select(entityT, forEntityT) {
    var token = $('input[name="__RequestVerificationToken"]').val();
    var title = document.getElementById("Title").value;
    var id = document.getElementById("Id").value;
    if (!entityT.endsWith('s')) {
        entityT += 's';
    }

    var url = "/Books/Select" + entityT + "For" + forEntityT;
    var jqxhr;
    if (id == '0') {
        jqxhr = $.ajax({
            method: "PUT",
            url: url,
            data: { "title": title, "__RequestVerificationToken": token },
            dataType: "html",
            success: function (response) {
                LoadToModal(response, "Select " + entityT.toLowerCase());
                document.getElementById("Id").value = document.getElementById(forEntityT.toLowerCase() + "Id").value;
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

function SubmitSelect(event, entityT) {
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