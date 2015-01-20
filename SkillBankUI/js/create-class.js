$(function(){


	//Create class form validation

	var stepsLeft = 3;

	$("#form-about-me").validate({
		rules: {
			selfintro: {
				required: true
			},
			location: {
				required: true
			}
		},
		messages: {
			selfintro: {
		      required: ""
		    },
		    location: {
		      required: ""
		    }
		}
	});

	var firstFormAdding = 1;
	$("#form-about-me .form-control").bind("keyup", function(){
		
		if ($("#form-about-me").valid()) {
			$("#tab-about-me .icon").addClass("success");
			if (firstFormAdding > 0) {
				firstFormAdding = -1;
				stepsLeft = stepsLeft + firstFormAdding;
			}
		} else {
			$("#tab-about-me .icon").removeClass("success");
			if (firstFormAdding < 0) {
				firstFormAdding = 1;
				stepsLeft = stepsLeft + firstFormAdding;
			}
		}

		$("#number-steps-left").text(stepsLeft);
	});

	$("#form-class-overview").validate({
		rules: {
			title: {
				required: true
			},
			summary: {
				required: true
			}
		},
		messages: {
			title: {
		      required: ""
		    },
		    summary: {
		      required: ""
		    }
		}
	});

	var secondFormAdding = 1;
	$("#form-class-overview .form-control").bind("keyup", function(){
		var adding = 1;

		if ($("#form-class-overview").valid()) {
			$("#tab-class-overview .icon").addClass("success");
			if (secondFormAdding > 0) {
				secondFormAdding = -1;
				stepsLeft = stepsLeft + secondFormAdding;
			}
		} else {
			$("#tab-class-overview .icon").removeClass("success");
			if (secondFormAdding < 0) {
				secondFormAdding = 1;
				stepsLeft = stepsLeft + secondFormAdding;
			}
		}

		$("#number-steps-left").text(stepsLeft);
	});



	//Textarea charactors limit
	$('#selfintro').limit('400','#selfintro-charsleft');

	$('#summary').limit('300','#summary-charsleft');

	$('#class-detail').limit('2000','#class-detail-charsleft');
	
});