


function initCarousel(selector, options) {
    var $slider = $(selector).slick(options);
}
window.initCarousel = initCarousel;


function InitializeSlidercarousel(selector) {
    $(document).ready(function () {
        var $slider = $(selector).slick({
            autoplay: true,
            autoplaySpeed: 2000,
            adaptiveHeight: true,
            centerMode: true,
            dots: true,
            customPaging: function (slider, i) {
                return '<div class="PaginationDot-module__carousel_dots"><span class="PaginationDot-module__dot"></span></div>';
            },
            clone: true,
            infinite: true,
            centerPadding: '12%', // Adjust the padding to control how much the next/prev slides are shown
            slidesToShow: 1,
            focusOnSelect: true,
            prevArrow: '<p class="pe-5 pointer prevArrowBtn slick-disabled"><i class="fa-regular fa-angle-left fa-xl" aria-hidden="true"></i><span class="ps-1">Prev</span></p>',
            nextArrow: '<p class="pointer nextArrowBtn"><span class="pe-1">Next</span><i class="fa-regular fa-angle-right fa-xl " aria-hidden="true"></i></p>',
            // initialSlide :1,
            responsive: [
                {
                    breakpoint: 1240,
                    settings: {
                        slidesToShow: 2,
                        centerPadding: '40px'
                    }
                },
                {
                    breakpoint: 1008,
                    settings: {
                        slidesToShow: 1,
                        centerPadding: '40px'
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        centerPadding: '0px'
                    }
                }
            ],
        });

        $('.prevArrowBtn').off('click');
    });
}

function InitializeSlidercarousel(selector, cutomNav) {
    $(document).ready(function () {
        var $slider = $(selector).slick({
            autoplay: true,
            speed: 500,
            infinite: true,
            slidesToShow: 3,
            slidesToScroll: 1,
            responsive: [
                {
                    breakpoint: 1240,
                    settings: {
                        slidesToShow: 2,
                        centerPadding: '40px'
                    }
                },
                {
                    breakpoint: 1008,
                    settings: {
                        slidesToShow: 1,
                        centerPadding: '40px'
                    }
                },
                {
                    breakpoint: 800,
                    settings: {
                        slidesToShow: 1,
                        centerPadding: '0px'
                    }
                }
            ],
        });

    });
}

window.InitializeSlidercarousel = InitializeSlidercarousel;
function disbalePrevNext(selector) {

    $(selector).on('beforeChange', function (event, slick, currentSlide, nextSlide) {
        // Check if the current slide is the first slide
        if (nextSlide === 0) {
            $('.prevArrowBtn').addClass('slick-disabled');  // Disable the previous button
            $('.prevArrowBtn').off('click');
        } else {
            $('.prevArrowBtn').removeClass('slick-disabled'); // Enable the previous button
            $('.prevArrowBtn').on('click', function () {
                $(event.currentTarget).slick('slickPrev');
            });
        }

        // Check if the current slide is the last slide
        if (nextSlide === slick.slideCount - 1) {
            $('.nextArrowBtn').addClass('slick-disabled');  // Disable the previous button
            $('.nextArrowBtn').off('click');
        } else {
            $('.nextArrowBtn').removeClass('slick-disabled'); // Enable the previous button
            $('.nextArrowBtn').on('click', function () {
                $(event.currentTarget).slick('slickNext');
            });
        }
    });
}
window.disbalePrevNext = disbalePrevNext;


function handlePrevNext(selector,prevSelector, nextSelector, updateSelector) {

    var currSlide, nextSlide,slick;
    
    $(selector).on('beforeChange', function (event, slick, currentSlide, nextSlide) {
        currSlide = currentSlide;
        slick = slick;

        if (updateSelector) {
            $(updateSelector).removeClass('RecommendedCardCarouselStyle-module__curr_slider_card');

            $(slick.$slides[currentSlide]).addClass("RecommendedCardCarouselStyle-module__curr_slider_card");
        }

        // Check if the current slide is the first slide
        if (nextSlide === 0) {
            $(prevSelector).off('click');
        } else {
            $(prevSelector).on('click', function () {
                $(event.currentTarget).slick('slickPrev');
            });
        }

        // Check if the current slide is the last slide
        if (nextSlide === slick.slideCount - 1) {
            $(nextSelector).off('click');
        } else {
            $(nextSelector).on('click', function () {
                $(event.currentTarget).slick('slickNext');
            });
        }
    });
}
window.handlePrevNext = handlePrevNext;