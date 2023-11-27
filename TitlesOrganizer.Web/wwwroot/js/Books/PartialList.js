function PartialPagerClick(index, entityType, partialType) {
    var pageNoId = partialType.toLowerCase();
    if (!partialType.endsWith('s')) {
        pageNoId += 's';
    }

    pageNoId += "PageNo";
    document.getElementById(pageNoId).value = index;
    ReloadPartial(entityType, partialType);
}

function ReloadPartial(entityType, partialType) {
    entityType = entityType.toLowerCase();
    var entityId = entityType + "Id";
    if (!partialType.endsWith('s')) {
        partialType += 's';
    }
    var lowPartial = partialType.toLowerCase();
    var pageSizeId = lowPartial + "PageSize";
    var pageNoId = lowPartial + "PageNo";
    var partial = partialType + "Partial";

    var id = null;
    if (document.getElementById(entityId)) {
        id = document.getElementById(entityId).value;
    }
    else if (document.getElementById("Id")) {
        id = document.getElementById("Id").value;
    }

    var pageSize = document.getElementById(pageSizeId).value;
    var pageNo = document.getElementById(pageNoId).value;
    var token = $('input[name="__RequestVerificationToken"]').val();
    var url = "/Books/" + partial;
    var data = {};
    data[pageSizeId] = pageSize;
    data[pageNoId] = pageNo
    data[entityId] = id;
    data["__RequestVerificationToken"] = token;
    $.post(url, data)
        .done(function (response) {
            var container = $("#" + lowPartial + "Partial");
            container.html(response);
            if (container.closest("#partialModal").length == 1) {
                $("#partialModal").modal('show');
            }
        })
        .fail(function (response) {
            fhToastr.error(response.responseText);
        });
}