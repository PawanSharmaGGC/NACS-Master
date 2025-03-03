function initializeSlickSlider_DeepDive(carouselId) {
    var hfTotalSlides = parseInt($("#hfTotalSlides-" + carouselId).val(), 10) || 0;

    var $carousel = $('#sliderContainer-' + carouselId);
    if ($carousel.hasClass('slick-initialized')) {
        $carousel.slick('unslick');
    }

    $('#sliderContainer-' + carouselId).slick({
        centerMode: hfTotalSlides > 3,
        centerPadding: '60px',
        slidesToShow: 3,
        infinite: true,
        slidesToScroll: 1,
        speed: 500,
        prevArrow: $('.prev-btn[data-deepdive-carousel-id="' + carouselId + '"]'),
        nextArrow: $('.next-btn[data-deepdive-carousel-id="' + carouselId + '"]'),
        responsive: [
            {
                breakpoint: 480,
                settings: {
                    centerMode: true,
                    slidesToShow: 1,
                }
            }
        ]
    });
}

function applySliderStyles_DeepDive(event, slick, carouselId) {
    if (!slick) {
        slick = $('#sliderContainer-' + carouselId).slick('getSlick');
    }
    const currentSlide = slick.slickCurrentSlide();

    resetSliderClasses_DeepDive(carouselId);
    applyAdjacentSlideClasses_DeepDive(slick, carouselId);
}

function resetSliderClasses_DeepDive(carouselId) {
    $('#sliderContainer-' + carouselId + ' .slick-slide').find('.card-body').addClass('bg-296DC1 DeepDiveStyle-module__slider_card_body').removeClass('bg-FFFFFF DeepDiveStyle-module__slider_active_card_body');
    $('#sliderContainer-' + carouselId + ' .slick-active').find('.card-body').addClass('bg-FFFFFF').removeClass('bg-296DC1');
    $('#sliderContainer-' + carouselId + ' .slick-slide').find('.text-desc').addClass('fs-5').removeClass('fs-3');

    var hfTotalSlides = parseInt($("#hfTotalSlides-" + carouselId).val(), 10) || 0;
    var screenWidth = $(window).width();
    if (hfTotalSlides > 3 || screenWidth <= 480) {
        $('#sliderContainer-' + carouselId + ' .slick-current').find('.card-body').addClass('DeepDiveStyle-module__slider_active_card_body').removeClass('DeepDiveStyle-module__slider_card_body');
        $('#sliderContainer-' + carouselId + ' .slick-current').find('.text-desc').addClass('fs-3').removeClass('fs-5');
    }
}

function applyAdjacentSlideClasses_DeepDive(slick, carouselId) {
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
}

$(document).ready(function () {
    $('[data-deepdive-carousel-id]').each(function () {
        var carouselId = $(this).data('deepdive-carousel-id');
        var $carousel = $('#sliderContainer-' + carouselId);

        if ($carousel.length > 0) {  // Make sure the carousel element exists
            initializeSlickSlider_DeepDive(carouselId);
            $carousel.on('afterChange', function (event, slick) {
                applySliderStyles_DeepDive(event, slick, carouselId);
            });
            // Apply styles after initialization
            applySliderStyles_DeepDive(null, $carousel.slick('getSlick'), carouselId);
        } else {
            console.error('Slider with ID ' + carouselId + ' not found!');
        }
    });



    // Handle anchor tag clicks
    $('.DeepDiveStyle-module__dive_nav_link').click(function (e) {
        e.preventDefault(); // Prevent default anchor behavior

        // Remove 'active' class from all tags
        $('.DeepDiveStyle-module__dive_nav_link').removeClass('active');

        // Add 'active' class to the clicked tag
        $(this).addClass('active');

        // Get data from the clicked link
        var tagId = $(this).data('tag-id');
        var topN = $('#hfTopN').val();
        var carouselId = $('#hfUniqueId').val();
        var cardCTAText = $('#hfCardCTAText').val();

        // Make the Ajax request to get the cards based on the tag
        $.ajax({
            url: '/DeepDiveWidget/GetCardsByTag/' + tagId,
            type: 'GET',
            data: { topN: topN },
            success: function (response) {
                if (response.success) {

                    // Remove and clear any old Slick instance
                    if ($('#sliderContainer-' + carouselId).hasClass('slick-initialized')) {
                        $('#sliderContainer-' + carouselId).slick('unslick');
                        //console.log("Slick instance destroyed.");
                    }
                    $('#sliderContainer-' + carouselId).empty();


                    // Loop through the new cards and append them to the container
                    $.each(response.cards, function (index, cardItem) {

                        var itemLink = cardItem.itemPageUrl !== "#"
                            ? `<a href="${cardItem.itemPageUrl}" class="pointer text-decoration-none">
                                        <span class="me-2 color-0053A5">${cardCTAText}</span>
                                        <i class="fa-regular fa-arrow-right color-0053A5"></i>
                                    </a>`
                            : '';  // Empty string if ItemPageUrl is "#"


                        var cardHtml = `
                            <div class="DeepDiveStyle-module__flex-card border border-0 card">
                                <div class="p-4 mb-3 card-body">
                                    <div>
                                                <div class="text-start mb-4 border_left_blue EyebrowTitleStyle-module__eyebrow">
                                                    <span class="ps-4 text-uppercase font-monospace text-primary">${cardItem.title}</span></div>
                                            </div>

                                    <div class="p-1 text-start text-desc">
                                        <span class="color-0053A5">${cardItem.description}</span>
                                    </div>
                                    <div class="d-flex justify-content-between">
                                        <div class="fs-5 mt-3">
                                        ${itemLink}
                                        </div>
                                        <div class="align-self-end opacity-50">
                                            <img src="${cardItem.image}" alt="${cardItem.imageAltText}">
                                        </div>
                                    </div>
                                </div>
                            </div>
                        `;
                        $('#sliderContainer-' + carouselId).append(cardHtml);
                    });


                    // Update the hidden field with the new total slide count
                    var totalSlides = response.cards.length;
                    $('#hfTotalSlides-' + carouselId).val(totalSlides);

                    if (totalSlides > 0) {

                        // Re-initialize Slick slider after appending the new content
                        initializeSlickSlider_DeepDive(carouselId);

                        // Reapply styles if necessary
                        applySliderStyles_DeepDive(null, $('#sliderContainer-' + carouselId).slick('getSlick'), carouselId);
                    }

                }
            },
            error: function (error) {
                console.error('Error:', error);
            }
        });
    });
});

