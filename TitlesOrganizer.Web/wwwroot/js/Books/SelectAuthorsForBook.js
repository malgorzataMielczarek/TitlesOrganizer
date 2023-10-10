function PagerClick(index) {
    document.getElementById("pageNo").value = index;
    Submit();
}

function Submit() {
    document.forms[0].setAttribute("action", "SelectAuthorsForBook");
    document.forms[0].submit();
}

function AuthorSelectionChanged(checkbox) {
    var row = $(checkbox).closest("tr")[0];
    var id = row.querySelector('input[type="hidden"]').value;
    var fullName = row.querySelector('td:nth-of-type(2)').innerText;
    var otherBooks = row.querySelector('td:nth-of-type(3)').innerText;

    var selectedTable = document.querySelector("#selectedAuthors > tbody");
    if (selectedTable) {
        var selectedRow = $(selectedTable).find('tr > input[type="hidden"][value="' + id + '"]').parent()[0];

        if (checkbox.checked) {
            if (!selectedRow) {
                var row = document.createElement("tr");

                var idInput = document.createElement("INPUT");
                idInput.setAttribute("type", "hidden");
                idInput.setAttribute("value", id);
                var lastInput = selectedTable.querySelector('tr:last-of-type > input[type="hidden"]');
                var lastName = lastInput.getAttribute("name");
                var numberStart = lastName.indexOf('[') + 1;
                var numberEnd = lastName.indexOf(']');
                var number = lastName.substring(numberStart, numberEnd);
                var newNumber = (parseInt(number) + 1).toString();
                var name = lastName.replace(number, newNumber);
                var identifier = lastInput.id.replace(number, newNumber);
                idInput.setAttribute("name", name);
                idInput.id = identifier;
                row.appendChild(idInput);

                var authorTd = document.createElement("td");
                authorTd.innerText = fullName;
                row.appendChild(authorTd);

                var otherBooksTd = document.createElement("td");
                otherBooksTd.innerText = otherBooks;
                row.appendChild(otherBooksTd);

                selectedTable.appendChild(row);
            }
        }
        else {
            if (selectedRow) {
                var nextRows = $(selectedRow).nextAll("tr");
                selectedTable.removeChild(selectedRow);
                for (var i = 0; i < nextRows.length; i++) {
                    var thisInput = nextRows[i].querySelector('input[type="hidden"]');
                    var thisName = thisInput.getAttribute("name");
                    var thisNumberStart = thisName.indexOf("[") + 1;
                    var thisNumberEnd = thisName.indexOf("]");
                    var thisNumber = thisName.substring(thisNumberStart, thisNumberEnd);
                    var thisNewNumber = (parseInt(thisNumber) - 1).toString();
                    thisInput.setAttribute("name", thisName.replace(thisNumber, thisNewNumber));
                    thisInput.id = thisInput.id.replace(thisNumber, thisNewNumber);
                }
            }
        }
    }
}