var beteacher = beteacher || new beteacher_Class;
$(document).ready(function () { beteacher.init(); });

function beteacher_Class() {
    this.init = function () {
        $("#beteach-gobtn").click(function () { beteacher.gotoNextStep(); });
        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("beteacher page");
        }
    };
       
    this.gotoNextStep = function () {
        var memberId = sitecommon.getMemberId();
        if (memberId != null && memberId > 0) {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("create class");
            }
            window.location = "/class/edit";
        }else if ($("#header-loginbtn").length > 0) {
            $("#header-loginbtn").trigger("click");
        }
    }

}



