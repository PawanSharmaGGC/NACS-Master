function initializeSlickSlider(carouselId) {
    var hfTotalSlides = parseInt($("#hfTotalSlides-" + carouselId).val(), 10) || 0;
    var slidesToDisplay = Math.min(hfTotalSlides, 3);

    var $carousel = $('#testimonial-slider-' + carouselId);
    if ($carousel.hasClass('slick-initialized')) {
        //console.log("Slick already initialized for", carouselId);
        return;
    }

    $('#testimonial-slider-' + carouselId).slick({
        centerMode: true,
        centerPadding: '12%',
        slidesToShow: slidesToDisplay,
        infinite: true,
        speed: 500,
        prevArrow: $('.prev-btn[data-carousel-id="' + carouselId + '"]'),
        nextArrow: $('.next-btn[data-carousel-id="' + carouselId + '"]'),
        responsive: [
            {
                breakpoint: 480,
                settings: {
                    slidesToShow: 1,
                    slidesToScroll: 1
                }
            }
        ]
    });
}

function applySliderStyles(event, slick, carouselId) {
    if (!slick) {
        slick = $('#testimonial-slider-' + carouselId).slick('getSlick');
    }
    const currentSlide = slick.slickCurrentSlide();

    resetSliderClasses(carouselId);

    $('#testimonial-slider-' + carouselId + ' .slick-current').addClass('TestimonialCarouselStyle-module__curr_slider_card').removeClass('TestimonialCarouselStyle-module__slider_card');
    $('#testimonial-slider-' + carouselId + ' .slick-current .card-body').addClass('TestimonialCarouselStyle-module__active_slider_card').removeClass('TestimonialCarouselStyle-module__slider_card');
    $('#testimonial-slider-' + carouselId + ' .slick-current .card-body i').removeClass('fa-xl color-FFFFFF').addClass('fa-2xl color-0053A5');
    $('#testimonial-slider-' + carouselId + ' .slick-current .card-body .t-text').removeClass('color-FFFFFF').addClass('color-002569');
    $('#testimonial-slider-' + carouselId + ' .slick-current .card-body .t-author').removeClass('color-FFFFFF').addClass('color-000000');

    applyAdjacentSlideClasses(slick, carouselId);
}

function resetSliderClasses(carouselId) {
    $('#testimonial-slider-' + carouselId + ' .flex_card').removeClass('TestimonialCarouselStyle-module__curr_slider_card TestimonialCarouselStyle-module__prev_slider_card TestimonialCarouselStyle-module__next_slider_card');
    $('#testimonial-slider-' + carouselId + ' .flex_card .card-body').removeClass('TestimonialCarouselStyle-module__active_slider_card TestimonialCarouselStyle-module__slider_card');
    $('#testimonial-slider-' + carouselId + ' .flex_card .card-body').addClass('TestimonialCarouselStyle-module__slider_card');
    $('#testimonial-slider-' + carouselId + ' .flex_card .card-body i').removeClass('fa-2xl color-0053A5').addClass('fa-xl color-FFFFFF');
    $('#testimonial-slider-' + carouselId + ' .flex_card .card-body .t-text').removeClass('color-002569').addClass('color-FFFFFF');
    $('#testimonial-slider-' + carouselId + ' .flex_card .card-body .t-author').removeClass('color-000000').addClass('color-FFFFFF');
}

function applyAdjacentSlideClasses(slick, carouselId) {
    const slideCount = slick.slideCount;
    let centerIndex = slick.slickCurrentSlide();

    let prevSlide, nextSlide;
    if (centerIndex === 0) {
        prevSlide = centerIndex - 1;
        nextSlide = centerIndex + 1;
    } else if (centerIndex === slideCount - 1) {
        prevSlide = centerIndex - 1;
        nextSlide = slideCount;
    } else {
        prevSlide = (centerIndex - 1 + slideCount) % slideCount;
        nextSlide = (centerIndex + 1) % slideCount;
    }

    $('#testimonial-slider-' + carouselId + ' .flex_card[data-slick-index="' + prevSlide + '"]').addClass('TestimonialCarouselStyle-module__prev_slider_card').removeClass('TestimonialCarouselStyle-module__slider_card');
    $('#testimonial-slider-' + carouselId + ' .flex_card[data-slick-index="' + nextSlide + '"]').addClass('TestimonialCarouselStyle-module__next_slider_card').removeClass('TestimonialCarouselStyle-module__slider_card');

    var inActiveSlides = $('#testimonial-slider-' + carouselId + ' .flex_card').not('.TestimonialCarouselStyle-module__curr_slider_card, .TestimonialCarouselStyle-module__prev_slider_card, .TestimonialCarouselStyle-module__next_slider_card, .TestimonialCarouselStyle-module__slider_card');
    inActiveSlides.addClass('TestimonialCarouselStyle-module__slider_card');
}

$(document).ready(function () {
    $('[data-carousel-id]').each(function () {
        var carouselId = $(this).data('carousel-id');
        var $carousel = $('#testimonial-slider-' + carouselId);
        
        if ($carousel.length > 0) {  // Make sure the carousel element exists
            initializeSlickSlider(carouselId);
            $carousel.on('afterChange', function (event, slick) {
                applySliderStyles(event, slick, carouselId);
            });
            // Apply styles after initialization
            applySliderStyles(null, $carousel.slick('getSlick'), carouselId);
        } else {
            console.error('Carousel with ID ' + carouselId + ' not found!');
        }
    });
});
