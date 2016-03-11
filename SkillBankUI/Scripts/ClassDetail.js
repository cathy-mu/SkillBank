var classdetail = classdetail || new classdetail_Class;
$(document).ready(function () { classdetail.init(); });
function classdetail_Class() {

    this.init = function () {
        this.sendMessageBtn = $("#message-sendbtn");

        this.initEvents();
        this.initClassProve();
        this.initReviewMoreBtn();
        this.initLikeBtn();
    };

    this.initEvents = function () {
        var shouldVerify = ($("#class-detail-contact").attr("href") == "#modal-verify-mobile");

        $("#class-detail-contact, #class-detail-btmcontact").click(function () {
            $("#message-content").val("");
            if (shouldVerify) {
                $("#classdetail-nextaction").val("chat");
            }
        });
        $("#bookpop-contactname, #bookpop-contactphone, #bookpop-contactemail").focus(function () { $("#bookpop-updatememberinfo").val(1); });
        classdetail.sendMessageBtn.click(function () { classdetail.addMessage(); });
        if (shouldVerify) {
            $("#class-detail-book").click(function () { $("#classdetail-nextaction").val("book"); });
            mobileverification.initEvents();
        }

        //detail page
        if ($("#classpreview-prove").length == 0) {
            $("#bookpop-bookbtn").click(function () { classdetail.addOrder(); });
            $(".review-filter-class").find(".iCheck-helper").click(function () { classdetail.filterReview($(this), 0); });
            $(".review-filter-other").find(".iCheck-helper").click(function () { classdetail.filterReview($(this), 1); });
            $(".review-morebtn").click(function () { classdetail.getMoreReview($(this)); });
            $("#classdetail-commentaddbtn").click(function () { classdetail.addComment(); });

            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("class detail");
            }
        } else {
            //preview page
            $("#classpreview-setgroup").click(function () {
                classdetail.setRecommendation();
            });
        }

    }

    this.setRecommendation = function () {
        var classId = $("#classdetail-hidclassid").val();
        var paraStr = "";
        $(".class-groupitem").each(function () {
            var $this = $(this);
            var rank = $this.find(".class-grouprank").val();
            var id = $this.attr("data-id");
            if ($(this).find(".class-groupset").parent().hasClass("checked")) {
                if (rank == "") {
                    alert("Rank " + id + " 不能为空");
                    return false;
                }
                else {
                    paraStr += (id + ",");
                    paraStr += ($(this).find(".class-grouphome").parent().hasClass("checked") ? 1 : 0);
                    paraStr += ("," + rank + ";");
                }
            }

        });
        var paraData = { "classId": classId, "paraStr": paraStr };
        var savePath = "/Tools/SetRecommendationClass";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert("设置成功");
            }
        });
    }

    this.addComment = function () {
        var classId = $("#classdetail-hidclassid").val();
        var commentCtl = $("#classdetail-comment");
        if (commentCtl.val() != "") {
            var paraData = { "ClassId": classId, "CommentText": commentCtl.val() };
            var savePath = "/api/comment";
            consoleLog(paraData);
            $.ajax({
                url: savePath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                }
            });
        }
    }

    this.initLikeBtn = function () {
        $("#class-detail-linkicon").click(function () {
            var memberId = sitecommon.getMemberId();
            if (memberId == undefined || memberId <= 0) {
                $("#header-loginbtn").trigger("click");
            } else {
                var isLike = $("#class-detail-linkicon").hasClass("fa-heart-like");
                var likeNum = parseInt($("#class-detail-linknum").text());
                classdetail.setLikeTag(!isLike);
                if (isLike) {
                    $("#class-detail-linkicon").removeClass("fa-heart-like");
                    $("#class-detail-linknum").text(likeNum - 1);
                } else {
                    if (typeof (mixpanel) != "undefined") {
                        ga('send', 'event', 'like', 'click', 'onClass_page');
                        mixpanel.track("like on classdetail");
                    }
                    $("#class-detail-linkicon").addClass("fa-heart-like");
                    $("#class-detail-linknum").text(likeNum + 1);
                }
            }
        });
    }

    this.setLikeTag = function (isLike) {
        var classId = $("#classdetail-hidclassid").val();
        var paraData = { "classId": classId, "IsLike": isLike };
        var savePath = "/API/LikeClass";

        consoleLog(paraData);
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

    this.initClassProve = function () {
        if ($("#classpreview-prove").length > 0) {
            $("#classpreview-prove").click(function () { classdetail.setClassTag($(this), 1, 2); });
            $("#classpreview-reject").click(function () { classdetail.setClassTag($(this), 2, 2); });
            $("#classpreview-setcatetag").click(function () { classdetail.setClassCateTag(); });
            $("#classpreview-cate").change(function () {
                var selObj = $(this).children('option:selected');
                $("#classdetail-hidcateid").val(selObj.val());
            });
        } else {
            $("#classpreview-active").click(function () { classdetail.setClassTag($(this), 1, 14); });
            $("#classpreview-disactive").click(function () { classdetail.setClassTag($(this), 2, 14); });
        }
    }
    
    this.validCompareDate = function (comparedate, currdate) {
        if (currdate == comparedate)
            return 0;
        else {
            return (comparedate > currdate) ? 1 : -1;
        }
    }

    this.addOrder = function () {
        var classId = $("#classdetail-hidclassid").val();

        var dateCtl = $("#bookpop-bookdate");
        var bookDate = dateCtl.val();
        var currDate = $("#bookpop-currdate").val();
        if (bookDate != "" && classdetail.validCompareDate(bookDate, currDate) > 0) {
            dateCtl.removeClass("inputerror");
        } else {
            dateCtl.addClass("inputerror");
            return false;
        }

        var nameCtl = $("#bookpop-contactname");
        if ($.trim(nameCtl.val()) == "") {
            nameCtl.addClass("inputerror");
            return false;
        } else {
            nameCtl.removeClass("inputerror");
        }

        var phoneCtl = $("#bookpop-contactphone");
        if ($.trim(phoneCtl.val()) == "") {
            phoneCtl.addClass("inputerror");
            return false;
        } else {
            phoneCtl.removeClass("inputerror");
        }

        var emailCtl = $("#bookpop-contactemail");
        if (!sitecommon.validEmail(emailCtl.val())) {
            emailCtl.addClass("inputerror");
            return false;
        } else {
            emailCtl.removeClass("inputerror");
        }

        var studentRemark = $("#bookpop-remark").val();
        var classTitle = $("#classdetail-title").text();
        var memberName = $("#classdetail-hidmrname").val();
        var mailAddress = $("#classdetail-hidmremail").val();
        var mobile = $("#bookpop-bookbtn").attr("data-tmobile");
        var paraData;
        if ($("#bookpop-updatememberinfo").val() == 0) {
            paraData = { classid: classId, bookdate: bookDate, remark: studentRemark, mailaddr: mailAddress, mailname: memberName, title: classTitle, mobile: mobile };
        } else {
            var paraData = { classid: classId, bookdate: bookDate, remark: studentRemark, mailaddr: mailAddress, mailname: memberName, title: classTitle, mobile: mobile, name: nameCtl.val(), phone: phoneCtl.val(), email: emailCtl.val() };
        }
        var savePath = "/OrderHelper/AddOrder";
        
        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("book class");
        }
        
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (data == "true") {
                    window.location.href = "/member/learn";
                }
            }
        });
        
    }

    this.addMessage = function () {
        var messageCtl = $("#message-content");
        var savePath = "/MessageHelper/AddMessage";
        var messageContent = messageCtl.val();

        if (messageContent == undefined || $.trim(messageContent) == "") {
            messageCtl.addClass("inputerror");
        } else {
            var senderName = $("#classdetail-hidmsname").val();
            var receiverName = $("#classdetail-hidmrname").val();
            var receiverEmail = $("#classdetail-hidmremail").val();
            console.log(senderName, receiverName);
            messageCtl.removeClass("inputerror");
            var toId = $("#classdetail-hidmemberid").val();
            var classTitle = $("#classdetail-title").text();
            var mobile = $("#message-sendbtn").attr("data-tomobile");
            var paraData = { to: toId, message: messageContent, msname: senderName, mremail: receiverEmail, mrname: receiverName, title: classTitle, mobile: mobile };
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("chat on classdetail");
            }

            $.ajax({
                url: savePath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                    if (data == "true") {
                        $("#message-content").val("");
                        $("#contactpop-close").trigger("click");
                    }
                }
            });
        }
    }

    this.setClassCateTag = function () {
        var classId = $("#classdetail-hidclassid").val();
        var cateId = $("#classdetail-hidcateid").val();
        var classTags = $("#classpreview-tags").val().replace("，",",");
        var savePath = "/ClassHelper/UpdateClassTagCategory";
        var paraData = { classId: classId, cateId: cateId, tags: classTags };
        consoleLog(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (data) {
                    $("#classpreview-setcatetag").addClass("saved").removeClass("btn-primary").text("已修改");
                }
            }
        });
    }

    this.setClassTag = function (btnObj, infoValue, saveType) {
        var savePath = "/ClassHelper/UpdateClassStatus";//Class update 1.1
        var paraData;
        var classId = $("#classdetail-hidclassid").val();
        if (saveType == 14) {
            paraData = { classId: classId, infoValue: infoValue, saveType: saveType };
        }
        else if (saveType == 18) {
            var cateId = $("#classdetail-hidcateid").val();
            paraData = { classId: classId, infoValue: cateId, saveType: saveType };
        }
        else {//2 prove class
            var isProved = (infoValue == 1);
            var cateId = $("#classdetail-hidcateid").val();
            if (isProved && typeof (mixpanel) != "undefined") {
                ga('send', 'event', 'class', 'click', 'peo_create_class');
                mixpanel.track("prove class");
            }
            if (btnObj.attr("data-classname")) {
                paraData = { classId: classId, infoValue: cateId, saveType: saveType, isProved: isProved, className: btnObj.attr("data-classname"), name: btnObj.attr("data-teachername"), email: btnObj.attr("data-email"), mobile: btnObj.attr("data-mobile"), device: btnObj.attr("data-device") };
            } 
        }
        consoleLog(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (infoValue == 1) {
                    if (saveType == 14) {
                        $("#classpreview-active").addClass("saved").removeClass("btn-primary").text("已上线");
                    }else {
                        $("#classpreview-prove").addClass("saved").removeClass("btn-primary").text("已批准");
                    }
                } else if (infoValue == 2) {
                    if (saveType == 14) {
                        $("#classpreview-disactive").addClass("saved").removeClass("btn-warning").text("已下架");
                    } else {
                        $("#classpreview-reject").addClass("saved").removeClass("btn-warning").text("已拒绝");
                    }
                }
                
            }
        });
    }
        

    this.initReviewMoreBtn = function () {
        classdetail.checkReviewMoreBtn(0, 0);
        classdetail.checkReviewMoreBtn(1, 0);
        //Comment test
        //classdetail.checkReviewMoreBtn(2, 0);
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
        var classId = $("#classdetail-hidclassid").val();
        var memberId = $("#classdetail-hidmemberid").val();
        var feedbackType = radObj.parent().parent().attr("data-value");
        var maxid = parseInt($("#classdetail-hidmaxid" + tabid).val()) + 1;
        var paraData = { tabid: tabid, memberid: memberId, classid: classId, feedback: feedbackType, maxid: maxid };
        classdetail.appendReviewItem(paraData, tabid, feedbackType, 1)
    }

    this.getMoreReview = function (btnObj) {
        var tabid = btnObj.attr("data-value");
        var classId = $("#classdetail-hidclassid").val();
        var memberId = $("#classdetail-hidmemberid").val();
        var feedbackType;
        if (tabid == 0) {
            feedbackType = $("#review-current").find("div.checked").parent().attr("data-value");
        } else if (tabid == 1) {
            feedbackType = $("#review-other").find("div.checked").parent().attr("data-value");
        } else if (tabid == 2) {
            feedbackType = 0;
        }
        var maxid = $("#classdetail-hidminid" + tabid).val();
        var paraData = { tabid: tabid, memberid: memberId, classid: classId, feedback: feedbackType, maxid: maxid };
        classdetail.appendReviewItem(paraData, tabid, feedbackType, 0);
    }

    this.appendReviewItem = function (paraData, tabid, feedback, isClear) {
        var tabObj = $("#review-list-tab" + tabid);
        var savePath = "/FeedBackHelper/GetClassReview";
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
                    _streamsTemp.find(".gray-light label").html(streams[i].CreatedDate);
                    _streamsTemp.find(".span11 p").html(streams[i].Comment);
                    if (tabid == 1 && streams[i].ClassId != 0) {
                        _streamsTemp.find("a.review-item-classlnk").attr("href", "/class/detail/" + streams[i].ClassId).text(streams[i].Title);
                    }
                    if (tabid < 2) {
                        if (streams[i].FeedBack == 1) {
                            _streamsTemp.find(".span8 .label-bad").removeClass("hide");
                        } else if (streams[i].FeedBack == 2) {
                            _streamsTemp.find(".span8 .label-okay").removeClass("hide");
                        } else {
                            _streamsTemp.find(".span8 .label-good").removeClass("hide");
                        }
                    } else {
                    }
                    _streamsTemp.appendTo(tabObj);
                }
                var maxid = $("#classdetail-hidminid" + tabid).val(streams[streams.length - 1].ReviewId);
                classdetail.checkReviewMoreBtn(tabid, feedback);
            }, error: function (e) {
                consoleLog(e);
            }
        });
    }

}