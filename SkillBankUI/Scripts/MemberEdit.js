var memberedit = memberedit || new memberedit_Class;

$(document).ready(function () { memberedit.init(); });

function memberedit_Class() {
    this.init = function () {
        this.initSliderBar();
    };

   

    this.initSliderBar = function () {
       // $("#slider").slider();

        $("#skill-slider, #teach-slider").slider({
            range: "min",
            value: 50,
            slide: function (event, ui) {
                $(this).find("a.ui-slider-handle").text(ui.value);
            }

        });

        $("#skill-slider a.ui-slider-handle").text($("#skill-slider").slider("value"));
        $("#teach-slider a.ui-slider-handle").text($("#teach-slider").slider("value"));

    }

   

}




