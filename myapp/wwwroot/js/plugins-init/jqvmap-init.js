(function($) {
    "use strict" 


 var dzVectorMap = function(){
	
	var screenWidth = $(window).width();
	
	var handleWorldMap = function(trigger = 'load'){
		var vmapSelector = $('#world-map');
		if(trigger == 'resize')
		{
			vmapSelector.empty();
			vmapSelector.removeAttr('style');
		}
		
		vmapSelector.delay( 500 ).unbind().vectorMap({ 
			map: 'world_en',
			backgroundColor: 'transparent',
			borderColor: 'rgb(239, 242, 244)',
			borderOpacity: 0.25,
			borderWidth: 1,
			color: 'rgb(239, 242, 244)',
			colors: { in: "rgba(19, 98, 252, 1)",
            gb: "rgba(19, 98, 252, 0.8)",
            tr: "rgba(19, 98, 252, 0.7)",
            us: "rgba(19, 98, 252,0.6)",
            ru: "rgba(19, 98, 252, 0.5)",
        },
			enableZoom: true,
			hoverColor: 'rgba(19, 98, 252, 0.9)',
			hoverOpacity: null,
			normalizeFunction: 'linear',
			scaleColors: ['#b6d6ff', '#005ace'],
			selectedColor: 'rgba(19, 98, 252, 0.9)',
			selectedRegions: null,
			showTooltip: true,
			onRegionClick: function(element, code, region)
			{
				var message = 'You clicked "'
					+ region
					+ '" which has the code: '
					+ code.toUpperCase();
		 
				alert(message);
			}
		});
	}
	
	var handleUsaMap = function(trigger = 'load'){
		var vmapSelector = $('#usa');
		if(trigger == 'resize')
		{
			vmapSelector.empty();
			vmapSelector.removeAttr('style');
		}
		
		vmapSelector.delay(500).unbind().vectorMap({ 
			map: 'usa_en',
			backgroundColor: 'transparent',
			borderColor: 'rgb(239, 242, 244)',
			borderOpacity: 0.25,
			borderWidth: 1,
			color: 'rgb(239, 242, 244)',
			colors: { 
				va: "rgba(19, 98, 252, 0.8)",
				tx: "rgba(19, 98, 252, 0.7)",
			},
			enableZoom: true,
			hoverColor: 'rgba(19, 98, 252, 0.9)',
			hoverOpacity: null,
			normalizeFunction: 'linear',
			scaleColors: ['#b6d6ff', '#005ace'],
			selectedColor: 'rgba(19, 98, 252, 0.9)',
			selectedRegions: null,
			showTooltip: true,
			onRegionClick: function(element, code, region)
			{
				var message = 'You clicked "'
					+ region
					+ '" which has the code: '
					+ code.toUpperCase();
		 
				alert(message);
			}
		});
	}
	
		return {
			init:function(){
			},
			
			
			load:function(){
				handleWorldMap();
				handleUsaMap();
			},
			
			resize:function(){
				handleWorldMap('resize');
				handleUsaMap('resize');
			}
		}
	
	}();

	jQuery(document).ready(function(){
	});
		
	jQuery(window).on('load',function(){
		setTimeout(function(){
			dzVectorMap.load();
		}, 1000); 
		
	});

	jQuery(window).on('resize',function(){
		setTimeout(function(){
			dzVectorMap.resize();
		}, 1000); 
		
	});     

})(jQuery);	