
const pageSize = 8;
var page = 1;
function moreLaptop() {
    $.ajax({
        url:  "Home/LoadMoreProducts",
        type: "get",
        dataType: "html",
        data: {
            pageSize: pageSize,
            page: page
        },
        success: function (respone) {
            var productMore = document.getElementById("productLaptop");
            productMore.innerHTML += respone;
            page++;
        },
    });
}