var em;

window.onload = function (event) {
    getEMValue("em_calc");

}

window.onresize = function (event) {

}

// https://stackoverflow.com/a/12675966
function last(array) {
    return array[array.length - 1];
}

function getEMValue(id) {
    var div = document.getElementById(id);
    div.style.height = "1em";
    em = div.offsetHeight;
}

/* 
    open overlay's are stored by id in the open_overlays array. 
    close_overlay will close the latest openend overlay until there are no more open overlays and it will close the overlay_parent
 */

var open_overlays = [];

function open_overlay(e, id) {
    if (open_overlays.length == 0) {
        document.getElementById('grid_overlay').style.display = "block";
    }
    //close all previously opened overlay_children
    for (i = 0; i < document.getElementsByClassName('overlay_child').length; i++) {
        document.getElementsByClassName('overlay_child')[i].style.display = "none";
    }
    document.getElementById(id).style.display = "inline-block";
    open_overlays.push(id);
}

function close_overlay(e, force) {
    // get mouse position & check if it overlaps with the currently open overlay.
    x = e.clientX;
    y = e.clientY;
    // prevents js erros. when closing the last overlay it is called twice?
    if (force == false && open_overlays.length != 0) {
        overlay_rect = document.getElementById(last(open_overlays)).getBoundingClientRect();
    }
    else {
        overlay_rect = "";
    }
    if (force == true || force == "back" || x < overlay_rect.left || x > overlay_rect.right || y < overlay_rect.top || y > overlay_rect.bottom )
    {
        // close the grid_overlay if there is only 1 overlay open at the time of closing.
        if (open_overlays.length == 1 || force == true) {
            document.getElementById('grid_overlay').style.display = "none";
            open_overlays.pop();
        }
        // close the overlay_child that is currently open. remove it from open_overlays. show next latest overlay.
        else {
            document.getElementById(last(open_overlays)).style.display = "none";
            open_overlays.pop();
            document.getElementById(last(open_overlays)).style.display = "inline-block";
        }
    }
}


// NOTE: https://stackoverflow.com/a/29754070
function waitForElementToDisplay(selector, time, fun, loops) {
    if (document.querySelector(selector) != null) {
        fun();
        return;
    }
    else {
        if (loops < 5) {
            setTimeout(function () {
                loops++;
                waitForElementToDisplay(selector, time, fun, loops);
            }, time);
        }
    }
}