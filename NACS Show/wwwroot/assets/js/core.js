//vars
let sessionCategories = {'category1':'Category 1','category2':'Category 2','category3':'Category 3'};
let sessionSegments = {'segment1':'Segment 1','segment2':'Segment 2'};

$(document).ready(function() {
  // if less than standard tablet width
  if ($(window).width() < 768) {
    $('.multi-ad-container, .sessions__cards, .speakers__cards').slick({dots: true, centerMode: true, arrows: false});
  }
  else {
  window.onscroll = function() {myFunction()};

    var header = document.getElementById("masthead");
    var stickyheader = document.getElementById("stickyheader");
    var tagline = document.getElementById("tagline"); //86
    var page = document.getElementById("page"); //36
    var navbar = document.getElementById("navbar");
    var sticky = stickyheader.offsetTop;
    
    function myFunction() {  
      
      if (window.pageYOffset > sticky) {
        stickyheader.classList.add("sticky");
        stickyheader.style.zIndex = '99999';
        stickyheader.style.background = 'var(--color-white-smoke)';
        header.style.padding = '0';
        tagline.style.display = 'none';
        page.style.marginTop = stickyheader.offsetHeight + 'px';
      } else {
        stickyheader.classList.remove("sticky");
        tagline.style.display = 'block';
        header.style.padding = '2rem 0';
        page.style.marginTop = '0px';
      }
    }
  }
});


$(document).ready(function(){
  $('.slick-carousel').slick({
    infinite: true,
    arrows: true,
    slidesToShow: 1,
    adaptiveHeight: true,
    responsive: [
      {
        breakpoint: 2000,
        settings: {
          dots: false,
          prevArrow: "<img src='/App_Themes/NACSShow2021Core/img/arrow-left.png' style='height:75px;width:25px;position:absolute;top:35%;left:-8%;' />",
          nextArrow: "<img src='/App_Themes/NACSShow2021Core/img/arrow-right.png' style='height:75px;width:25px;position:absolute;top:35%;right:-8%;' />",
        }
      },
      {
        breakpoint: 400,
        settings: {
          dots: true,
          arrows: false
        }
      },
    ]
  });

  $('.call-to-action__card p, .daily-articles__cards .card-content__desc').each(function() {
    //$clamp($(this)[0], {clamp: 5});
  });

  $('.daily-articles__cards .card-content__desc').each(function() {
    //$clamp($(this)[0], {clamp: 3});
  });

  $('.body-text-auto').each(function() {
    //$clamp($(this)[0], {clamp: 3});
  });

  $('.pricing-table').click(function() {
    window.location = "https://www.nacsshow.com/register/start";
  });
      
      $("#save300").hover(function() {
    $(this).css('cursor','pointer');
});
      
   $('#save300').click(function() {
    window.location = "https://www.nacsshow.com/Register";
  })
      
   

 
});

// manage favorited
function getFavoritesFromStore() {
  let item = localStorage.getItem('favorited');

  if(item === null || item === "") {
    return [];
  } else {
    return item.split(',');
  }
}

function addFavorite(id) {
  let favoritedCardIds = getFavoritesFromStore();

  if(favoritedCardIds.indexOf(id) === -1) {
    favoritedCardIds.push(id);
  }

  localStorage.setItem('favorited', favoritedCardIds);
}

function removeFavorite(id) {
  let favoritedCardIds = getFavoritesFromStore();

  favoritedCardIds = favoritedCardIds.filter(function(item) {
    return item !== id
  });

  localStorage.setItem('favorited', favoritedCardIds)
}

function initFavorites() {
  // Favorite Button Click
  $('.btn-fav').click(function() {
    let _this = $(this);
    let dataFavId = _this.closest('div[data-fav-id]').attr('data-fav-id');
    let isActive = _this.hasClass('active');


    _this.toggleClass('active');
    _this.find('.icon-heart').toggleClass('icon-heart--selected');

    switch(isActive) {
      case true:
        removeFavorite(dataFavId);
        break;

      case false:
        addFavorite(dataFavId);
        break;
    }
  });

  let favoritedCardIds = getFavoritesFromStore();

  $.each(favoritedCardIds, function(k,v) {
    let _btn = $('[data-fav-id='+v+']').find('.btn-fav');
    let _icon = $('[data-fav-id='+v+']').find('.icon-heart');

    _btn.addClass('active');
    _icon.toggleClass('icon-heart--selected');

    let _parent = $('[data-fav-id='+v+']').parent().clone();

    $('.my-favorites .education-sessions__cards').append(_parent);

  });

  if($('.my-favorites .education-sessions__cards > div').length !== 0) {
    $('.my-favorites').removeClass('hidden');
  }
}

if($('[data-jplist-control]').length > 0) {
  initFavorites();
  jplist.init();
}

// Filters and Pagination
$('.filters button, .pagination button').click(function() {
  initFavorites();
  labelFilters();
});

$('.filters select').change(function() {
  labelFilters();
});

// Toggle Button
$('.button--dropdown').click(function(e) {
  e.preventDefault();

  $(this).toggleClass('active');
  $(this).toggleClass('button-pill-light button-pill-outline');

  $('.banner__reveal').slideToggle(300);
});

$('.reveal__close').click(function(e) {
  e.preventDefault();

  $('.button--dropdown').trigger('click');
});

// Sessions categories and segments
function labelFilters() {
  $('.education-sessions__cards .card-section__card').each(function() {
    let _this = $(this);
    let _classes = _this[0].classList;
    let categoryKeys = Object.keys(sessionCategories);
    let segmentKeys = Object.keys(sessionSegments);

    $.each(_classes, function(k,v) {
      let _actions = _this.find('.card-actions');

      if(categoryKeys.includes(v)) {
        if(_actions.find('.'+v).length === 0) {
          _actions.prepend('<div class="link--pill btn-filter '+v+'">'+sessionCategories[v]+'</div>')
        }
      }

      if(segmentKeys.includes(v)) {
        if(_actions.find('.'+v).length === 0) {
          _actions.find('.btn-fav').before('<div class="link--pill btn-filter '+v+'">'+sessionSegments[v]+'</div>')
        }
      }
    });
  });
}

labelFilters();

