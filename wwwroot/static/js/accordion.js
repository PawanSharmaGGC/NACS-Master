$(document).ready(function () {
    // Check if the event handler has already been applied to the container
    $('.faq-container').each(function () {
        var $faqContainer = $(this);

        // We add a data attribute to ensure the event handler is only applied once
        if (!$faqContainer.data('initialized')) {
            // Add data attribute to indicate initialization
            $faqContainer.data('initialized', true);

            // Use event delegation to bind to faq-title clicks
            $faqContainer.on('click', '.faq-title', function () {
                var $faqItem = $(this).closest('.faq-item');
                var $desc = $faqItem.find('.faq-desc');
                var $btn = $faqItem.find('.faq-btn');

                // If the clicked item is already open, close it
                if ($desc.hasClass('collapse') === false) {
                    // Collapse the clicked FAQ item
                    $desc.addClass('collapse');
                    $btn.text('+');
                } else {
                    // Collapse all FAQ items within this container first
                    $faqContainer.find('.faq-desc').addClass('collapse');
                    $faqContainer.find('.faq-btn').text('+');

                    // Then expand the clicked FAQ item
                    $desc.removeClass('collapse');
                    $btn.text('-');
                }
            });
        }
    });
});
