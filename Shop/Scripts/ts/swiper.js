$(document).ready(function() {


    var mainSwiper = new Swiper('.js-main-page-swiper', {
        direction: 'horizontal',
        loop: true//,
       // pagination: '.swiper-pagination',
       // nextButton: '.swiper-button-next',
       // prevButton: '.swiper-button-prev',
       // scrollbar: '.swiper-scrollbar'
    });

    var mainFavorite = new Swiper('.js-main-page-favorite-swiper', {
        slidesPerView: 4,
        spaceBetween: 30,
        grabCursor: true,
        breakpoints: {
            1200: {
                slidesPerView: 4
                ,spaceBetween: 30
            },
            992: {
                slidesPerView: 3.5
                ,spaceBetween: 30
            },
            768: {
                slidesPerView: 2.5
                ,spaceBetween: 30
            },
            520: {
                slidesPerView: 1.5
                ,spaceBetween: 30
            },
            350: {
                slidesPerView: 1
                , spaceBetween: 30
            }
        }

        // pagination: '.swiper-pagination',
        // nextButton: '.swiper-button-next',
        // prevButton: '.swiper-button-prev',
        //,scrollbar: '.swiper-scrollbar'
    });
    //js-card-page-swiper
    var cardSwiper = new Swiper('.js-card-page-swiper', {
        direction: 'horizontal',
        loop: true,
        // pagination: '.swiper-pagination',
         nextButton: '.swiper-button-next',
         prevButton: '.swiper-button-prev'//,
        // scrollbar: '.swiper-scrollbar'
    });

});