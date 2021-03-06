﻿var dashboard = dashboard || new dashboard_Class;
$(document).ready(function () { dashboard.init(); });

function dashboard_Class() {

    this.init = function () {
        this.isTracked = false;
        this.bindWeChatInterval;
        this.resendTimeout = 5000;
        this.resendCount;

        $(".class-share").click(function () { dashboard.shareClass($(this)); });
        $(".mypage-notifications").find(".notify-clear").click(function () { dashboard.setMemberNotificationAsRead($(this)); });
        $(".notification-item").find(".notify-remove").click(function () { dashboard.setOrderNotificationAsRead($(this)); });
        sitecommon.showLoginPop(true);
        $("#getcoinintro-verifybtn").click(function () { dashboard.gotoVerifyPage(); });
        $(".qacode-trigger").click(function () { dashboard.showQACode(); });
    };

    this.gotoVerifyPage = function () {
        if (!dashboard.isTracked && typeof (mixpanel) != "undefined") {
            dashboard.isTracked = true;
            mixpanel.track("v click");
        }
        location.href = "/member/verification";
    }

    this.shareClass = function (btnObj) {
        var currentBlog = sitecommon.getBlogNum(-1);
        if (currentBlog != -2) {
            var title = btnObj.attr("data-title");
            var classid = btnObj.attr("data-classid");

            var sharetext = encodeURI($("#share-text1").val() + title + $("#share-text2").val());
            var url = "http://www.skillbank.cn/class/detail/" + classid;
            var imgPath = "http://www.skillbank.cn/img/certificate.jpg";

            if ($("#share-socialtype").val() == 1)//sina
            {
                url = encodeURI(url + "?etag=sinaweibo_link_websiteshare");
                var sharePath = "http://v.t.sina.com.cn/share/share.php?title=" + sharetext + "&url=" + url + '&rcontent=testcontent' + '&pic=' + imgPath;
                window.open(sharePath, '_blank', 'scrollbars=no,width=600,height=450,left=75,top=20,status=no,resizable=yes');
            } else if ($("#share-socialtype").val() == 3) {//QQ
                url = encodeURI(url + "?etag=qq_link_websiteshare");
                var _appkey = encodeURI("801527091");
                var _site = "http://www.skillbank.cn";
                var sharePath = 'http://v.t.qq.com/share/share.php?title=' + sharetext + '&url=' + url + '&appkey=' + _appkey + '&site=' + _site + '&pic=' + imgPath;
                window.open(sharePath, '转播到腾讯微博', 'width=700, height=680, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, location=yes, resizable=no, status=no');
            }
            if (typeof (mixpanel) != "undefined") {
                ga('send', 'event', 'share', 'click', 'peo_share_ownclass');
                mixpanel.track("share ownclass dashboard");
            }
        }
    }


    this.setMemberNotificationAsRead = function (clearObj) {
        var savePath = "/NotificationHelper/SetOrderNotificationAsReadByMemberId";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: null,
            cache: false,
            success: function (data) {
                consoleLog(data);
                if (data == "true") {
                    clearObj.parent().parent().slideUp();
                }
            }
        });
    }

    this.setOrderNotificationAsRead = function (clearObj) {
        var paraData = { "id": clearObj.attr("data-id") };
        var savePath = "/NotificationHelper/SetOrderNotificationAsReadByNotificationId";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (data == "true") {
                    clearObj.parent().parent().parent().slideUp();
                }
            }
        });
    }

    this.showQACode = function () {
        //var qrImg = $("#qrcode-img");
        //qrImg.attr("src", qrImg.attr("data-src"));

        $(".mobilesite-qrstep1").hide();
        $(".mobilesite-qrstep2").fadeIn();
        sitecommon.addTrackEvent("creat qr");
        //check if bind wechat each 5s * 10times
        dashboard.resendCount = 10;
        dashboard.bindWeChatInterval = window.setInterval("dashboard.bindWeChatCountdown()", dashboard.resendTimeout);
    }

    this.bindWeChatCountdown = function () {
        var btnObj = $("#verifypop-sendbtn");
        dashboard.resendCount--;
        
        if (dashboard.resendCount <= 0 && dashboard.bindWeChatInterval != undefined) {
            window.clearInterval(dashboard.bindWeChatInterval);
            dashboard.bindWeChatInterval = null;
        }

        var savePath = "/API/Member";
        $.ajax({
            url: savePath,
            type: "GET",
            dataType: "Json",
            data: null,
            cache: false,
            success: function (data) {
                if (data) {
                    if (data.OpenId != "" && data.SocailAccount != data.OpenId) {
                        $(".mobilesite-qrstep2").hide();
                        $(".mobilesite-qrstep3").fadeIn();
                        if (dashboard.bindWeChatInterval != undefined) {
                            window.clearInterval(dashboard.bindWeChatInterval);
                            dashboard.bindWeChatInterval = null;
                        }
                        dashboard.resendCount = 0;//to clear timer
                    }
                }
            }
        });
    }

}