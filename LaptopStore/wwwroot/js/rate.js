const stars = document.querySelectorAll(".stars input");
const result = document.getElementById("rating-result");

const ratingTexts = {
	1: "Rất không hài lòng 😡",
	2: "Không hài lòng 😟",
	3: "Bình thường 😐",
	4: "Hài lòng 😊",
	5: "Rất hài lòng 😍",
};

stars.forEach((star) => {
	star.addEventListener("change", (e) => {
		const rating = e.target.value; // Lấy giá trị của sao được chọn
		result.textContent = ratingTexts[rating]; // Cập nhật text hiển thị
	});
});