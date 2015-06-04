var memberteach = memberteach || new memberteach_Class;

$(document).ready(function () { memberteach.init(); });

function memberteach_Class() {
    this.init = function () {
        sitecommon.showLoginPop(true);
        this.initTabStatus();
        //accept pop
        $(".teach-accept").click(function () { memberteach.showAcceptPopup($(this)); });
        $("#acceptpop-contactname, #acceptpop-contactphone, #acceptpop-contactemail").focus(function () { $("#acceptpop-updatememberinfo").val(1); });
        //bookaccpet pop
        $("#acceptpop-bookbtn").click(function () { memberteach.acceptBooking(); });

        $(".teach-reject").click(function () { memberteach.rejectBooking($(this)); });

        //not test yet
        $(".teach-review").click(function () { memberteach.showTeacherReviewPop($(this)); });
        $(".teach-refundreject").click(function () { memberteach.rejectRefund($(this)); });
        $(".teach-refundprove").click(function () { memberteach.proveRefund($(this)); });
        $(".class-share").click(function () { memberteach.shareClass($(this)); });
        sitecommon.limitReviewLength("feedback-comment", 200);
    }
    
    this.initTabStatus = function () {
        var showOrderTab = ($.getUrlParam("tab") == 1);
        if (showOrderTab) {
            $("div.myclass-teaching ul.nav-tabs li:eq(0)").removeClass("active");
            $("div.myclass-teaching ul.nav-tabs li:eq(1)").addClass("active");
            $("div.myclass-teaching div.tab-content div.tab-pane:eq(0)").removeClass("active");
            $("div.myclass-teaching div.tab-content div.tab-pane:eq(1)").addClass("active");
        }
    }

    this.showAcceptPopup = function (btnObj) {
        $("#acceptpop-updatememberinfo").val("");
        $("#acceptpop-title").text(btnObj.parent().attr("data-title"));
        $("#acceptpop-student").text(btnObj.parent().attr("data-name"));
        $("#acceptpop-date").text(btnObj.attr("data-date"));
        $("#acceptpop-hidorderid").val(btnObj.attr("data-orderid"));
        $("#acceptpop-hidstudentid").val(btnObj.attr("data-studentid"));
        $("#acceptpop-hidmailto").val(btnObj.parent().attr("data-mailaddr"));
        $("#acceptpop-hidmobile").val(btnObj.parent().attr("data-mobile"));
    }

    this.acceptBooking = function () {
        var nameCtl = $("#acceptpop-contactname");
        if ($.trim(nameCtl.val()) == "") {
            nameCtl.addClass("inputerror");
            return false;
        } else {
            nameCtl.removeClass("inputerror");
        }
        var phoneCtl = $("#acceptpop-contactphone");
        if ($.trim(phoneCtl.val()) == "") {
            phoneCtl.addClass("inputerror");
            return false;
        } else {
            phoneCtl.removeClass("inputerror");
        }
        var emailCtl = $("#acceptpop-contactemail");
        if (!sitecommon.validEmail(emailCtl.val())) {
            emailCtl.addClass("inputerror");
            return false;
        } else {
            emailCtl.removeClass("inputerror");
        }

        var orderId = $("#acceptpop-hidorderid").val();
        var studentId = $("#acceptpop-hidstudentid").val();
        var mailAddress = $("#acceptpop-hidmailto").val();
        var classTitle = $("#acceptpop-title").text();
        var memberName = $("#acceptpop-student").text();
        var mobile = $("#acceptpop-hidmobile").val();

        var paraData;
        if ($("#acceptpop-updatememberinfo").val() == 0) {
            paraData = { orderid: orderId, studentid: studentId, mailaddr: mailAddress, mailname: memberName, title: classTitle, mobile: mobile };
        } else {
            paraData = { orderid: orderId, studentid: studentId, mailaddr: mailAddress, mailname: memberName, title: classTitle, mobile: mobile, name: nameCtl.val(), phone: phoneCtl.val(), email: emailCtl.val() };
        }

        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("accept booking");
        }
        consoleLog(paraData);

        var savePath = "/OrderHelper/AcceptOrder";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (data == 2) {
                    alert("对不起，该学生已经没有足够的课币，无法接受预定。");
                } else if (data == 3) {
                    alert("对不起，订单状态已改变。");
                }
                //1- error 1 success 2not enough coin
                window.location.href = "/member/teach";
            }
        });
    }

    this.updateOrderStatus = function (paraData) {
        consoleLog(paraData);

        var savePath = "/OrderHelper/UpdateOrderStatus";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                consoleLog(data);
                if (data == 0) {
                    window.location.href = "/member/teach";
                }
            }
        });
    }
    
    this.rejectBooking = function (btnObj) {
        var orderId = btnObj.attr("data-orderid");

        if (orderId != undefined && orderId > 0) {
            var mailAddress = btnObj.parent().attr("data-mailaddr");
            var classTitle = btnObj.parent().attr("data-title");
            var memberName = btnObj.parent().attr("data-name");
            var studentId = btnObj.parent().attr("data-studentid");
            var mobile = btnObj.parent().attr("data-mobile");
            var paraData = { "orderid": orderId, "status": 2, "mailaddr": mailAddress, "mailname": memberName, "title": classTitle, "mobile":mobile, "student": studentId };//2 is reject booking
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("reject booking");
            }
            memberteach.updateOrderStatus(paraData);
        }
    }
    
    this.rejectRefund = function (btnObj) {
        var orderId = btnObj.attr("data-orderid");
        
        if (orderId != undefined && orderId > 0) {
            var mailAddress = btnObj.parent().attr("data-mailaddr");
            var classTitle = btnObj.parent().attr("data-title");
            var memberName = btnObj.parent().attr("data-name");
            var studentId = btnObj.parent().attr("data-studentid");
            var mobile = btnObj.parent().attr("data-mobile");
            var paraData = { orderid: orderId, status: 8, mailaddr: mailAddress, mailname: memberName, title: classTitle, "mobile": mobile, student: studentId };//8 is reject refund
            memberteach.updateOrderStatus(paraData);
        }
    }

    this.proveRefund = function (btnObj) {
        var orderId = btnObj.attr("data-orderid");
        var studentId = btnObj.attr("data-studentid");
        if (orderId != undefined && orderId > 0 && studentId != undefined && studentId > 0) {
            var mailAddress = btnObj.parent().attr("data-mailaddr");
            var classTitle = btnObj.parent().attr("data-title");
            var memberName = btnObj.parent().attr("data-name");
            var studentId = btnObj.parent().attr("data-studentid");
            var mobile = btnObj.parent().attr("data-mobile");
            var paraData = { orderid: orderId, studentid: studentId, mailaddr: mailAddress, mailname: memberName, title: classTitle, "mobile": mobile };

            consoleLog(paraData);
            var savePath = "/OrderHelper/RefundProve";
            $.ajax({
                url: savePath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                    if (data == 3) {
                        alert("对不起，订单状态已改变。");
                    }
                    window.location.href = "/member/teach";
                }
            });
        }
    }

    this.addTeacherReview = function (orderId) {
        var feedbackCtl = $(".feedback").find("div.checked");
        if (feedbackCtl.length > 0) {
            var feedback = feedbackCtl.parent().attr("data-value");
            var paraData = { orderid: orderId, feedback: feedback, comment: $("#feedback-comment").val() };
            consoleLog(paraData);
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("teach give review");
            }

            var savePath = "/FeedbackHelper/AddTeacherReview";
            $.ajax({
                url: savePath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                    consoleLog(data);
                    if (data == "true") {
                        window.location.reload();
                    }
                }
            });
        } 
    }

    this.showTeacherReviewPop = function (btnObj) {
        var name = btnObj.parent().attr("data-name");
        var avatar = btnObj.attr("data-avatar");
        var bookDate = btnObj.attr("data-date");
        var className = btnObj.parent().attr("data-title");
        var orderId = btnObj.attr("data-orderid");

        $("#feedback-avatar").attr("src", avatar);
        $("#feedback-bookdate").text(bookDate);
        $("#feedback-name").text(name);
        $("#feedback-classname").text(className);
        $("#feedback-subbtn").unbind().click(function () { memberteach.addTeacherReview(orderId); });
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
                window.open(sharePath, 'socialshare', 'scrollbars=no,width=600,height=450,left=75,top=20,status=no,resizable=yes');
            } else if ($("#share-socialtype").val() == 3) {//QQ
                url = encodeURI(url + "?etag=qq_link_websiteshare");
                var _appkey = encodeURI("801527091");
                var _site = "http://www.skillbank.cn";
                var sharePath = 'http://v.t.qq.com/share/share.php?title=' + sharetext + '&url=' + url + '&appkey=' + _appkey + '&site=' + _site + '&pic=' + imgPath;
                window.open(sharePath, 'socialshare', 'width=700, height=680, top=0, left=0, toolbar=no, menubar=no, scrollbars=no, location=yes, resizable=no, status=no');
            }
            if (typeof (mixpanel) != "undefined") {
                ga('send', 'event', 'share', 'click', 'peo_share_ownclass');
                mixpanel.track("share ownclass teach");
            }
        }
    }

}