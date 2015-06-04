//jQuery.noConflict();
var sitecommon = sitecommon || new sitecommon_Class;
var configSitePath = "http://"+window.location.host;
var configCacheServer = configSitePath;
/*Social login QQ*/
//var login_btn = document.getElementById("sociallogin-qqbtn"), logout_btn = document.getElementById("sociallogin-logoutbtn");

$(document).ready(function () { sitecommon.init(); });

function sitecommon_Class() {
    this.classCityCookieName = "clcity";//0 for all city
    this.classCategoryCookieName = "clcate";//0 for all cate
    this.classOrderCookieName = "clorder";//orderby and isasc  
    this.expireMins = 30;

    this.socialTypeCookieName = "socialtype";
    this.socialIdCookieName = "socialid";
    this.socialNameCookieName = "socialname";
    this.socialAvatarCookieName = "socialavatar";
    this.memberIdCookieName = "mid";
    //this.isLoginCookieName = "login";
    this.skillCookieName = "classskill";
    this.shareCookieName = "sshare";
    this.backurl = "burl";
    this.checkCoinInterval;
    this.siteDomain;

    this.init = function () {
        this.siteDomain = sitecommon.getDomain();
        $("#sociallogin-logoutbtn").click(function () { sitecommon.socialLogout(); });
        this.initSearchClass();
        $("#sociallogin-sinabtn").click(function () { sitecommon.oAuthLogin(1); });
        $("#sociallogin-renrenbtn").click(function () { sitecommon.oAuthLogin(2); });
        $("#sociallogin-qqbtn").click(function () { sitecommon.oAuthLogin(3); });
        this.checkShareCoins();
        
        $("#header-alerts-pop").click(function () { sitecommon.setAlertAsPoped(); });
        $("#header-alerts-read").click(function () { sitecommon.setAlertAsRead(); });
        $("#header-findclass").click(function () { sitecommon.gotoClassPage(""); });

        this.initTrackEvent();
        if (this.siteDomain != "") {
            this.restoreCookie();
        }
    };

    this.restoreCookie = function () {
        sitecommon.resetCookie(sitecommon.memberIdCookieName);
        sitecommon.resetCookie("sai");
        sitecommon.resetCookie("sid");
        sitecommon.resetCookie("stype");
        //sitecommon.resetCookie("mtype");
        //sitecommon.resetCookie("ohdate");
        //sitecommon.resetCookie("ctr");
        //sitecommon.resetCookie("lng");
    }
        
    this.initTrackEvent = function () {  
        if (typeof (mixpanel) != "undefined") {
            $("#header-membermenu-dashboard").click(function () { sitecommon.addTrackEvent("menu dashboard"); });
            $("#header-membermenu-profile").click(function () { sitecommon.addTrackEvent("menu edit profile"); });
            $("#header-membermenu-message").click(function () { sitecommon.addTrackEvent("menu chat"); });
            $("#header-membermenu-teach").click(function () { sitecommon.addTrackEvent("menu teach"); });
            $("#header-membermenu-learn").click(function () { sitecommon.addTrackEvent("menu learn"); });
            $("#header-alerts-pop").click(function () { sitecommon.addTrackEvent("topbar alert"); });
            $("#header-membermenu-help").click(function () { sitecommon.addTrackEvent("topbar help"); });

            var memberId = sitecommon.getMemberId();
            if (memberId != undefined && memberId > 0) {
                mixpanel.track("page view");
            }
        }
    }

    this.addTrackEvent = function (eventName) {//navUrl,
        if (typeof (mixpanel) != "undefined") {
            mixpanel.track(eventName);
        }
        //window.location = navUrl;
    }
    
    

    //Hide alter for top bar, when user click and see the sub menu for alter
    this.setAlertAsPoped = function () {
        $("#header-alerts-pop span.label-msg").fadeOut();
        var savePath = "/MessageHelper/SetAlertAsClicked";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            cache: false,
            success: function () { }, error: function (e) {
                consoleLog(e);
            }
        });
    }

    this.setAlertAsRead = function () {
        var savePath = "/MessageHelper/SetAlertAsRead";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            cache: false,
            success: function () { }, error: function (e) {
                consoleLog(e);
            }
        });
    }

    this.getBlogNum = function (beforeNum) {
        var blogNum = 0;
        var paraData = { checknum: beforeNum };
        consoleLog(paraData);
        var isAsync = (beforeNum != -1);

        $.ajax({
            url: "/signup/BeforeShareClassOnSocial",
            type: "POST",
            data: paraData,
            dataType: "Json",
            async: isAsync,
            cache: false,
            success: function (data) {
                var result = data.Data;
                //not get coins before
                if (result) {
                    consoleLog(result);
                    blogNum = result.n;
                    //before click share  **s**
                    if (beforeNum == -1 && blogNum >= 0) {
                        var date = new Date();
                        date.setTime(date.getTime() + (5 * 60 * 1000));
                        $.cookie(sitecommon.shareCookieName, blogNum, { expires: date, path: '/' });
                        //Turn on timer
                        if (sitecommon.checkCoinInterval == undefined) {
                            sitecommon.checkCoinInterval = window.setInterval("sitecommon.checkShareCoins()", 10000);
                        }
                    } else if (blogNum == -2) {
                        $("#header-loginbtn").trigger("click");
                    }
                } else {
                    $.cookie(sitecommon.shareCookieName, "", { expires: -1, path: '/' });
                    if (sitecommon.checkCoinInterval != undefined && sitecommon.checkCoinInterval != null) {
                        window.clearInterval(sitecommon.checkCoinInterval);
                    }
                }
            }, error: function (e) {
                consoleLog(e);
            }
        });
        return blogNum;
    }

    this.checkShareCoins = function () {
        consoleLog("----------check share---------");
        var cookieNum = $.cookie(sitecommon.shareCookieName);
        if (cookieNum != undefined && cookieNum != "") {
            var beforeNum = parseInt(cookieNum);
            var afterNum = sitecommon.getBlogNum(beforeNum);
            consoleLog(afterNum + "   :   " + beforeNum);
            if (afterNum > 0 && afterNum > beforeNum) {
                //clear timer and add coin
                window.clearInterval(sitecommon.checkCoinInterval);
                sitecommon.removeCookie(sitecommon.shareCookieName);
            }
        } else if (sitecommon.checkCoinInterval != undefined && sitecommon.checkCoinInterval != null) {
            window.clearInterval(sitecommon.checkCoinInterval);//clear timer if no cookie anymore
        }
    }

    this.showLoginPop = function (checkSocial) {
        var memberId = sitecommon.getMemberId();
        if (memberId == undefined || memberId <= 0) {
            $("#header-loginbtn").trigger("click");
        } else if (checkSocial && $("#loginstatus-social").val() != "1") {
            $("#header-loginbtn").trigger("click");
        }
        consoleLog(memberId);
    }

    this.updateHeaderMemberInfo = function (name, avatar) {
        $("#header-membermenu-avatar").attr("src", sitecommon.getMemberAvatarPath(avatar, "s"));
        $("#header-membermenu-name").text(name);
    }

    this.setCookie = function (cookieName, cookieValue) {
        consoleLog(sitecommon.siteDomain);
        if (sitecommon.siteDomain == "") {
            $.cookie(cookieName, cookieValue, { expires: 30, path: '/' });
        } else {
            $.cookie(cookieName, cookieValue, { expires: 30, path: '/', domain: sitecommon.getDomain() });
        }
    }

    this.setCookie = function (cookieName, cookieValue, expireMins) {
        consoleLog(sitecommon.siteDomain);
        var date = new Date();
        date.setTime(date.getTime() + (expireMins * 60 * 1000));
        if (sitecommon.siteDomain == "") {
            $.cookie(cookieName, cookieValue, { expires: date, path: '/' });
        } else {
            $.cookie(cookieName, cookieValue, { expires: date, path: '/', domain: sitecommon.siteDomain });
        }
    }

    this.removeCookie = function (cookieName) {
        consoleLog(sitecommon.siteDomain);
        $.cookie(cookieName, "", { expires: -1, path: '/' });
        if (sitecommon.siteDomain != "") {
            $.cookie(cookieName, "", { expires: -1, path: '/', domain: sitecommon.siteDomain });
        }
    }

    this.resetCookie = function (cookieName) {
        var value = sitecommon.getCookie(cookieName);
        if (value != null) {
            consoleLog("clear" + cookieName);
            $.cookie(cookieName, "", { expires: -1, path: '/', domain: "www" + sitecommon.siteDomain });
            $.cookie(cookieName, "", { expires: -1, path: '/' });
            $.cookie(cookieName, value, { expires: 30, path: '/', domain: sitecommon.siteDomain });
        }
    }

    this.getDomain = function () {
        var currDomain = window.location.host;
        if (window.location.host.indexOf("www.") == 0) {
            currDomain = currDomain.replace("www.", ".");
        } else if (window.location.host.indexOf("m.") == 0) {
            currDomain = currDomain.replace("m.", ".");
        } else if (window.location.host.indexOf("localhost") == 0) {
            currDomain = "";
        }  
        
        return currDomain;
    }


    this.getCookie = function (cookieName) {
        return $.cookie(cookieName);
    }

    //this.initMemberInfoBySocial = function (socialType, socialId, socialName, socialAvatar) {
    //    this.setCookie(sitecommon.socialTypeCookieName, socialType);
    //    this.setCookie(sitecommon.socialIdCookieName, socialId);
    //    this.setCookie(sitecommon.socialNameCookieName, decodeURIComponent(socialName));
    //    this.setCookie(sitecommon.socialAvatarCookieName, decodeURIComponent(socialAvatar));
    //}

    this.getMemberSocialName = function () {
        return this.getCookie(sitecommon.socialNameCookieName);
    }

    this.getMemberSocialAvatar = function () {
        return this.getCookie(sitecommon.socialAvatarCookieName);
    }

    this.getMemberSocialId = function () {
        return this.getCookie(this.socialIdCookieName);
    }

    this.getMemberSocialType = function () {
        return this.getCookie(this.socialTypeCookieName);
    }

    this.getMemberId = function () {
        return this.getCookie(this.memberIdCookieName);
    }

    this.getMemberAvatarPath = function (path, size) {
        //var path = this.getCookie(sitecommon.memberAvatarCookieName);
        if (path != undefined && path != null) {
            if (path.indexOf("/profile/") > -1) {
                switch (size) {
                    case "s":
                        path = "http://skillbank.b0.upaiyun.com" + path + "!30";
                        break;
                    case "m":
                        path = "http://skillbank.b0.upaiyun.com" + path + "!40";
                        break;
                    case "b":
                        path = "http://skillbank.b0.upaiyun.com" + path + "!100";
                        break;
                    case "h":
                        path = "http://skillbank.b0.upaiyun.com" + path + "!180";
                        break;
                }
            } else {
                if (this.getMemberSocialType() == 4 || path.indexOf("qlogo") > -1) {
                    switch (size) {
                        case "s":
                            path = path.replace("/132", "/46");
                            break;
                        case "m":
                            path = path.replace("/132", "/64");
                            break;
                        default:
                            break;
                    }
                } else if (this.getMemberSocialType() == 1 || path.indexOf("sina") > -1) {
                    switch (size) {
                        case "s":
                            path = path.replace("/180/", "/30/");
                            break;
                        case "m":
                            path = path.replace("/180/", "/50/");
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        return path;
    }

    this.validDate = function (year, month, day) {
        if ((month == 4 || month == 6 || month == 9 || month == 11) && day == 31)
            return false;
        else if (month == 2 && ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0) && day > 29)
            return false;
        else if (month == 2 && ((year % 4 != 0 || year % 100 == 0) && year % 400 != 0) && day > 28)
            return false;
        return true;
    }

    this.validCompareDate = function (comparedate, currdate) {
        if (currdate == comparedate)
            return 0;
        else {
            if (currdate.substr(6, 2) == "20") {
                currdate = currdate.substr(-4, 4) + "/" + currdate.substr(3, 2) + "/" + currdate.substr(0, 2);
            }
            else {
                currdate = currdate.substr(0, 4) + "/" + currdate.substr(5, 2) + "/" + currdate.substr(8, 2);
            }
            var comyear = comparedate.substr(-4, 4);
            comparedate = comyear + "/" + comparedate.substr(0, 5);
            if (comparedate > currdate) {
                return 1;
            }
            else {
                return -1;
            }
        }
    }

    this.validCode = function (code) {
        var patten = new RegExp(/^[0-9]{6}$/);
        return (code != "" && patten.test(code));
    }

    this.validMobile = function (mobile) {
        var patten = new RegExp(/^[1]+\d{10}$/);
        return (mobile != "" && patten.test(mobile));
    }

    this.validEmail = function (email) {
        var patten = new RegExp(/^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]+$/);
        return patten.test(email);
    }

    this.limitReviewLength = function (ctrId, len) {
        var ctrObj = $("#" + ctrId);
        ctrObj.keyup(function () {
            var content = ctrObj.val();
            if (content.length > len) {
                ctrObj.val(content.substr(0, len));
            }
        });
    }
    
    this.initSearchClass = function () {
        $("#class-searchkey").keypress(function () {
            var serachKey = $("#class-searchkey").val();
            if (event.keyCode == 13 && serachKey != undefined && serachKey != "") {
                sitecommon.gotoClassPage(serachKey);
            }
        });
    }

    this.gotoClassPage = function (serachKey) {
        if (sitecommon.getMemberId() > 0) {
            sitecommon.removeCookie(sitecommon.classCityCookieName);
            sitecommon.removeCookie(sitecommon.classCategoryCookieName);
            sitecommon.removeCookie(sitecommon.classOrderCookieName);
        }
        if (serachKey != "") {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("search");
            }
            window.location.href = "/class/?k=" + encodeURIComponent(serachKey);
        } else {
            window.location.href = "/class/";
        }
    }

    this.oAuthLogin = function (type) {
        var currUrl = window.location.href;
        sitecommon.setCookie(sitecommon.backurl, currUrl);
        if (type == 1) {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("sina signup");
            }
            var returnUrl = "http://www.skillbank.cn/signup";
            location.href = "https://api.weibo.com/oauth2/authorize?client_id=111240964&response_type=code&redirect_uri=" + encodeURIComponent(returnUrl);
        } else if (type == 2) {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("renren signup");
            }
        } else if (type == 3) {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("qq signup");
            }
            //var returnUrl = "http://www.skillbank.cn/signup";
            //location.href = "https://open.t.qq.com/cgi-bin/oauth2/authorize?client_id=801527091&response_type=code&redirect_uri=" + encodeURIComponent(returnUrl);
        }
    }

    this.socialLogout = function () {
        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("menu exit");
        }
        
        $.ajax({
            url: "/SignUp/LogOutSocialAccount",
            type: "POST",
            dataType: "Json",
            cache: false,
            success: function (data) {
                //consoleLog(data);
            }, error: function (e) {
                //consoleLog(e);
            }
        });

        $("#header-membermenu").addClass("hide");
        $("#header-loginbtn").removeClass("hide");
        sitecommon.setCookie(sitecommon.memberIdCookieName, 0);
        sitecommon.removeCookie("sai");
        sitecommon.removeCookie("sid");
        sitecommon.removeCookie(sitecommon.socialIdCookieName);
        sitecommon.removeCookie(sitecommon.socialTypeCookieName);

        var currHost = window.location.host;
        //if (currHost.indexOf("m.skillbank.cn") > 0 || currHost.indexOf("/m/") > 0)
        //{
        //    window.location.href = "/m/";
        //} else {
        //    window.location.href = "/";
        //}
    }

}

(function ($) {
    $.getUrlParam
     = function (name) {
         var reg
          = new RegExp("(^|&)" +
          name + "=([^&]*)(&|$)");
         var r
          = window.location.search.substr(1).match(reg);
         if (r != null) return unescape(r[2]); return null;
     }
})(jQuery);

function consoleLog(loginfo) {
    if (window.console) {
        console.log(loginfo);
    }
}



