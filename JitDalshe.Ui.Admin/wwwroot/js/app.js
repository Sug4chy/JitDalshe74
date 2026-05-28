function showModal(modalID) {
    $(modalID).modal('show')
}

function hideModal(modalID) {
    $(modalID).modal('hide')
}

function initSwiper(selector) {
    return new Swiper(selector, {
        ally: true,
        slidesPerView: 1,
        spaceBetween: 100,
        navigation: {
            nextEl: selector + " .swiper-button-next",
            prevEl: selector + " .swiper-button-prev",
        },
        pagination: {
            el: selector + " .swiper-pagination",
            clickable: true,
            bulletActiveClass: "swiper-active-bullet swiper-pagination-bullet-active",
        },
        loop: true
    });
}