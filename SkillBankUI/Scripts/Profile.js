var proflie = proflie || new proflie_Class;

$(document).ready(function () { proflie.init(); });

function proflie_Class() {
    this.init = function () {
        this.ctlPosX = $("#profile-myposx").val();
        this.ctlPosY = $("#profile-myposy").val();
        this.initMapDistince();
        this.initEvents();
        this.initReviewMoreBtn();

    };

    
    this.initEvents = function () {
        $(".review-filter-students").find(".iCheck-helper").click(function () { proflie.filterReview($(this), 0); });
        $(".review-filter-teachers").find(".iCheck-helper").click(function () { proflie.filterReview($(this), 1); });
        $(".review-morebtn").click(function () { proflie.getMoreReview($(this)); });
        proflie.initLikeBtn();
    }

    this.initMapDistince = function () {
        var obj = new AMap.LngLat(proflie.ctlPosX, proflie.ctlPosY);
        //data-pos="PosX,PosY"
        $(".classlist-distince").each(function () {
            var pos = $(this).attr("data-pos").split(',');
            //consoleLog(pos[0] + " : " + pos[1]);
            var distince = obj.distance(new AMap.LngLat(pos[0], pos[1]));
            if (distince > 1000) {
                distince = (distince / 1000).toFixed(0) + "千米";
            } else {
                distince = Math.round(distince) + "米";
            }
            $(this).text(distince);
        });
    }

    this.initReviewMoreBtn = function () {
        proflie.checkReviewMoreBtn(0, 0);
        proflie.checkReviewMoreBtn(1, 0);
    }

    this.checkReviewMoreBtn = function (tabId, feedback) {
        var total = $("#review-tabsum" + tabId + feedback).attr("data-value");
        var curr = $("#review-list-tab" + tabId).find(".review-item").length;
        if (curr < total) {
            $("#review-morebtn" + tabId).removeClass("hide");
        } else {
            $("#review-morebtn" + tabId).addClass("hide");
        }
    }

    this.filterReview = function (radObj, tabid) {
        var memberId = $("#profile-hidmemberid").val();
        var feedbackType = radObj.parent().parent().attr("data-value");
        var maxid = parseInt($("#profile-hidmaxid" + tabid).val()) + 1;
        var paraData = { tabid: tabid, memberid: memberId, feedback: feedbackType, maxid: maxid };
        proflie.appendReviewItem(paraData, tabid, feedbackType, 1)
    }

    this.getMoreReview = function (btnObj) {
        var tabid = btnObj.attr("data-value");
        var memberId = $("#profile-hidmemberid").val();
        var feedbackType;
        if (tabid == 0) {
            feedbackType = $("#review-students").find("div.checked").parent().attr("data-value");
        } else {
            feedbackType = $("#review-teachers").find("div.checked").parent().attr("data-value");
        }
        var maxid = $("#profile-hidminid" + tabid).val();
        var paraData = { tabid: tabid, memberid: memberId, feedback: feedbackType, maxid: maxid };
        proflie.appendReviewItem(paraData, tabid, feedbackType, 0);
    }

    this.appendReviewItem = function (paraData, tabid, feedback, isClear) {
        var tabObj = $("#review-list-tab" + tabid);
        var savePath = "/FeedBackHelper/GetMemberReview";
        consoleLog(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (streams) {
                consoleLog(streams);
                if (isClear) {
                    tabObj.empty();
                }
                for (var i = 0; i < streams.length; i++) {
                    var _streamsTemp = $("#review-item-template").children().clone();
                    _streamsTemp.find("a.review-item-memberlnk").attr("href", "/profile/" + streams[i].MemberId)
                    _streamsTemp.find(".span1 a.review-item-memberlnk img").attr("src", sitecommon.getMemberAvatarPath(streams[i].Avatar, "m"));
                    _streamsTemp.find(".gray-light a.review-item-memberlnk").text(streams[i].Name);
                    _streamsTemp.find("a.review-item-classlnk").attr("href", "/class/detail/" + streams[i].ClassId).text(streams[i].Title);
                    _streamsTemp.find(".gray-light label").html(streams[i].CreatedDate);
                    _streamsTemp.find(".span11 p").html(streams[i].Comment);
                    if (streams[i].FeedBack == 1) {
                        _streamsTemp.find(".span8 .label-bad").removeClass("hide");
                    } else if (streams[i].FeedBack == 2) {
                        _streamsTemp.find(".span8 .label-okay").removeClass("hide");
                    } else {
                        _streamsTemp.find(".span8 .label-good").removeClass("hide");
                    }
                    _streamsTemp.appendTo(tabObj);
                }
                var maxid = $("#profile-hidminid" + tabid).val(streams[streams.length - 1].ReviewId);
                proflie.checkReviewMoreBtn(tabid, feedback);
            }, error: function (e) {
                consoleLog(e);
            }
        });
    }

    this.initLikeBtn = function () {
        $(".profile-linkicon").click(function () {
            var memberId = sitecommon.getMemberId();
            if (memberId == undefined || memberId <= 0) {
                $("#header-loginbtn").trigger("click");
            } else {
                var likeIconObj = $(this);
                var likeNumObj = likeIconObj.next(".profile-linknum");
                var memberId = likeIconObj.attr("data-id");

                var isLike = likeIconObj.hasClass("fa-heart-like");
                var likeNum = parseInt(likeNumObj.text());
                proflie.setLikeTag(memberId, !isLike);
                if (isLike) {
                    likeIconObj.removeClass("fa-heart-like");
                    likeNumObj.text(likeNum - 1);
                } else {
                    //if (typeof (mixpanel) != "undefined") {
                    //    mixpanel.track("like on memberprofile");
                    //}
                    likeIconObj.addClass("fa-heart-like");
                    likeNumObj.text(likeNum + 1);
                }
            }
        });
    }

    //Seems not use it yet
    this.setLikeTag = function (relatedId, isLike) {
        var paraData = { "FollowingId": relatedId, "IsFollow": isLike };
        var savePath = "/API/FollowMember";
        consoleLog(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function () {
            }
        });
    }

}