var contextPath = window.location.origin + window.location.pathname.substring(0, window.location.pathname.indexOf("/", 1));
function requireLogin() {
	Swal.fire({
		title: 'Bạn cần đăng nhập?',

		icon: 'warning',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Đăng nhập',
		cancelButtonText: 'Hủy'
	}).then((result) => {
		if (result.isConfirmed) {
			// Nếu người dùng chọn "Đồng ý", chuyển hướng đến servlet logout


			window.location.href=contextPath + "/Account/Login";

		}
	});


}