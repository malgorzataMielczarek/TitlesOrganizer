function PagerClick(index) {
    document.getElementById("pageNo").value = index;
    Submit();
}

function Submit() {
    document.forms[0].setAttribute("action", "SelectAuthorsForBook");
    document.forms[0].submit();
}

function AuthorSelectionChanged(selected, fullName, otherBooks) {
    var selectedTable = document.querySelector("#selectedAuthors > tbody");
    if (selectedTable) {
        var authors = $(selectedTable).find('tr > td > label:contains("' + fullName + '")');
        var author = undefined
        var booksTd = undefined;

        if (authors.length > 0) {
            for (var i = 0; i < authors.length; i++) {
                booksTd = authors[i].parentElement.nextElementSibling;
                if (booksTd.querySelector("label").innerText == otherBooks) {
                    author = authors[i];
                    break;
                }

                booksTd = undefined;
            }
        }

        if (selected) {
            if (!(author && booksTd)) {
                var row = document.createElement("tr");

                var authorTd = document.createElement("td");
                var authorLbl = document.createElement("label");
                authorLbl.innerText = fullName;
                authorTd.appendChild(authorLbl);
                row.appendChild(authorTd);

                var otherBooksTd = document.createElement("td");
                var otherBooksLbl = document.createElement("label");
                otherBooksLbl.innerText = otherBooks;
                otherBooksTd.appendChild(otherBooksLbl);
                row.appendChild(otherBooksTd);

                selectedTable.appendChild(row);
            }
        }
        else {
            if (author && booksTd) {
                selectedTable.removeChild(booksTd.parentElement);
            }
        }
    }
}