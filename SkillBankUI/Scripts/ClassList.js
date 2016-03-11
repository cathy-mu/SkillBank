var classlist = classlist || new classlist_Class;
$(document).ready(function () { classlist.init(); }); 

function classlist_Class() {

    this.init = function () {
        this.ctlCatePId = $("#classlist-hidcategorypid");//for parent category
        this.ctlCateId = $("#classlist-hidcategoryid");//catagory id for submit
        this.ctlCityId = $("#classlist-hidcityid");
        this.cityCtlId = "classlist-city";
        this.cateIdPrefix = "#class-category-list";
        this.subCateIdPrefix = "#class-subcategory-list";
        this.cityPrefix = "#class-city-list";

        this.ctlPosX = $("#classlist-myposx").val();
        this.ctlPosY = $("#classlist-myposy").val();

        this.initEvents();
        this.initCity();
        this.initCategory();
        
        this.initMapDistince();
        this.initLikeBtn();
        this.initClassTab();
    };

    this.initClassTab = function () {
        $(".class-tabs-holder").click(function () {
            sitecommon.setCookie(sitecommon.classCategoryCookieName, "0_0", classlist.expireMins);
            sitecommon.setCookie(sitecommon.classCityCookieName, 0, classlist.expireMins);
            classlist.gotoClassTab($(this).attr("data-id"));
        });
    }

    this.gotoClassTab = function (tabid) {
        if (tabid > 0) {
            if (typeof (mixpanel) != "undefined") {
                if (tabid == 1) {
                    mixpanel.track("vip");
                } else if (tabid == 3) {
                    mixpanel.track("minority");
                }
            }
            window.location.href = "/class/?tabid=" + tabid;
            
        } else {
            window.location.href = "/class";
        }
    }

    this.initEvents = function () {
        $("#classf-createbtn").click(function () { classlist.createNewClass(); });
        $("#classlist-orderby a").click(function () { classlist.changeOrder($(this)); });
        $(".pagination li:not(.curr)").click(function () { classlist.pagination($(this)); });
    }

    this.pagination = function (navObj) {
        var currId = $.getUrlParam("id");
        var gotoId;
        var replaceUrl = false;
        if (currId == undefined || currId == "") {
            currId = 1;
        } else {
            replaceUrl = true;
        }

        var url = window.location.href;
        var navId = navObj.attr("data-id");
        if (navId == "pre") {
            gotoId = parseInt(currId)-1;
        } else if (navId == "next") {
            gotoId = parseInt(currId)+1;
        } else {
            gotoId = navId;
        }
        if (replaceUrl) {
            url = url.replace("?id=" + currId, "?id=" + gotoId).replace("&id=" + currId, "&id=" + gotoId);
        } else if (url.indexOf("?") > 0) {
            url += "&id=" + gotoId;
        } else {
            url += "?id=" + gotoId
        }
        
        window.location =url;
    }

    this.initMapDistince = function () {
        var obj = new AMap.LngLat(this.ctlPosX, this.ctlPosY);
        //data-pos="PosX,PosY"
        $(".classlist-distince").each(function () {
            var pos = $(this).attr("data-pos").split(',');
            var distince = obj.distance(new AMap.LngLat(pos[0], pos[1]));
            if (distince > 1000) {
                distince = (distince / 1000).toFixed(0) + "千米";
            } else {
                distince = Math.round(distince) + "米";
            }
            $(this).text(distince);
        });
    }

    //For category dropdowns
    this.initCategory = function () {
        $(this.cateIdPrefix).change(function () {
            var selObj = $(this).children('option:selected');
            var subData = selObj.attr("data-hassub");
            sitecommon.setCookie(sitecommon.classCategoryCookieName, selObj.val() + "_" + subData, classlist.expireMins);
            classlist.getClassInfo();
        });
    }

    //For city dropdowns
    this.initCity = function () {
        $(this.cityPrefix).change(function () {
            var selObj = $(this).children('option:selected');
            sitecommon.setCookie(sitecommon.classCityCookieName, selObj.val(), classlist.expireMins);
            classlist.getClassInfo();
        });
    }
    
    // Filter Class List 
    this.getClassInfo = function () {
        var paraData = { "cityId": sitecommon.getCookie(sitecommon.classCityCookieName), "cateId": sitecommon.getCookie(sitecommon.classCategoryCookieName), "orderBy": sitecommon.getCookie(sitecommon.classOrderCookieName) };
        
        var currId = $.getUrlParam("id");
        var currTabId = $.getUrlParam("tabid");
        var url = window.location.href;
        var replaceUrl = false;
        if (currId != undefined && currId != "") {
           url = url.replace("?id=" + currId, "?id=1").replace("&id=" + currId, "&id=1");
        }
        if (currTabId != undefined && currTabId != "") {
            url = url.replace("?tabid=" + currTabId, "");
        }
        url = url.replace("#", "");
        window.location = url;
    }
    
    // Change Class List  order
    this.changeOrder = function (orderObj) {
        //$("#classlist-orderby a").removeClass("selected");
        //orderObj.addClass("selected");

        //var isByAsc = orderObj.find("i").hasClass("fa-long-arrow-down");
        //if (isByAsc) {
        //    orderObj.find("i").removeClass("fa-long-arrow-down").addClass("fa-long-arrow-up");
        //} else {
        //    orderObj.find("i").removeClass("fa-long-arrow-up").addClass("fa-long-arrow-down");
        //}
        var orderKey = orderObj.attr("data-id");
        sitecommon.setCookie(sitecommon.classOrderCookieName, orderKey, classlist.expireMins);
        classlist.getClassInfo();
    }

    this.initLikeBtn = function () {
        $(".class-list-linkicon").click(function () {
            var memberId = sitecommon.getMemberId();
            if (memberId == undefined || memberId <= 0) {
                $("#header-loginbtn").trigger("click");
            } else {
                var likeIconObj = $(this);
                var likeNumObj = likeIconObj.next(".class-list-linknum");
                var classId = likeIconObj.attr("data-id");

                var isLike = likeIconObj.hasClass("fa-heart-like");
                var likeNum = parseInt(likeNumObj.text());
                classlist.setLikeTag(classId, !isLike);
                if (isLike) {
                    likeIconObj.removeClass("fa-heart-like");
                    likeNumObj.text(likeNum - 1);
                } else {
                    if (typeof (mixpanel) != "undefined") {
                        mixpanel.track("like on classlist");
                    }
                    likeIconObj.addClass("fa-heart-like");
                    likeNumObj.text(likeNum + 1);
                }
            }
        });
    }

    this.setLikeTag = function (classId, isLike) {
        var paraData = {"classId": classId, "IsLike": isLike };
        var savePath = "/API/LikeClass";
        
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {

            }, error: function (e) {
                consoleLog(e);
            }
        });
    }

}




