function showModal(modalID) {
    $(modalID).modal('show')
}

function hideModal(modalID) {
    $(modalID).modal('hide')
}

function initSwiper() {
    return new Swiper(".mySwiper", {
        ally: true,
        slidesPerView: 1,
        spaceBetween: 100,
        navigation: {
            nextEl: ".swiper-button-next",
            prevEl: ".swiper-button-prev",
        },
        pagination: {
            el: ".swiper-pagination",
            clickable: true,
            bulletActiveClass: "swiper-active-bullet swiper-pagination-bullet-active",
        },
        loop: true
    });
}