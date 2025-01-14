var el = document.getElementById("wrapper");
var toggleButton = document.getElementById("menu-toggle");

toggleButton.onclick = function () {
    el.classList.toggle("toggled");
};

var currentUrl = window.location.pathname;
document.querySelectorAll('.list-group-item').forEach(function (link) {
    if (link.href.includes(currentUrl)) {
        link.classList.add('active');
    }
});