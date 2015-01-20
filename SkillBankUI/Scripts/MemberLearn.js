﻿var memberlearn = memberlearn || new memberlearn_Class;

$(document).ready(function () { memberlearn.init(); });

function memberlearn_Class() {
    this.init = function () {
        sitecommon.showLoginPop(false);
        $(".learn-cancle").click(function () { memberlearn.cancleBooking($(this)); });
        $(".learn-refund").click(function () { memberlearn.refundOrder($(this)); });
        $(".learn-review").click(function () { memberlearn.showStudentReviewPop($(this)); });
        $(".learn-confirm").click(function () { memberlearn.showConfirmPop($(this)); });
    }

    this.cancleBooking = function (btnObj) {
        var orderId = btnObj.attr("data-orderid");
        if (orderId != undefined && orderId > 0) {
            var mailAddress = btnObj.parent().attr("data-mailaddr");
            var classTitle = btnObj.parent().attr("data-title");
            var memberName = btnObj.parent().attr("data-name");
            var paraData = { orderid: orderId, status: 3, mailaddr: mailAddress, mailname: memberName, title: classTitle };//3 is cancle booking
            consoleLog(paraData);
            memberlearn.updateOrderStatus(paraData);
        }
    }

    this.refundOrder = function (btnObj) {
        var orderId = btnObj.attr("data-orderid");
        if (orderId != undefined && orderId > 0) {
            var mailAddress = btnObj.parent().attr("data-mailaddr");
            var classTitle = btnObj.parent().attr("data-title");
            var memberName = btnObj.parent().attr("data-name");
            var paraData = { orderid: orderId, status: 6, mailaddr: mailAddress, mailname: memberName, title: classTitle };//6 is order refund
            consoleLog(paraData);
            memberlearn.updateOrderStatus(paraData);
        }
    }

    this.addStudentReview = function (orderId, classId) {
        var feedbackCtl = $(".feedback").find("div.checked");

        if (feedbackCtl.length > 0) {
            var feedback = feedbackCtl.parent().attr("data-value");
            var paraData = { orderid: orderId, classid: classId, feedback: feedback, comment: $("#feedback-comment").val() };
            consoleLog(paraData);
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("student give review");
            }
            
            var savePath = "/OrderHelper/ReviewOrder";
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
                }/*, error: function (e) {
                    console.log(e);
                }*/
            });
        }
    }

    this.showStudentReviewPop = function (btnObj) {
        var teacherName = btnObj.parent().attr("data-name");
        var teacherAvatar = btnObj.attr("data-avatar");
        var bookDate = btnObj.attr("data-date");
        var className = btnObj.parent().attr("data-title");
        var orderId = btnObj.attr("data-orderid");
        var classId = btnObj.attr("data-classid");

        $("#feedback-teacheravatar").attr("src", teacherAvatar);
        $("#feedback-bookdate").text(bookDate);
        $("#feedback-teachername").text(teacherName);
        $("#feedback-classname").text(className);
        $("#feedback-subbtn").unbind().click(function () { memberlearn.addStudentReview(orderId, classId); });
    }

    this.updateOrderStatus = function (paraData) {
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
                    window.location.reload();
                } else if (data == 2) {
                    alert("预定状态已改变,无法执行当前操作");//need blurb
                    window.location.reload();
                }
            }/*, error: function (e) {
                console.log(e);
            }*/
        });
    }

    this.showConfirmPop = function (btnObj) {
        var teacherName = btnObj.parent().attr("data-name");
        var teacherAvatar = btnObj.attr("data-avatar");
        var bookDate = btnObj.attr("data-date");
        var className = btnObj.parent().attr("data-title");

        var orderId = btnObj.attr("data-orderid");
        var classId = btnObj.attr("data-classid");
        var teacherId = btnObj.attr("data-teacherid");
        var mailAddress = btnObj.parent().attr("data-mailaddr");

        $("#confirm-feedback-comment").val("");
        $("#confirm-teacheravatar").attr("src", teacherAvatar);
        $("#confirm-bookdate").text(bookDate);
        $("#confirm-teachername").text(teacherName);
        $("#confirm-classname").text(className);

        $("#confirm-subbtn").unbind().click(function () { memberlearn.confirmOrder(orderId, teacherId, classId, mailAddress, teacherName, className); });
    }

    this.confirmOrder = function (orderId, teacherId, classId, mailAddress, memberName, classTitle) {
        var feedbackCtl = $(".confirm-feedback").find("div.checked");
        if (feedbackCtl.length > 0) {
            var feedback = feedbackCtl.parent().attr("data-value");
            var paraData = { orderid: orderId, teacherid: teacherId, classid: classId, feedback: feedback, comment: $("#confirm-feedback-comment").val(), mailaddr: mailAddress, mailname: memberName, title: classTitle };
            consoleLog(paraData);
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("direct pay");
            }
            var savePath = "/OrderHelper/ConfirmOrder";
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
                }/*, error: function (e) {
                    console.log(e);
                }*/
            });
        }
    }

}