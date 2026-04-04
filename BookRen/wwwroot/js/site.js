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

function selectItems() {
    let selectedItemsIds = document.getElementsByClassName("selected-item");
    let idsArr = '';

    for (var i = 0; i < selectedItemsIds.length; i++) {
        if (selectedItemsIds[i].checked == true) {
            idsArr += ',' + selectedItemsIds[i].value;
        }
    }

    document.getElementById("selected-ids").value = idsArr;
}


//local cart storing for unauthorized users

userStorage = window.localStorage;

function createLocalCart() {

    let cart = {};

    cart.items = [];

    localStorage.setItem("cart", JSON.stringify(cart));
}

function addItemsToLocalCart(id) {

    if (localStorage.getItem("cart") === null) {
        createLocalCart();
    }

    let item = {};

    item.Id = Math.random();

    item.Book = id;

    item.Quantity = 1;

    if (localStorage.getItem("cart")) {
        let cart = JSON.parse(localStorage.getItem("cart"));

        cart.Items.push(item);

        localStorage.setItem("cart", JSON.stringify(cart));
    }

    alert('Book is added to your cart!');
}

//getting items from the cart

let localCart = document.getElementById("localCart");

function showItemsFromLocalCart() {
    if (localStorage.getItem("cart") !== null) {
        let cart = JSON.parse(localStorage.getItem("cart"));

        const url = "/Cart/UserLocalCart/" + new URLSearchParams(cart);
        fetch(url)
            .then((response) => {
                if (!response.ok) {
                    throw new Error(`HTTP error ${response.status}`);
                }
                return response.text();
            })
            .then((data) => {
                console.log(data);
            });
    }
}

localCart.addEventListener("load", showItemsFromLocalCart);
