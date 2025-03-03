
document.addEventListener("DOMContentLoaded", function () {
    const bioCollapse = document.getElementById("collapseExample11");
    const readBioBtn = document.getElementById("first-collapse");
    const collapseIcon = document.getElementById("collapse-icon");

    readBioBtn.addEventListener("click", function () {
        const isExpanded = bioCollapse.classList.contains("show");
        if (isExpanded) {
            bioCollapse.classList.remove("show");
            collapseIcon.textContent = "Read Bio +";
            readBioBtn.style.borderRadius = "0px 0px 18px 0px";
        } else {
            bioCollapse.classList.add("show");
            collapseIcon.textContent = "Read Bio -";
            readBioBtn.style.borderRadius = "0px 0px 0px 0px";
        }
    });
});