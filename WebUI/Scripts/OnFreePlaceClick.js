function onFreePlaceClick(event) {
    var raiser = event.id;

    var data = raiser.split(" ");
    var carriageNumber = data[0];
    var placeNumber = data[1];

    var carriageHidden = document.getElementById("carriageNumberHiddenInput");
    var placeHidden = document.getElementById("placeNumberHiddenInput")

    carriageHidden.value = carriageNumber;
    placeHidden.value = placeNumber;

    var old = document.getElementsByClassName("checked");

    if (old.length != 0) {
        old[0].classList.remove("checked");
    }

    event.classList.add("checked");
}