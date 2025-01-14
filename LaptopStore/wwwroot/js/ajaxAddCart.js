var contextPath = window.location.origin + window.location.pathname.substring(0, window.location.pathname.indexOf("/", 1));
function addToCart(productId) {
	$.ajax({
		url: window.location.origin + "/Cart/AddCart?productId=" + productId ,
		type: "GET",
		dataType: "json",
		success: function (response) {

			const notyf = new Notyf();
			notyf.open({
				type: "success",
				message: "thêm giỏ hàng thành công",
				position: {
					x: "center",
					y: "center",
				},
				duration: 2500,
				dismissible: true
			});

			const cartCountElement = document.getElementById('cartCount');
			const cartCount = response.quantityCart;  // Lấy số lượng giỏ hàng từ response

			if (cartCount > 0) {
				// Hiển thị số lượng giỏ hàng
				cartCountElement.innerText = cartCount;
				cartCountElement.style.display = 'inline';  // Hiển thị lại nếu số lượng > 0
			} else {
				// Ẩn nếu giỏ hàng trống
				cartCountElement.style.display = 'none';
			}


		},
		error: function (xhr, status, error) {
			console.error("Error loading section:", error);
		}
	});
}