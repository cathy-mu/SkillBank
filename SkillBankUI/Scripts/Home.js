var home = home || new home_Class;

$(document).ready(function () { home.init(); });

function home_Class() {
    this.init = function () {
        home.initFooterBtn();
        if (typeof (mixpanel) != "undefined") {
            home.initMoreBtnTracking();
            home.initClassTracking();
        }
    };

    this.initFooterBtn = function () {
        $(".home-joinus-box").click(function () {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("register hbottom");
            }
            var memberId = sitecommon.getMemberId();
            if (memberId != undefined && memberId > 0) {
                window.location.href = "/class/beteacher";
            } else {
                $("#header-loginbtn").trigger("click");
            }
        });
    }

    this.initMoreBtnTracking = function () {
        $(".home-classlistlink").click(function () {
            var btnId = $(this).attr("data-id");
            if (btnId == 1) {
                mixpanel.track("vip more");
            } else if (btnId == 2) {
                mixpanel.track("lastest more");
            } else if (btnId == 3) {
                mixpanel.track("minority more");
            }
        });
    }
    
    this.initClassTracking = function () {
        $(".class-item-hd a[data-code],.class-item-bd h3 a[data-code]").click(function () {
            mixpanel.track($(this).attr("data-code"));
        });
    }

}

