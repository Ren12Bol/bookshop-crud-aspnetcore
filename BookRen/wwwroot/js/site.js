function makeUpdateQntBtnVisible(numberItem) {
    document.getElementById("qnt-update-" + numberItem).style.display = "inherit";
}

function toggle(source) {
    let checkboxes = document.querySelectorAll('input[type="checkbox"]');
    let firstCheckbox = document.getElementById("firstCheckbox");
    let n = 0;

    if (source == firstCheckbox) {
        for (var i = 0; i < checkboxes.length; i++) {
            if (checkboxes[i] != source) {
                checkboxes[i].checked = source.checked;
            }
        }
    }
    else {
        let checkboxesArr = Array.from(checkboxes);
        checkboxesArr.splice(0, 1);

        if (checkboxesArr.every(i => i.checked == true)) {
            firstCheckbox.checked = true;
        }
        else {
            firstCheckbox.checked = false;
        }
    }

    showSelectAllBtn();
}

function showSelectAllBtn() {
    let checkboxes = document.querySelectorAll('input[type="checkbox"]');
    let hiddenBtn = document.getElementById("delete-all-btn");

    for (var i = 0; i < checkboxes.length; i++) {
        if (checkboxes[i].checked == true) {
            hiddenBtn.style.display = "inherit";
            break;
        }
        else {
            hiddenBtn.style.display = "none";
        }
    }
}