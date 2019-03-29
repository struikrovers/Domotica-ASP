var em;

window.onload = function (event) {
    getEMValue("em_calc");
    // makes the overlays content render correctly
    // window location source: https://stackoverflow.com/a/3154568
    setTimeout(function () {
        if (window.location.href.match('default.aspx') || window.location.href.match('admin.aspx')) {
            document.getElementById('ContentPlaceHolder1_grid_overlay').style.display = "none";
            document.getElementById('ContentPlaceHolder1_grid_overlay').style.visibility = "visible";
        }
    }, 50);
    setTimeout(function () {
        if (window.location.href.match('Login.aspx')) {
            if (document.getElementById("ContentPlaceHolder1_PasswordInput").value != "") {
                document.getElementById("ContentPlaceHolder1_remember").checked = true;
            }
        }
    }, 50);
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
        document.getElementById('ContentPlaceHolder1_grid_overlay').style.display = "block";
    }
    //close all previously opened overlay_children
    for (i = 0; i < document.getElementsByClassName('overlay_child').length; i++) {
        document.getElementsByClassName('overlay_child')[i].style.display = "none";
    }
    document.getElementById(id).style.display = "inline-block";
    open_overlays.push(id);
    overlay_rect = document.getElementById(id).getClientRects();
    icon_width = document.getElementById("close_overlay_back").getClientRects()[0].width;
    document.getElementById("close_overlay_back").style.left = overlay_rect[0].x - icon_width/2 + "px";
    document.getElementById("close_overlay_icon").style.left = overlay_rect[0].right - icon_width / 2 + "px";
    document.getElementById("masterForm").style.overflowY = "hidden";
}

function close_overlay(e, force) {
    // get mouse position & check if it overlaps with the currently open overlay.
    x = e.clientX;
    y = e.clientY;

    overlay_rect = last(open_overlays);

    function closer() {
        if (overlay_rect != null) {
            // close the grid_overlay if there is only 1 overlay open at the time of closing.
            if (open_overlays.length == 1 || force == "close") {
                document.getElementById('ContentPlaceHolder1_grid_overlay').style.display = "none";
                open_overlays.pop();
                document.getElementById("masterForm").style.overflowY = "scroll";
            }
            // close the overlay_child that is currently open. remove it from open_overlays. show next latest overlay.
            else {
                document.getElementById(last(open_overlays)).style.display = "none";
                open_overlays.pop();
                document.getElementById(last(open_overlays)).style.display = "inline-block";
            }
        }
    }

    back_button = 'close_overlay_back';
    close_button = 'close_overlay_icon';
    if (force == "close" || force == "back")
    {
        closer();
        /*
        if (force != "close" && force != "back") {
            if (!click_overlap(x, y, back_button) && !click_overlap(x, y, close_button)) {
                closer();
            }
        }
        else {
            closer();
        }
        */
    }
}


function click_overlap(x, y, id) {
    if (id != null) {
        // returns false if the specified element is not overlapping
        rectBox = document.getElementById(id).getBoundingClientRect();
        if (x < rectBox.left || x > rectBox.right || y < rectBox.top || y > rectBox.bottom) {
            return false;
        }
        else {
            return true;
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
        else {
            //console.log("couldn't find " + selector);
        }
    }
}

function ChangeSettingsIcon(ParentID, left = "", right = "", top = "", bottom = "") {
    waitForElementToDisplay("#" + ParentID + "settings", 10, () => {
        var widget_settings = document.getElementById(ParentID.concat("settings"));
        if (left != "") { widget_settings.style.left = left; }
        if (right != "") { widget_settings.style.right = right; }
        if (top != "") { widget_settings.style.top = top; }
        if (bottom != "") { widget_settings.style.bottom = bottom; }
    }, 0);
}

function changeTextPlacement(ParentID, clientID) {
    setTimeout(function () {
        width_input = document.getElementById(clientID.concat("_Radio")).getBoundingClientRect().width;
        width_container = document.getElementById(ParentID.concat("grid_child_commenter")).getBoundingClientRect().width;
        width = width_container - (width_input + 20)
        para = document.getElementById(ParentID.concat("grid_child_comment"));
        para.style.width = width + "px";
        para.style.right = "5px";
        para.style.position = "absolute";
    }, 50);
}

function timeValidator(input) {
    btn = document.getElementById(input.id.replace("_timeInput", "_submitBTN"));
    var regex = /^([0-1][0-9]||[2][0-3]){1}:([0-5][0-9]){1}(\s-\s||-)(([0-2][0-9]){1}|([3][0-1]){1}|([0-9]){1}){1}$/g;
    if (!regex.test(input.value)) {
        input.className = "timeInputField";
        input.className += " bounce";
        btn.disabled = true;
    }
    else{
        input.className = "timeInputField";
        btn.disabled = false;
    }
}

function checkuserinput(ClientID) {
    extension = '_Range_Display';
    extension2 = '_Range_Input';
    if(document.getElementById(ClientID + '_Range_Display_Vert') != null){
        extension = '_Range_Display_Vert';
        extension2 = '_Range_Input_Vert';
    }
    numberInput = document.getElementById(ClientID + extension)
    if(parseInt(numberInput.value) > parseInt(numberInput.max)){
        numberInput.value = maxvalue
    }
    if(parseInt(numberInput.value) < parseInt(numberInput.min)){
        numberInput.value = minvalue
    }
    document.getElementById(ClientID + extension2).value = numberInput.value;
}

function fixValue(el){
    if(el.value > el.max){
        el.value = el.max;
    }
    if(el.value < el.min){
        el.value = el.min
    }
}

function setTime(el) {
    var d = new Date();
    if (el.value == "") {
        min = d.getMinutes();
        hour = d.getHours()
        hour_ = "";
        min_ = "";
        if(min < 10){
            min_ = "0";
        }
        if(hour < 10){
            hour = "0";
        }
        el.value =  hour_ + hour+ ":" + min_ + min + "-" + d.getDate();
    }
}

function OpenUpdater(){
    panel = document.getElementById('ContentPlaceHolder1_outputUpdatePanel')
    setTimeout(() => {
        panel.style.top = "1em";
    }, 500);
    setTimeout(() => {
        panel.style.top = "-10em";
    }, 3000);
}