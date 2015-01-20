var signup = signup || new signup_Class;
$(document).ready(function () { signup.init(); });

function signup_Class() {
    this.createMemberPath = "/MemberHelper/CreateMember";
    
    this.init = function () {
        this.emailObj = $("#signup-email");
        this.nameObj = $("#signup-name");
        this.isEnable = true;
        
        this.initInfo();
        this.initEvents();
    };

    this.validSignUpForm = function () {
        var isValidEmail = sitecommon.validEmail(this.emailObj.val());
        var name = $.trim(this.nameObj.val().replace(/[ ]/g, ""));

        var patten = new RegExp(/(skillbank|技能银行|客服)/);
        var isValidName = (name != "" && !patten.test(name));

        if (!isValidName) {
            this.nameObj.addClass("inputerror");
            return false;
        } else {
            this.nameObj.removeClass("inputerror");
        }
        if (!isValidEmail) {
            this.emailObj.addClass("inputerror");
            return false;
        } else {
            this.emailObj.removeClass("inputerror");
        }
        this.createMember();
    };

    this.initEvents = function () {
        $("#signup-subbtn").click(function () { signup.validSignUpForm(); });
    };

    this.initInfo = function () {
        var isAuth = $.getUrlParam("code");
        if (isAuth == undefined || isAuth == "") {
            var name = sitecommon.getMemberSocialName();
            var socialType = sitecommon.getMemberSocialType();
            var avatarPath = sitecommon.getMemberSocialAvatar();
            this.updateHeaderMemberInfo = function (name, avatar) {
                $("#header-membermenu-avatar").attr("src", sitecommon.getMemberAvatarPath(avatarPath, "s"));
                $("#header-membermenu-name").text(name);
            }

            $("#signup-socialname").text(name);
            $("#signup-socialavatar").attr("src", sitecommon.getMemberAvatarPath(avatarPath, "m"));
            $("#signup-hidavatar").val(sitecommon.getMemberSocialAvatar());
            $("#signup-hidsid").val(sitecommon.getMemberSocialId());
            $("#signup-hidtype").val(sitecommon.getMemberSocialType());
        } else if ($("#signup-mid").val() != undefined) {
            var memberId = $("#signup-mid").val();
            if (memberId != "" && memberId > 0) {
                signup.redirectAfterLogin();
            } else {
                if (typeof (mixpanel) != "undefined") {
                    mixpanel.track("signup page");
                }
            }
        }
    }

    /* For account (Social login) */
    this.createMember = function () {
        if (signup.isEnable) {
            signup.isEnable = false;

            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("submit signup");
                ga('send', 'event', 'button', 'click', 'sign_up1');
            }
            var city = 0;
            var userName = this.nameObj.val();
            var paraData = { account: $("#signup-hidsid").val(), socialType: $("#signup-hidtype").val(), memberName: userName, email: this.emailObj.val(), avatar: $("#signup-hidavatar").val()/*, cityId: city*/ };
            consoleLog(paraData);

            $.ajax({
                url: this.createMemberPath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                    var result = data.Data;
                    if (result) {
                        consoleLog(result);
                        if (result.r) {
                            ga('send', 'event', 'button', 'click', 'sign_up');
                        }
                        signup.redirectAfterLogin();
                    }
                }, error: function (e) {
                    consoleLog(e);
                    signup.isEnable = true;
                }
            });
        }
    }

    this.redirectAfterLogin = function () {
        var backUrl = sitecommon.getCookie(sitecommon.backurl);

        if (backUrl == undefined || backUrl == "") {
            backUrl = document.referrer.toLowerCase();
        } else {
            backUrl = decodeURIComponent(backUrl);
        }

        if (backUrl == "" || backUrl.indexOf("/signup") > 0) {
            location.href = "/";
        }
        else {
            location.href = backUrl;
        }
    }
    

}