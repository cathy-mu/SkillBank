var verification = verification || new verification_Class;
$(document).ready(function () { verification.init(); });

function verification_Class() {
    this.SendVerifyCodePath = "/MemberHelper/SendMobileVerifyCode";
    this.VerifyMobilePath = "/MemberHelper/VerifyMobile";

    this.init = function () {
        this.verifyBtnObj = $("#verify-check");
        this.sendsmsBtnObj = $("#verify-smssend");
        this.mobileObj = $("#profileinfo-phone");
        this.mobileUpdateObj = $("#verify-mobile");
        this.codeObj = $("#profileinfo-verifycode");

        this.initEvents();
        this.resendCount = 30;
        this.countResendInterval;
        this.isChecked = false;
    };

    this.initEvents = function () {
        $("#verify-smssend").click(function () { verification.sendVerifyCode(); });
        $("#verify-check").click(function () { verification.verifyMobile(); });
        verification.mobileUpdateObj.click(function () { $(".mobile-verified").hide(); $(".mobile-verify").show(); });
    }

    this.validCode = function (code) {
        var patten = new RegExp(/^[0-9]{6}$/);
        return patten.test(code);
    }

    this.validMobile = function (mobile) {
        var patten = new RegExp(/^[1][3,4,5,8][0-9]{9}$/);
        return patten.test(mobile);
    }

    this.sendVerifyCode = function () {
        $("#verify-errormobile").hide();
        $("#verify-errorcode").hide();
        var phone = verification.mobileObj.val();
        if (phone == "" || !verification.validMobile(phone)) {
            this.mobileObj.addClass("inputerror");
            return false;
        } else {
            this.mobileObj.removeClass("inputerror");
        }

        verification.sendsmsBtnObj.addClass("disabled");
        verification.countResendInterval = window.setInterval("verification.countResendNum()", 1000);

        var paraData = { "mobile": phone };

        $.ajax({
            url: verification.SendVerifyCodePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                var result = data.Data;
                if (result.r == 1) {
                    verification.verifyBtnObj.show();
                } else {
                    $("#verify-errormobile").show();
                }
            }, error: function (e) {
                consoleLog(e);
            }
        });
    }

    this.countResendNum = function () {
        verification.resendCount--;
        verification.sendsmsBtnObj.text(verification.resendCount + "秒后重发");

        if (verification.resendCount == 0) {
            window.clearInterval(verification.countResendInterval);// && verification.countResendNum != undefined
            verification.sendsmsBtnObj.removeClass("disabled").text("重发验证码");
            verification.resendCount = 30;
        }
    }

    this.verifyMobile = function () {
        $("#verify-errormobile").hide();
        $("#verify-errorcode").hide();

        var phone = verification.mobileObj.val();
        var code = verification.codeObj.val();


        if (phone == "" || !verification.validMobile(phone)) {
            this.mobileObj.addClass("inputerror");
            return false;
        } else {
            this.mobileObj.removeClass("inputerror");
        }

        if (code == "" || !verification.validCode(code)) {
            verification.codeObj.addClass("inputerror");
            return false;
        } else {
            verification.codeObj.removeClass("inputerror");
        }


        verification.verifyBtnObj.addClass("disabled");
        var paraData = { "mobile": phone, "code": code };

        $.ajax({
            url: this.VerifyMobilePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                var result = data.Data;
                console.log(result);
                if (result.r == "1") {
                    $(".mobile-verified").show();
                    $(".mobile-verified div span.gray-light").text(phone+" 已通过验证");
                    $(".mobile-verify").hide();

                    if (!verification.isChecked && typeof (mixpanel) != "undefined") {
                        verification.isChecked = true;
                        mixpanel.track("v mobile");
                    }
                } else {
                    $("#verify-errorcode").show();
                }
                verification.verifyBtnObj.removeClass("disabled");
            }
        });
        
        

    }

   

}




