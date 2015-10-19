var mobileverification = mobileverification || new mobileverification_Class;
$(document).ready(function () { mobileverification.init(); });

function mobileverification_Class() {
    this.SendVerifyCodePath = "/MemberHelper/SendMobileVerifyCode";
    this.VerifyMobilePath = "/MemberHelper/VerifyMobile";

    this.init = function () {
        this.verifyMobileInterval;
        this.resendCount = 30;
    };

    this.initEvents = function () {
        $("#verifypop-sendbtn").click(function () { mobileverification.sendVerifyCode(); });
        $("#verifypop-subbtn").click(function () { mobileverification.verifyMobile(); });
    }
    
    //mobile validation popup
    this.mobileVerifyCountdown = function () {
        var btnObj = $("#verifypop-sendbtn");
        mobileverification.resendCount--;
        
        if (mobileverification.resendCount <= 0 && mobileverification.verifyMobileInterval != undefined) {
            window.clearInterval(mobileverification.verifyMobileInterval);
            btnObj.removeClass("disabled").text("重发验证码");
            mobileverification.resendCount = 30;
        } else {
            btnObj.text(mobileverification.resendCount + "秒后重发");
        }
    }

    this.verifyMobile = function () {
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
            url: mobileverification.VerifyMobilePath,
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
                        var nextaction = actionObj.val();
                        $("#bookpop-contactphone").val(phone);
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

        $("#verifypop-sendbtn").addClass("disabled");
        mobileverification.verifyMobileInterval = window.setInterval("mobileverification.mobileVerifyCountdown()", 1000);
        var paraData = { "mobile": phone };
        $.ajax({
            url: mobileverification.SendVerifyCodePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                var result = data.Data;
                if (result.r == 1) {
                    $("#verifypop-subbtn").show();
                } else {//error
                    $("#verifypop-errormobile").show();
                }
            }
        });
    }
   
}
