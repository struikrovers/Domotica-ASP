var em;

window.onload = function (event) {
    getEMValue("em_calc");

}

window.onresize = function (event) {

}


function getEMValue(id) {
    var div = document.getElementById(id);
    div.style.height = "1em";
    em = div.offsetHeight;
}