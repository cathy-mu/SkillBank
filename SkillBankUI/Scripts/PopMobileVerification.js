var mobileverification = mobileverification || new mobileverification_Class;
$(document).ready(function () { mobileverification.init(); });

function mobileverification_Class() {
    this.SendVerifyCodePath = "/MemberHelper/SendMobileVerifyCode";
    this.VerifyMobilePath = "/MemberHelper/VerifyMobile";

    this.init = function () {
        //this.initEvents();
        this.verifyMobileInterval;
        this.resendCount = 30;
    };

    this.initEvents = function () {
        $("#verifypop-sendbtn").click(function () { mobileverification.sendVerifyCode(); });
        $("#verifypop-subbtn").click(function () { mobileverification.verifyMobile(); });
    }
    
    //mobile validation popup
    this.mobileVerifyCountdown = function () {
        mobileverification.resendCount--;
        var btnObj = $("#verifypop-sendbtn");
        btnObj.text(mobileverification.resendCount + "秒后重发");

        if (mobileverification.resendCount == 0) {
            window.clearInterval(mobileverification.mobileVerifyCountdown);// && verification.countResendNum != undefined
            btnObj.removeClass("disabled").text("重发验证码");
            mobileverification.resendCount = 30;
        }
    }

    this.verifyMobile = function () {
        var verifyMobilePath = "/MemberHelper/VerifyMobile";
        $("#verify-errormobile").hide();
        $("#verify-errorcode").hide();

        var phoneObj = $("#verifypop-mobile");
        var codeObj = $("#verifypop-code");
        var phone = phoneObj.val();
        var code = codeObj.val();

        if (sitecommon.validMobile(phone)) {
            phoneObj.removeClass("inputerror");
        } else {
            phoneObj.addClass("inputerror");
            return false;
        }

        if (sitecommon.validCode(code)) {
            codeObj.removeClass("inputerror");
        } else {
            codeObj.addClass("inputerror");
            return false;
        }

        var checkBtnObj = $("#verifypop-subbtn");

        checkBtnObj.addClass("disabled");
        var paraData = { "mobile": phone, "code": code };

        $.ajax({
            url: verifyMobilePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                var result = data.Data;
                if (result.r == "1") {
                    $('#modal-verify-mobile .close').trigger("click");
                    var actionObj = $("#classdetail-nextaction");
                    if (actionObj.length > 0) {
                        var nextaction = actionObj.val()
                        if (nextaction == "chat") {
                            $("#class-detail-book").attr("href", "#modal-booking-request");
                            $("#class-detail-btmcontact, #class-detail-contact").attr("href", "#modal-contact-teacher").trigger("click");
                        } else {
                            $("#class-detail-book").attr("href", "#modal-booking-request").trigger("click");
                            $("#class-detail-btmcontact, #class-detail-contact").attr("href", "#modal-contact-teacher");
                        }
                    } else if ($("#class-publish").length > 0) {
                        classedit.publishClass();
                    } else if ($("#chat-sendmessagebtn").length > 0) {
                        $("#chat-sendmessagebtn").attr("data-isverify", 1);
                        chat.addMessage();
                    }
                    
                    if (typeof (mixpanel) != "undefined") {
                        mixpanel.track("popup v mobile");
                    }
                } else {
                    $("#verifypop-errorcode").show();
                }
                checkBtnObj.removeClass("disabled");
            }
        });
    }
    
    this.sendVerifyCode = function () {
        var sendVerifyCodePath = "/MemberHelper/SendMobileVerifyCode";

        $("#verifypop-errormobile").hide();
        $("#verifypop-errorcode").hide();
        var mobileObj = $("#verifypop-mobile");
        var phone = mobileObj.val();
        if (sitecommon.validMobile(phone)) {
            mobileObj.removeClass("inputerror");
        } else {
            mobileObj.addClass("inputerror");
            return false;
        }

        $("#verifypop-smssend").addClass("disabled");
        mobileverification.verifyMobileInterval = window.setInterval("mobileverification.mobileVerifyCountdown()", 1000);

        var paraData = { "mobile": phone };
        consoleLog(paraData);

        $.ajax({
            url: sendVerifyCodePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                var result = data.Data;
                if (result.r == 1) {
                    $("#verifypop-subbtn").show();
                } else {
                    $("#verifypop-errormobile").show();
                }
            }
        });
    }

   
}
