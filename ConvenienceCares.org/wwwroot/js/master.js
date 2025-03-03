$(document).ready(function (){

	// SlideFadeToggle

	$.fn.slideFadeToggle = function(speed, easing, callback) {
	    return this.animate({
	        opacity: 'toggle',
	        height: 'toggle'
	    }, speed, easing, callback);
	};
	
	
	// Mobile Nav Trigger
	
	$('#nav-trigger').click(function(){
	
		$(this).toggleClass('active');
		
		$('#nav-menu').find('> ul').slideFadeToggle(300, 'easeInOutQuad');
		
		
	
	});
	
	
	
	// Navigation Dropdowns
	
	$('.trigger').click(function(){
		
		var selectedTrigger = $(this);
		
		$(selectedTrigger).next().slideFadeToggle(300, 'easeInOutQuad');
		
		if ($(selectedTrigger).hasClass('active')){
			
			$(selectedTrigger).removeClass('active');
			
		} else {
		
			if ($('.trigger').hasClass('active')){
				
				$('.trigger.active').next().slideFadeToggle(300, 'easeInOutQuad');
				$('.trigger').removeClass('active');
				
			}
			
			$(selectedTrigger).addClass('active');
		
		}
	
	});
	
	
	
	// Subnavigation Dropdowns
	
	$('.subtrigger').click(function(){
		
		var selectedTrigger = $(this);
		
		$(selectedTrigger).next().slideFadeToggle(300, 'easeInOutQuad');
		
		if ($(selectedTrigger).hasClass('active')){
			
			$(selectedTrigger).removeClass('active');
			
		} else {
			
			if ($('.subtrigger').hasClass('active')){
				
				$('.subtrigger.active').next().slideFadeToggle(300, 'easeInOutQuad');
				$('.subtrigger').removeClass('active');
				
			} 
			
			$(selectedTrigger).addClass('active');
			
		}	
		
	});
	
	
	
	// Tabbed Programs
	
	$('.tab').click(function() {
		
		// Grabs rel attr
		var programTabRel = $(this).attr('rel');
		
		// Checks if tab is aleady selected
	    if ($('#' + programTabRel).hasClass('active')){
		    
	    } else {
		    
		    //Removes active tab, replaces with clicked tab
		    $('.tab.active').removeClass('active');
			$(this).addClass('active');
			
			//Slides out old content, slides in new content
			$('.tab-detail.active').slideFadeToggle(300, 'easeInOutQuad');
			$('.tab-content.active').slideFadeToggle(300, 'easeInOutQuad');
		    $('#' + programTabRel).slideFadeToggle(300, 'easeInOutQuad');
		    
		    //Toggles active class on active/inactive content
			$('.tab-detail.active').removeClass('active');
			$('.tab-content.active').removeClass('active');
		    $('#' + programTabRel).addClass('active');
	    }
	    
	});
	
	
	
	
	// Navigation Dropdowns
	
	$('#view-more-articles').click(function(){
		
		$('#view-more-articles').slideFadeToggle(300, 'easeInOutQuad');
		
		$('.collapsed').slideFadeToggle(300, 'easeInOutQuad');
		
	});


});