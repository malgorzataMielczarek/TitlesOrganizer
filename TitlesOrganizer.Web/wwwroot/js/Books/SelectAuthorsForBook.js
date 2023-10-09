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
        var authors = selectedTable.querySelectorAll("tr > td [textContent='" + fullName + "']");
        var author = undefined
        var books = undefined;

        if (authors.length > 0) {
            for (var i = 0; i < authors.length; i++) {
                if (authors[i].nextSibling.textContent == otherBooks) {
                    author = authors[i];
                    books = authors[i].nextSibling;
                    break;
                }
            }
        }

        if (selected === "true") {
            if (!(author && books)) {
                var row = document.createElement("tr");

                var authorTd = document.createElement("td");
                authorTd.textContent = fullName;
                row.appendChild(authorTd);

                var otherBooksTd = document.createElement("td");
                otherBooksTd.textContent = otherBooks;
                row.appendChild(otherBooksTd);

                selectedTable.appendChild(row);
            }
        }
        else {
            if (author && books) {
                selectedTable.removeChild(author.parentElement);
            }
        }
    }
}