
function deleteUser(userId, page) {
	Swal.fire({
		title: 'Bạn có muốn xoá tài khoản này?',
		icon: 'question',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Đồng ý',
		cancelButtonText: 'Hủy bỏ'
	}).then((result) => {
		if (result.isConfirmed) {
			window.location.href = window.location.origin + "/Admin/DeleteUser?userId=" + userId + "&page=" + page;
		}
	});
}

function deleteProduct(productId, page) {
	Swal.fire({
		title: 'Bạn có muốn xoá sản phẩm này?',
		icon: 'question',
		showCancelButton: true,
		confirmButtonColor: '#3085d6',
		cancelButtonColor: '#d33',
		confirmButtonText: 'Đồng ý',
		cancelButtonText: 'Hủy bỏ'
	}).then((result) => {
		if (result.isConfirmed) {
			window.location.href = window.location.origin + "/Admin/DeleteProduct?productId=" + productId + "&page=" + page;
		}
	});
}