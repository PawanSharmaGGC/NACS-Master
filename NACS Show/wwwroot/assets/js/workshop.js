//Translations
var text = document.getElementById("_spanTranslation").innerHTML;
if (text.indexOf("s") > 0){
text = text.replace("","<div style='text-align:center'><span style='font-size:28px'><i class='fas fa-headphones-alt'></i></span><br>");
text = text.replace("Chinese (Mandarin)","Chinese (Mandarin)");
text = text.replace("Portuguese","Portuguese");
text = text.replace("Spanish", "Spanish");
text = text.replace(new RegExp(";",""),"&nbsp;&nbsp;");
text = text.replace("","</div>"); 
}
if (text.indexOf("fas") > 0)  
{
	document.getElementById("_spanTranslation").innerHTML = text;
}
else
{
	document.getElementById("_spanTranslation").innerHTML = "";
}

//Segment Tooltip

$(document).ready(function() {
     CreateTooltips();
});

function CreateTooltips() {
var viewportWidth = $(window).width();
var tipPlacement = (viewportWidth > 479) ? "right" : "bottom";
var txtLaunchers = "<img id='thumb' src='/Sessions/Education-Sessions/PublishingImages/iconLaunchers_75x75.png' style='width:75px!important'/><strong>Launchers</strong> are looking to dip their toe into new waters, desiring to master the basics.<br/><a href='/Sessions/Education-Sessions/Segments' target='_blank' style='color:#2bbcf6'>More about segments&nbsp;<i class='fa fa-chevron-double-right'></i></a>";
var txtExperimenters = "<img id='thumb' src='/Sessions/Education-Sessions/PublishingImages/iconExperimenters_75x75.png'style='width:75px' /><strong>Experimenters</strong> are still in testing mode, but looking to set their company apart within a given area.<br/><a href='/Sessions/Education-Sessions/Segments' target='_blank' style='color:#2bbcf6'>More about segments&nbsp;<i class='fa fa-chevron-double-right'></i></a>";
var txtTransformers = "<img id='thumb' src='/Sessions/Education-Sessions/PublishingImages/iconTransformers_75x75.png' style='width:75px'/><strong>Transformers</strong>&#39; focus within a given topic is on developing and implementing more complex processes, standards, and consistency.<br/><a href='/Sessions/Education-Sessions/Segments' target='_blank' style='color:#2bbcf6'>More about segments&nbsp;<i class='fa fa-chevron-double-right'></i></a>";

$.each($('.segment-tag.Launchers'), function(){ 
   $(this).tooltip({title: txtLaunchers, html: true, placement: tipPlacement, container:this}); 
  // $(this).tooltip('show');
});

$.each($('.segment-tag.Experimenters'), function(){ 
   $(this).tooltip({title: txtExperimenters, html: true, placement: tipPlacement, container:this});
   //$(this).tooltip('show');
});

$.each($('.segment-tag.Transformers'), function(){ 
   $(this).tooltip({title: txtTransformers, html: true, placement: tipPlacement, container:this}); 
   //$(this).tooltip('show');
});


}