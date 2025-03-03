

$(document).ready(function() {
//(function ($) {
  var $window = $(window);
  var allPanels = $(
    '.header-navigation-mobile .menu__item > .menu, .footer-navigation .menu .menu__item'
  );

  if ($window.width() < 960) {
    allPanels.hide();

    $('.footer-navigation .menu > .menu__heading').click(function () {
      allPanels.stop().slideUp();
      $(this).nextAll('.menu__item').stop().slideToggle();
      return false;
    });

    
    $('.header-navigation-mobile .menu__item:has(ul) > .menu__heading').click(
      function () {
        allPanels.stop().slideUp();
        $(this).nextAll('.menu').stop().slideToggle();
        return false;
      }
    );
  }

  $window.on('resize', function () {
    if ($window.width() > 960) {
      allPanels.show();
    } else {
      allPanels.hide();
    }
  });
});//(jQuery);

$(document).ready(function() {
  
//(function ($) {
  var allPanels = $('.accordion > .accordion--item').hide();

  $(
    '.accordion > .accordion--header > button, .accordion > .accordion--header > a'
  ).click(function () {
    
   
    var $item = $(this).parent().next();

    if ($item.is(':visible')) {
      $item.stop().slideUp();
    } else {
      $item.stop().slideDown(); // works
    }
    return false;
  });
   
 });//(jQuery);
   
