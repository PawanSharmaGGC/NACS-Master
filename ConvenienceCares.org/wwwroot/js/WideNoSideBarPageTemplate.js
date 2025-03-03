$('.ajax__tab_header').each(function () {
    $(this).find('.ajax__tab_tab').each(function (index) {
        $(this).click(function () {
            window.location.hash = 'tabs-' + index;
        });
    });
});