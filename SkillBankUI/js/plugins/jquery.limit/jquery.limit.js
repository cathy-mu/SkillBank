(function($){ 
     $.fn.extend({  
         limit: function(limit,element) {
			
			var interval, f;
			var self = $(this);
					
			$(this).focus(function(){
				interval = window.setInterval(substring,100);
			});
			
			$(this).blur(function(){
				clearInterval(interval);
				substring();
			});
			
			substringFunction = "function substring(){ var val = $(self).val();var length = val.length;if(length > limit){$(self).val($(self).val().substring(0,limit));}";
			if(typeof element != 'undefined')
			    //substringFunction += "if($(element).html() != limit-length){$(element).html((limit-length<=0)?'0':limit-length);}"
                //Cathy update character feature
			    substringFunction += "if($(element).html() != (length+'/'+limit)){$(element).html((limit-length<=0)?(limit+'/'+limit):(length+'/'+limit));}"

			substringFunction += "}";
			
			eval(substringFunction);
			
			
			
			substring();
			
        } 
    }); 
})(jQuery);