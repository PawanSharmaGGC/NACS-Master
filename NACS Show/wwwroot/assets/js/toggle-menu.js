//(function ($) {
$(document).ready(function() {  
  var $window = $(window);
  var $mobileMenus = $('.navigation-mobile');
  var allPanels = $(
    '.header-navigation-mobile .menu__item > .menu, .footer-navigation .menu .menu__item'
  );
//alert($window.width()); //BSM
  if ($window.width() < 960) {
   //alert($window.width()); //BSM
    $mobileMenus.hide();

    $('.button--toggle-mobile-menu').on('click tap', function () {
      var navigation = $(this).parent().next().find('.navigation-mobile');
      console.log(navigation);
      allPanels.stop().slideUp();
      navigation.stop().slideToggle();
    });
  }

  $window.on('resize', function () {
    if ($window.width() > 960) {
      $mobileMenus.removeClass('--is-closed');
      
    } else {
      $mobileMenus.addClass('--is-closed');
    }
  });
});//(jQuery);
