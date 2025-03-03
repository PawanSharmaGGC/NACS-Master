/*
 * SimpleModal Basic Modal Dialog
 * http://www.ericmmartin.com/projects/simplemodal/
 * http://code.google.com/p/simplemodal/
 *
 * Copyright (c) 2010 Eric Martin - http://ericmmartin.com
 *
 * Licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 *
 * Revision: $Id: basic.js 243 2010-03-15 14:23:14Z emartin24 $
 *
 */

jQuery(function ($) {
	$('.basic-modal').click(function (e) {
	
		var linkID = $(this).find('a:first').attr('id');
		var windowID = linkID.replace('modal_link_','modal_window_');
	
		$('#' + windowID).modal();
		
		return false;
		
	});
});