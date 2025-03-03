function responsiveToggle(id) {
    var spans = document.querySelectorAll(".NavbarStyle_bottom_sec_dropdown__rQv9q > span");
    var divIcons = document.querySelectorAll(".NavbarStyle_list_side_icon__3SN8U");

    var hdnResponsiveToggleEle = document.getElementById("hdnResponsiveToggleId");
    var responsiveToggleId = parseInt(hdnResponsiveToggleEle.value);
    if (id !== responsiveToggleId) {
        hdnResponsiveToggleEle.value = id;
        responsiveToggleId = id;
        spans.forEach(function (span) {
            toggleClass(span);
        });
        divIcons.forEach(function (divIcon) {
            toggleClass(divIcon);
        });

    } else {
        hdnResponsiveToggleEle.value = 0;
        responsiveToggleId = 0;
        spans.forEach(function (span) {
            toggleClass(span);
        });
        divIcons.forEach(function (divIcon) {
            toggleClass(divIcon);
        });
        
        var clickedItem = document.getElementsByClassName("NavbarStyle_mobile_sub_menu_div__etIzo")[0];
        toggleClass(clickedItem);
    }

    if (responsiveToggleId > 0) {
        console.log("calling...");
        const list = document.getElementById("small_screen_nav_menu");
        const list_div = document.getElementById("navbarSupprotedContent11");
        list.style.width = "20%";
        list_div.className = "d-flex";

        //show clicked item div
        var clickedItem = document.getElementsByClassName("NavbarStyle_mobile_sub_menu_div__etIzo")[0];
        toggleClass(clickedItem);
        var ele = clickedItem.querySelector("#div-" + id);
        if (ele) {
            ele.className = "d-block";
        }
    }
    if (responsiveToggleId === 0) {
        console.log("calling...");
        const list = document.getElementById("small_screen_nav_menu");
        const list_div = document.getElementById("navbarSupprotedContent11");
        list_div.className = "d-block";
        list_div.style.width = "100%";
        list.style.width = "100%";

        var ele = clickedItem.querySelector("#div-" + id);
        if (ele) {
            ele.className = "d-none";
        }
    }
}

function toggleClass(clsName) {
    if (clsName.classList.contains("d-none")) {
        clsName.classList.add("d-inline");
        clsName.classList.remove("d-none");
    }
    else {
        clsName.classList.remove("d-inline");
        clsName.classList.add("d-none");
    }
}