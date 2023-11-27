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