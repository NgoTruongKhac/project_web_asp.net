
let timeout;
document.getElementById('searchInput').addEventListener('keyup', function () {
	clearTimeout(timeout);
	const searchText = this.value;
	const suggestionContainer = document.getElementById("listProductSuggest");
	if (searchText.trim() === "") {
		suggestionContainer.innerHTML = ""; // Xóa danh sách gợi ý
		suggestionContainer.style.display = "none"; // Ẩn container nếu cần
		return; // Không tiếp tục xử lý
	}
	if (searchText.length > 1) { // Chỉ tìm kiếm khi từ khóa dài hơn 2 ký tự
		timeout = setTimeout(() => {

			$.ajax({
				url: window.location.origin+"/Home/Search",
				method: 'GET',
				data: { keyWord: searchText },
				success: function (response) {


					suggestionContainer.innerHTML = response;
					suggestionContainer.style.display = "block"; // Hiển thị container


				},
				error: function () {
					console.error('Lỗi khi thực hiện yêu cầu AJAX.');
				}
			});
		}, 300); // Debounce 300ms


	}
});	