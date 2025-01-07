// Thời gian đếm ngược (5 phút)
var countDownTime = 5 * 60 * 1000;
var startTime = new Date().getTime();
var expected = startTime + countDownTime;

var x = setInterval(
	function () {
		var now = new Date().getTime();
		var distance = expected - now;

		var minutes = Math
			.floor((distance % (1000 * 60 * 60))
				/ (1000 * 60));
		var seconds = Math
			.floor((distance % (1000 * 60)) / 1000);

		document.getElementById("countdown").innerHTML = minutes
			+ " : " + seconds;

		if (distance < 0) {
			clearInterval(x);
			var countdownElement = document
				.getElementById("countdown");
			countdownElement.innerHTML = "Mã xác nhận đã hết hiệu lực.";
			countdownElement.style.fontSize = "20px";

		}
	}, 1000);