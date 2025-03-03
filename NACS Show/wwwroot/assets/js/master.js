$(document).ready(function () {

$('.EditingFormTable').find('tr').addClass('row')
$('.EditingFormTable').find('td').addClass('col-12')

    //$(window).resize();

    responsiveTabs();
    responsiveMenu();

    /*mobile menu slide right to left*/
    $(".navicon").click(function () {
        $(".mobilenav").slideToggle("fast", function () {
        });

    });

    $(".nav-closex").click(function () {
        $(".mobilenav").fadeOut("fast", function () {
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

//title resizing
//>1650. 7rem, lineheight = 134
//@1650, 5rem, lineheight = 96
//< 1080, 2rem, lineheight = 96

$(document).ready(ResizeHeader);
//$(window).resize(ResizeHeader);

function ResizeHeader() {

    var $title = $(".page-title h1");

    var $lineheight = 38;

    if (jQuery(window).width() >= 1650) { $lineheight = 134; }
    else if (jQuery(window).width() >= 1080 && jQuery(window).width() < 1650) { $lineheight = 96; }
    else if (jQuery(window).width() < 1080) { $lineheight = 38; }

    var $lines = $title.height() / $lineheight;
    var $numlines = $lines.toPrecision(1);
    var $fontsize = "7rem";
    //alert($numlines);

    if (jQuery(window).width() >= 1650) {
        if (($numlines >= 1) && ($numlines < 3)) {
            $title.css("font-size", "7rem");
            $fontsize = "7rem";
        }
        else if (($numlines == 3 )) {
            $title.css("font-size", "5rem");
            $fontsize = "5rem";
        }
        else if (($numlines >= 4) && ($numlines <= 5)) {
            $title.css("font-size", "4rem");
            $fontsize = "4rem";
        }
        else if (($numlines > 5) && ($numlines <= 7)) {
            $title.css("font-size", "3.5rem");
            $fontsize = "3.5rem";
        }
    }
    if (jQuery(window).width() >= 1080 && jQuery(window).width() < 1650) {
        if (($numlines >= 1) && ($numlines < 3)) {
            $title.css("font-size", "7rem");
            $fontsize = "7rem";
        }
        else if (($numlines == 3)) {
            $title.css("font-size", "5rem");
            $fontsize = "5rem";
        }
        else if (($numlines >= 4) && ($numlines <= 7)) {
            $title.css("font-size", "3rem");
            $fontsize = "3rem";
        }
    }
    if (jQuery(window).width() >= 800 && jQuery(window).width() < 1080) {
        if ($numlines == 2) {
            $title.css("font-size", "2.5rem");
            $fontsize = "2.5rem";
        }
        if ($numlines == 3) {
            $title.css("font-size", "3.5rem");
            $fontsize = "3.5rem";
        }
        if ($numlines >= 4) {
            $title.css("font-size", "2.5rem");
            $fontsize = "2.5rem";
        }
    }
    if (jQuery(window).width() < 800) {
        if ($numlines > 3 && $numlines < 4)
        {
            $title.css("font-size", "1.5rem");
            $fontsize = "1.5rem";
        }
        else if ($numlines >= 4) {
            $title.css("font-size", "1rem");
            $fontsize = "1rem";
        }
    }

    //var $alert = "Line height: " + $lineheight + "\n";
    //$alert += "Lines: " + $numlines + "\n";
    //$alert += "Font size: " + $fontsize + "\n";
    //alert($alert);

    //}

    //var $numWords = $title.text().split(" ").length;
    //var $numChars = $title.text().length;
         
    //if (($numChars >= 1) && ($numChars < 20)) {
    //    $title.css("font-size", "7rem");
    //}
    //else if (($numChars >= 20) && ($numChars < 40)) {
    //    $title.css("font-size", "6rem");
    //}
    //else if (($numChars >= 40) && ($numChars < 60)) {
    //    $title.css("font-size", "5rem");
    //}
    //else {
    //    $title.css("font-size", "4rem");
    //}

}

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

function responsiveMenu() {
     if(jQuery(window).width() < 768){
    $('.ajax__tab_panel').addClass("inactive-accordion-section");
  }
  else {
    $('.ajax__tab_panel').removeClass("inactive-accordion-section");
  }
}

function responsiveTabs()
{
  $('.nacs-tabs').each(function() {
	var $this = $(this);
	var index = 0;
	$this.find('.ajax__tab_tab').each(function() {
		var tabTitle = $(this).html();
		$this.find('.ajax__tab_panel').eq(index).before('<h3 class="tab_drawer_heading">' + tabTitle + '</h3>');
		index++;
	});
    
    $this.find('.tab_drawer_heading').last().addClass("last-accordion-section");
	$this.find('.tab_drawer_heading').each(function(){
		$(this).addClass("PaneHeader");
	});
  });
  
  //$('.tab_drawer_heading').first().next('.ajax__tab_panel').addClass('active-accordion-section').removeClass('inactive-accordion-section').css('visibility', 'visible');
$('.tab_drawer_heading').first().addClass('active-accordion-header');
  
  $('.tab_drawer_heading').click(function() {
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

function accordionMobileNav() {
    jQuery('.mobileNav li.contains-children a').click(function () {
        if (jQuery(this).parent().hasClass('showMore')) {
            jQuery(this).parent().removeClass('showMore').addClass('showLess');
        }
        else {
            jQuery(this).parent().removeClass('showLess').addClass('showMore');            
        }
        jQuery(this).parent().find('ul.third-level').slideToggle(500);
    });
    jQuery('.mobileNav li.contains-children').addClass('showMore');
    jQuery('.mobileNav li.contains-children > a').removeAttr('href').css('cursor','pointer');
}

$('.slickcarousel').slick({
  infinite: true,
  slidesToShow: 3,
  slidesToScroll: 3
});