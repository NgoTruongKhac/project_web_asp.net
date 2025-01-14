function changeImage() {


	const fileInputEdit = document.getElementById("file-upload-edit");
	const previewImageEdit = document.getElementById("preview-edit");

	fileInputEdit.addEventListener("change", function () {
		const file = fileInputEdit.files[0];
		console.log(file);

		if (file && file.type.startsWith("image/")) {
			const reader = new FileReader();

			reader.onload = function (e) {
				previewImageEdit.src = e.target.result;
				previewImageEdit.style.display = "block";
			};

			reader.readAsDataURL(file);
		} else {
			previewImageEdit.style.display = "none";
		}
	});
	const fileInputEditP = document.getElementById("file-upload-p-edit");
	const previewImageEditP = document.getElementById("preview-p-edit");

	fileInputEditP.addEventListener("change", function () {
		const file = fileInputEditP.files[0];
		console.log(file);

		if (file && file.type.startsWith("image/")) {
			const reader = new FileReader();

			reader.onload = function (e) {
				previewImageEditP.src = e.target.result;
				previewImageEditP.style.display = "block";
			};

			reader.readAsDataURL(file);
		} else {
			previewImageEditP.style.display = "none";
		}
	});

}


const fileInput = document.getElementById("file-upload-add");
const previewImage = document.getElementById("preview-add");

fileInput.addEventListener("change", function () {
	const file = fileInput.files[0];
	console.log(file);

	if (file && file.type.startsWith("image/")) {
		const reader = new FileReader();

		reader.onload = function (e) {
			previewImage.src = e.target.result;
			previewImage.style.display = "block";
		};

		reader.readAsDataURL(file);
	} else {
		previewImage.style.display = "none";
	}
});




const fileInputP = document.getElementById("file-upload-p");
const previewImageP = document.getElementById("preview-p");

fileInputP.addEventListener("change", function () {
	const file = fileInputP.files[0];
	console.log(file);

	if (file && file.type.startsWith("image/")) {
		const reader = new FileReader();

		reader.onload = function (e) {
			previewImageP.src = e.target.result;
			previewImageP.style.display = "block";
		};

		reader.readAsDataURL(file);
	} else {
		previewImageP.style.display = "none";
	}
});