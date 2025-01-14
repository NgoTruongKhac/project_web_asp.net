const allCheck = document.getElementById("allCheck");
allCheck.addEventListener("change", (e) => {
	const isChecked = e.target.checked;
	document.querySelectorAll(".myCheckbox").forEach(checkbox => {
		checkbox.checked = isChecked;
	});
	totalPrice();
});

function totalPrice() {

	let total = 0;

	const checkboxes = document.querySelectorAll(".myCheckbox:checked");

	checkboxes.forEach(checkbox => {
		const row = checkbox.closest(".row");
		const price = parseInt(row.querySelector(".text-danger").textContent.replace(/[^\d]/g, ""));
		const quantity = parseInt(row.querySelector(".quantity").value);
		total += price * quantity;
	});

	document.querySelector(".sticky-bottom .text-danger").textContent = new Intl.NumberFormat("vi-VN").format(total) + " đ";
	document.getElementById("totalPriceInput").value = total;

}



document.querySelectorAll(".myCheckbox, .quantity").forEach(element => {
	element.addEventListener("change", totalPrice);
});