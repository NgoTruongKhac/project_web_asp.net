
function confirmLogout() {
    Swal.fire({
        title: 'Bạn có muốn đăng xuất?',

        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Đồng ý',
        cancelButtonText: 'Hủy bỏ'
    }).then((result) => {
        if (result.isConfirmed) {
            // Nếu người dùng chọn "Đồng ý", chuyển hướng đến servlet logout
            window.location.href = window.location.origin + '/Account/Logout';
        }
    });
}