var chat = chat || new chat_Class;

$(document).ready(function () { chat.init(); });

function chat_Class() {
    this.init = function () {
        sitecommon.showLoginPop(false);
        $("#chat-sendmessagebtn").click(function () { chat.addMessage(); });
        //chat.setMessageAsRead();
        if ($("#chat-sendmessagebtn").attr("data-isverify") == 0) {
            mobileverification.initEvents();
        } 
    };

    this.addMessage = function () {
        if ($("#chat-sendmessagebtn").attr("data-isverify") == 0) {
            $("#chat-verify-mobile").trigger("click");
        }
        else {
            var messageCtl = $("#message-content");
            var savePath = "/MessageHelper/AddMessage";
            var messageContent = messageCtl.val();
            var mobile = $("#chat-sendmessagebtn").attr("data-mobile");

            if (messageContent == undefined || $.trim(messageContent) == "") {
                messageCtl.addClass("inputerror");
            } else {
                messageCtl.removeClass("inputerror");
                var senderName = $("#classdetail-hidmsname").val();
                var receiverName = $("#classdetail-hidmrname").val();
                var receiverEmail = $("#classdetail-hidmremail").val();
                var toId = $("#chat-hidcontactorid").val();
                var paraData = { to: toId, message: messageContent, msname: senderName, mremail: receiverEmail, mrname: receiverName, mobile: mobile };
                if (typeof (mixpanel) != "undefined") {
                    mixpanel.track("chat on chatpage");
                }

                $.ajax({
                    url: savePath,
                    type: "POST",
                    dataType: "Json",
                    data: paraData,
                    cache: false,
                    success: function (data) {
                        if (data == "true") {
                            messageCtl.val("");
                            window.location.reload();
                        }
                    }
                });
            }
        }
    }

}

