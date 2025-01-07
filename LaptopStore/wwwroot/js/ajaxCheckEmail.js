var contextPath = window.location.origin + window.location.pathname.substring(0, window.location.pathname.indexOf("/", 1));
function checkEmailRegister() {

    var Email = $("#EmailOrPhone").val();
    $.ajax({
        url: contextPath + "/CheckEmailRegister",
        type: "POST",
        data: { Email: Email },

        success: function (response) {

            if (!response.sucess) {
                document.getElementById("errorCheckEmail").textContent = response.message;

                document.getElementById("errorCheckEmail").style.display = "block";

            }



        }
    });

}
function checkEmailLogin() {

    var Email = $("#EmailOrPhone").val();
    $.ajax({
        url: contextPath + "/CheckEmailLogin",
        type: "POST",
        data: { Email: Email },

        success: function (response) {

            if (!response.sucess) {
                document.getElementById("errorCheckEmail").textContent = response.message;

                document.getElementById("errorCheckEmail").style.display = "block";

            }



        }
    });

}

