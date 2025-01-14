document.addEventListener("DOMContentLoaded", function () {
    let multipleCardCarousel = document.querySelector("#carouselExampleControls");

    if (window.matchMedia("(min-width: 768px)").matches) {
        let carousel = new bootstrap.Carousel(multipleCardCarousel, {
            interval: false, // Disable automatic sliding
            wrap: false, // Prevent wrapping at the end
        });

        let carouselWidth = document.querySelector(".carousel-inner").scrollWidth;
        let cardWidth = document.querySelector(".carousel-item").offsetWidth;
        let scrollPosition = 0;

        document
            .querySelector("#carouselExampleControls .carousel-control-next")
            .addEventListener("click", function () {
                if (scrollPosition < carouselWidth - cardWidth * 4) {
                    scrollPosition += cardWidth;
                    document
                        .querySelector("#carouselExampleControls .carousel-inner")
                        .scroll({ left: scrollPosition, behavior: "smooth" });
                }
            });

        document
            .querySelector("#carouselExampleControls .carousel-control-prev")
            .addEventListener("click", function () {
                if (scrollPosition > 0) {
                    scrollPosition -= cardWidth;
                    document
                        .querySelector("#carouselExampleControls .carousel-inner")
                        .scroll({ left: scrollPosition, behavior: "smooth" });
                }
            });
    } else {
        multipleCardCarousel.classList.remove("slide");
        carouselInner.addEventListener("touchstart", (e) => {
            isDragging = true;
            startX = e.touches[0].pageX - carouselInner.offsetLeft;
            scrollLeft = carouselInner.scrollLeft;
        });

        carouselInner.addEventListener("touchend", () => {
            isDragging = false;
        });

        carouselInner.addEventListener("touchmove", (e) => {
            if (!isDragging) return;
            e.preventDefault();
            const x = e.touches[0].pageX - carouselInner.offsetLeft;
            const walk = (x - startX) * 1;
            carouselInner.scrollLeft = scrollLeft - walk;
        });
    }
});

const carouselInner = document.querySelector(".carousel-inner");

let isDragging = false;
let startX;
let scrollLeft;

carouselInner.addEventListener("mousedown", (e) => {
    isDragging = true;
    carouselInner.classList.add("active");
    startX = e.pageX - carouselInner.offsetLeft;
    scrollLeft = carouselInner.scrollLeft;
});

carouselInner.addEventListener("mouseleave", () => {
    isDragging = false;
    carouselInner.classList.remove("active");
});

carouselInner.addEventListener("mouseup", () => {
    isDragging = false;
    carouselInner.classList.remove("active");
});

carouselInner.addEventListener("mousemove", (e) => {
    if (!isDragging) return;
    e.preventDefault();
    const x = e.pageX - carouselInner.offsetLeft;
    const walk = (x - startX) * 1;
    carouselInner.scrollLeft = scrollLeft - walk;
});

let mouseDownTime = 0;
const clickThreshold = 200; // Set a threshold for a quick click (200 ms)

document
    .querySelectorAll("#carouselExampleControls .carousel-item")
    .forEach((item) => {
        // Record the time when mouse is pressed down
        item.addEventListener("mousedown", () => {
            mouseDownTime = new Date().getTime();
        });

        // Handle mouse up event to determine if it's a quick click
        item.addEventListener("mouseup", (event) => {
            const mouseUpTime = new Date().getTime();
            const clickDuration = mouseUpTime - mouseDownTime;

            // If the duration is longer than the threshold, prevent the click
            if (clickDuration > clickThreshold) {
                event.preventDefault();
            }
        });

        // Prevent click event if it was a hold
        item.addEventListener("click", (event) => {
            const clickDuration = new Date().getTime() - mouseDownTime;
            if (clickDuration > clickThreshold) {
                event.preventDefault();
            }
        });
    });
