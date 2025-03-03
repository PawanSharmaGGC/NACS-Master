$(document).ready(function () {

$('.EditingFormTable').find('tr').addClass('row')
$('.EditingFormTable').find('td').addClass('col-12')

    $(window).resize();

    responsiveTabs();
    responsiveMenu();

    /*mobile menu slide right to left*/

  
    $( ".navicon" ).click(function() {
            $( ".mobilenav" ).slideToggle( "fast", function() {
            });

        });
        
        $( ".nav-closex" ).click(function() {
            $( ".mobilenav" ).fadeOut( "fast", function() {
            });
        });

    /*mobile menu slide top to bottom*/
    /*$(".hamburger").click(function (event) {
        event.preventDefault();
        $(".collapse").slideToggle("slow", function () {
        });
      });
    */

    updateNacsMagazineLinks();
});

function updateNacsMagazineLinks() {
    var allLinksToNM = $('a[href*="www.nacsmagazine.com"]');
    allLinksToNM.each(function () {
      if (!$(this).hasClass('no-login')) {
        var mylink = $(this);
        var redir = $(this).attr('href');
        mylink.click(function () {
            window.open('/Convenience.org/ApplicationPages/vendorssologin.aspx?ReturnURL=' + encodeURIComponent(redir), '_blank');
            return false;
        });
      }
    });
}

// Reorganize elements based on device//
$(window).resize(function () {
    if (jQuery(window).width() < 992) {
        /*reordering footer for mobile*/
        $('.footer-right').insertBefore('.footer-middle');
        $('.footer-left').insertAfter('.footer-middle');
        $('.register').insertBefore('#menuElem');
    }
    else {
        /*reordering footer for tablet and desktop*/
        $('.footer-left').insertBefore('.footer-middle');
        $('.footer-right').insertAfter('.footer-middle');
        $('.register').insertBefore('.exhibit');
    }
})

function responsiveMenu() {
    if (jQuery(window).width() < 992) {
      $('.left-nav-hamburger').remove();
      $('.left-nav-header').off('click');
      $('div.left-nav').removeClass('active');
      
      if ($('ul.left-nav').find('li').length > 0) {
      
      $('.left-nav-header').append('<div class="left-nav-hamburger"><i class="fal fa-bars"></i></div>');
$('ul.left-nav').hide();
$('.left-nav-header').click(function() {
$('ul.left-nav').slideToggle();
  $('div.left-nav').toggleClass('active');
  });
      }
        $('.ajax__tab_panel').addClass("inactive-accordion-section");
        $('#menuElem').children('li').each(function () {
            var linkTag = $(this).children('a');
            $(this).children('ul').prepend('<li><a href="' + $(linkTag).attr('href') + '">' + $(linkTag).text() + '</a></li>');
        });

        var qlLi = $('<li class="quick-links"><a href="#">Quick Links</a><ul /></li>');

        $('.quicklinks-hover-nav').find('li').find('a').each(function () {
            $(qlLi).find('ul').append('<li><a href="' + $(this).attr('href') + '">' + $(this).text() + '</a></li>');
        });

        $('#menuElem').prepend(qlLi);
        $('#menuElem').prepend('<li class="exhibitor-info"><a href="/Exhibit">Exhibitor Info</a></li>');
        $('#menuElem').find('ul').hide();
        $('#menuElem').children('li').each(function() {
          if ($(this).find('ul').length > 0) {
			$(this).addClass('contains-children');
            $(this).addClass('collapsed');
          }
        });
        $('#menuElem').children('li').find('a').click(function (e) {
            if ($(this).parent().find('ul').length > 0) {
                var toggle = false;
                if (!($(this).parent().find('ul').is(':visible'))) {
                    toggle = true;
                }
                $('#menuElem').children('li').find('ul').slideUp();
		$('#menuElem').children('li').addClass('collapsed');
		$('#menuElem').children('li').removeClass('expanded');
                if (toggle) {
                    $(this).parent().find('ul').slideToggle();
		    $(this).parent().removeClass('collapsed');
		    $(this).parent().addClass('expanded');
                }
		else {
		    $(this).parent().addClass('collapsed');
		    $(this).parent().removeClass('expanded');
		}
                e.preventDefault();
            }
        });
    }
    else {
      $('ul.left-nav').show();
      $('.left-nav-header').off('click');
      $('div.left-nav').removeClass('active');
      $('.left-nav-hamburger').remove();
        $('.ajax__tab_panel').removeClass("inactive-accordion-section");
        /*$('#menuElem').find('ul').show();
        $('#menuElem').children('li').each(function() {
        if ($(this).find('ul').length > 0) {
        console.log($(this).find('ul').find('li'));
            $(this).find('ul').find('li').first().remove();
        }
            }); */
    }
}

function responsiveTabs() {
    $('.nacs-tabs').each(function () {
        var $this = $(this);
        var index = 0;
        $this.find('.ajax__tab_tab').each(function () {
            var tabTitle = $(this).text();
            $this.find('.ajax__tab_panel').eq(index).before('<h3 class="tab_drawer_heading">' + tabTitle + '</h3>');
            index++;
        });

        $this.find('.tab_drawer_heading').last().addClass("last-accordion-section");
        $this.find('.tab_drawer_heading').each(function () {
            $(this).addClass("PaneHeader");
        });
    });

    $('.tab_drawer_heading').click(function () {
        var openSection = true;
        if ($(this).next('.ajax__tab_panel').hasClass('active-accordion-section')) {
            openSection = false;
        }
        $(this).parent().find('.ajax__tab_panel').removeClass('active-accordion-section').addClass('inactive-accordion-section');
        $(this).closest('.nacs-tabs').find('.tab_drawer_heading').removeClass('active-accordion-header');
        if (openSection) {
            $(this).next('.ajax__tab_panel').addClass('active-accordion-section').removeClass('inactive-accordion-section').css('visibility', 'visible');
            $(this).addClass('active-accordion-header');
        }
    });
}
