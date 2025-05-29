/* JavaScript Document */


function carouselReview(){
	jQuery('.gallery-carousel').owlCarousel({
		nav:true,
		margin:20,
		loop:true,
		autoWidth:true,
		items:4,
		autoplay:true,
		autoplayTimeout:1000,
		autoplayHoverPause:true,
		smartSpeed:2000,
		navText : ["<i class='fa fa-chevron-left'></i>","<i class='fa fa-chevron-right'></i>"]
	})
}


jQuery(window).on('load',function(){
	setTimeout(function(){
		carouselReview();
	}, 1000); 
});
/* Document .ready END */