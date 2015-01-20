$(function(){

    if ($(".chosen").length > 0) {
    	$(".chosen").chosen();
    }

    //Datepicker
    if ($(".datepicker").length > 0) {
        $(".datepicker").datepicker();
    }

    //Fix header on top on screen
    if ($(".create-class-hd").length > 0) {
        $(".create-class-hd").scrollToFixed();
    }
    else if ($(".header").length > 0) {
        $(".header").scrollToFixed();
    }

    //Skill slider --- A jQuery UI Slider
    if ($("#skill-slider").length > 0) {
        $("#skill-slider").slider({
          range: "min",
	      min: 0,
	      max: 100,
	      value: 60,
	      create: function( event, ui ) {
	      	$("#skill-slider").find(".ui-slider-handle").text(60);
	      },
	      change: function( event, ui ) {
	        $("#skill-slider").find(".ui-slider-handle").text(ui.value);
	      },
	      slide: function( event, ui ) {
	        $("#skill-slider-val").val( ui.value );
	    	$("#skill-slider-input").val( ui.value );
	    	$("#skill-slider").find(".ui-slider-handle").text(ui.value);
	      }
	    });
	    $("#skill-slider-val").val( $( "#skill-slider" ).slider( "value" ) );
	    $("#skill-slider-input").val( $( "#skill-slider" ).slider( "value" ) );
    }

    //Level slider --- A jQuery UI Slider
    if ($("#level-slider").length > 0) {
        $("#level-slider").slider({
          range: "min",
	      min: 0,
	      max: 100,
	      value: 60,
	      create: function( event, ui ) {
	      	$("#level-slider").find(".ui-slider-handle").text(60);
	      },
	      change: function( event, ui ) {
	        $("#level-slider").find(".ui-slider-handle").text(ui.value);
	      },
	      slide: function( event, ui ) {
	        $("#level-slider-val").val( ui.value );
	    	$("#level-slider-input").val( ui.value );
	    	$("#level-slider").find(".ui-slider-handle").text(ui.value);
	      }
	    });
	    $("#level-slider-val").val( $( "#level-slider" ).slider( "value" ) );
	    $("#level-slider-input").val( $( "#level-slider" ).slider( "value" ) );
    }


    //Small switch feature in get icon popup
    $(".btn-show-coin-methods").bind("click", function(){
    	$(".get-coin-methods").removeClass("none");
    	$(".get-coin-steps").addClass("none");
    });

	//Use iCheck for radio and checkbox
    $('input').iCheck({
              checkboxClass: 'icheckbox_square',
              radioClass: 'iradio_square',
              increaseArea: '20%'
            });

    //Use chosen to transform select 
    $(".chosen").chosen({"disable_search_threshold": 10});

    


    //Homepage Video Player
    var videoPlayers = [];

	$('#modal-video').on('show.bs.modal', function (e) {
		var $that = $(this);
		setTimeout(function(){
	  	$that.find("video").mediaelementplayer({
		  	success: function(mediaElement, domObject){
		  		videoPlayers.push(mediaElement);
		  		mediaElement.play();
		  	}
		  });
	  }, 100);
	});

	$('#modal-video').on('hide.bs.modal', function (e) {
	  for (var i = 0; i < videoPlayers.length; i++) {
	  	videoPlayers[i].pause();
	  }
	});


	//Show/Hide private label when input get focus
	$(".create-class .form-control").bind("focus", function(){
		$(this).closest(".row-fluid").find(".label-private, .field-hint").fadeIn(200);
	});
	$(".create-class .form-control").bind("blur", function(){
		$(this).closest(".row-fluid").find(".label-private, .field-hint").fadeOut(200);
	});
    /*
	$(".btn-toggle-class-detail").bind("click", function(){
		var $section = $(this).closest(".row-fluid").next(".section-class-detail");
		var $that = $(this);

		if ($section.hasClass("none")) {
			$section.removeClass("none");
			$that.find(".fa-minus-circle").removeClass("none");
			$that.find(".fa-plus-circle").addClass("none");
		} else {
			$section.addClass("none");
			$that.find(".fa-minus-circle").addClass("none");
			$that.find(".fa-plus-circle").removeClass("none");
		}
	});
    */
	
});