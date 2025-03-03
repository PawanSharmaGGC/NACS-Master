let repositioned = false;

$(document).ready(function() {

    $(window).scroll(function() {
        let scrolled = $(window).scrollTop();
        let ad_wrap = $('#header-ad-wrapper');
        let ad_height = $('#header-ad').outerHeight(true);

        if (scrolled > ad_height) {
            // DON'T WANT TO RUN CODE EVERY PIXEL PAST 80
            if (repositioned != true) {
                repositioned = true;
                $(ad_wrap).animate({height: 0}, 250, 'linear');
            }
        } else if (scrolled < ad_height) {
            // ONLY RUN IF THE LOGO HAS BEEN REPOSITIONED. OTHERWISE IT WOULD CONTINUE TO RUN.
            if (repositioned == true) {
                // SET TO FALSE SO THAT PREVIOUS STATE IS ONLY TRUE ONCE.
                repositioned = false;
                $(ad_wrap).animate({height: ad_height}, 250, 'linear');
            }
        }
    });

});

let mobileNav = 'closed';

function toggle_mobile_menu() {
    let header_ht = $('#header-base').innerHeight();
    let nav = $('#mobile-nav-wrapper');
    let hamburger = $('.hamburger');
    $(".p-0").hide();
  
    if (mobileNav == 'closed') {
        $(nav).css('height', $(window).height() - header_ht);
        $(hamburger).addClass('open');
        mobileNav = 'open';
    } else {
        $(nav).css('height', '0');       
        $(hamburger).removeClass('open');        
        mobileNav = 'closed';
    }

}

$(document).ready(function () {
    AOS.init({
        offset: 100, // offset (in px) from the original trigger point
        delay: 100, // values from 0 to 3000, with step 50ms
        duration: 800, // values from 0 to 3000, with step 50ms
    });

    //adjust_page_gutter();
});

function adjust_page_gutter() {
    let ht = $('#header-base').innerHeight();
    let wh = $(document).height();
    let fh = $('footer').innerHeight();

    //$('#page').css('paddingTop', ht);
   // $('#detail-hero').css('top', ht);

    let min_ht = (wh - fh) > 750 ? wh - fh : 750;
    $('#page').css('minHeight', min_ht);
}

 $(document).ready(function () {
            AOS.init({
                offset: 100, // offset (in px) from the original trigger point
                delay: 100, // values from 0 to 3000, with step 50ms
                duration: 800, // values from 0 to 3000, with step 50ms
            });

            resize_square_blocks();
        });

        var waitForFinalEvent = (function () {
            var timers = {};
            return function (callback, ms, uniqueId) {
                if (!uniqueId) {
                    uniqueId = "Don't call this twice without a uniqueId";
                }
                if (timers[uniqueId]) {
                    clearTimeout (timers[uniqueId]);
                }
                timers[uniqueId] = setTimeout(callback, ms);
            };
        })();

        $(window).resize(function () {
            waitForFinalEvent(function() {
                resize_square_blocks();
            }, 500, 'square block sizing');
        });

        function resize_square_blocks() {
            if ($(window).width() > 991) {
                $('.section-block.__square').each(function(i, obj) {
                    $(obj).height($(obj).width() - 30);
                });
            }
        }


function toggle_search() {
    let nav = $('#secondary-nav');

    if (nav.hasClass('search-open')) {
        nav.removeClass('search-open');
        $('.search-trigger .material-icons').fadeOut(50);
        setTimeout(function(){ $('.search-trigger img').fadeIn(); }, 60);
    } else {
        nav.addClass('search-open');
        $('.search-trigger img').fadeOut(50);
        setTimeout(function(){ $('.search-trigger .material-icons').fadeIn(); }, 60);
    }
}

function toggle_mobile_search() {
    let nav = $('#mobile-nav-wrapper');

    if (nav.hasClass('search-open')) {
        nav.removeClass('search-open');
        $('.search-trigger .material-icons').fadeOut(50);
        setTimeout(function(){ $('.search-trigger img').fadeIn(); }, 60);
    } else {
        nav.addClass('search-open');
        $('.search-trigger img').fadeOut(50);
        setTimeout(function(){ $('.search-trigger .material-icons').fadeIn(); }, 60);
    }
}
