﻿$(document).ready(function () {
    if ($("#btnsetmaster").length > 0) {
        $('#btnsetmaster').click(function () {
            var memberId = $("#profile-hidmemberid").val();
            var paraStr = "";
            $(".master-groupitem").each(function () {
                var $this = $(this);
                var rank = $this.find(".master-channellink").val();
                var id = $this.attr("data-id");
                if ($(this).find(".master-groupset").parent().hasClass("checked")) {
                    paraStr += (id + ",");
                    paraStr += ($(this).find(".master-channellink").val() + ";");
                }

            });

            var paraData = { "memberId": memberId, "paraStr": paraStr };
            var savePath = "/Tools/SaveMasterMember";
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
        });
    } else {
        var llCookieName = "debugll";
        var memberIdCookieName = "mid";
        $("#showll").click(function () {
            $.cookie(llCookieName, 'y', { expires: 30, path: '/' });
        });

        $('#hidell').click(function () {
            $.cookie(llCookieName, 'n', { expires: 30, path: '/' });
        });

        $('#setmemberid').click(function () {
            var memberId = $('#memberId').val();
            if (memberId != "") {
                sitecommon.setCookie(memberIdCookieName, memberId);
            }
        });

        $('#previewbtn').click(function () {
            if ($("#classId").val() != "") {
                window.open("/class/preview/" + $("#classId").val());
            }
        });

        $("#viewbtn").click(function () {
            if ($("#classId1").val() != "") {
                window.open("/tools/classview/" + $("#classId1").val());
            }
        });

        $("#btnsearche").click(function () {
            if ($("#email").val() != "") {
                window.open("/tools/SearchMemeber/?t=4&k=" + $("#email").val());
            }
        });
        $("#btnsearchn").click(function () {
            if ($("#name").val() != "") {
                window.open("/tools/SearchMemeber/?t=5&k=" + $("#name").val());
            }
        });
        $("#btnsearchp").click(function () {
            if ($("#phone").val() != "") {
                window.open("/tools/SearchMemeber/?t=6&k=" + $("#phone").val());
            }
        });
        $("#btnsearchs").click(function () {
            if ($("#email").val() != "") {
                window.open("/tools/SearchMemeber/?t=3&k=" + $("#social").val());
            }
        });

        $("#setmaster").click(function () {
            if ($("#masterMemberId").val() != "") {
                window.open("/tools/memberprofile/" + $("#masterMemberId").val());
            }
        });

        function coinUpdate(saveType, memberId, classId, coinNum) {
            var paraData = { type: saveType, member: memberId, id: classId, amount: coinNum };
            $.ajax({
                url: "/Tools/CoinUpdate",
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                async: false,
                success: function (data) {
                    if (saveType == 1 && data == "true") {
                        $('#labcoin').text("已有分享课币记录  用户ID" + memberId);
                    }
                    else if (saveType == 1) {
                        $('#labcoin').text("无分享课币记录  用户ID" + memberId);
                    } else {
                        alert("加好了呢");
                    }
                }
            });

        }

        $('#btn1').click(function () {
            var memberId = $('#memberid1').val();
            if (memberId != "" && memberId > 0) {
                coinUpdate(1, memberId, 0, 0);
            } else {
                alert("亲，别闹了。就只要MemberId，你都不告诉我。");
            }
        });

        $('#btn2').click(function () {
            var memberId = $('#memberid2').val();
            var amount = $('#coin2').val();
            if (memberId != "" && memberId > 0 && amount != "" && amount > 0) {
                coinUpdate(2, memberId, 0, amount);
            } else {
                alert("臣妾做不到。麻烦你检查下参数都填好了吗？");
            }
        });

        $('#btn3').click(function () {
            var memberId = $('#memberid3').val();
            var amount = $('#coin3').val();
            if (memberId != "" && memberId > 0 && amount != "") {
                coinUpdate(3, memberId, 0, amount);
            } else {
                alert("臣妾做不到。麻烦你检查下参数都填好了吗？");
            }
        });

        $('#btn4').click(function () {
            var classId = $('#classid4').val();
            var amount = $('#coin4').val();
            if (classId != "" && classId > 0 && amount != "" && amount > 0) {
                coinUpdate(4, 0, classId, amount);
            } else {
                alert("臣妾做不到。麻烦你检查下参数都填好了吗？");
            }
        });

        $('#btn5').click(function () {
            var memberId = $('#memberid5').val();
            if (memberId != "" && memberId > 0) {
                coinUpdate(5, memberId, 0, 0);
            } else {
                alert("亲，别闹了。就只要MemberId，你都不告诉我。");
            }
        });

        $('#btn6').click(function () {
            var memberId = $('#memberid6').val();
            var amount = $('#coin6').val();
            if (memberId != "" && memberId > 0 && amount != "") {
                coinUpdate(6, memberId, 0, amount);
            } else {
                alert("臣妾做不到。麻烦你检查下参数都填好了吗？");
            }
        });

        $("#getcoinbackbtn").click(function () {
            var paraData = { "type": 10, "member": 0, "id": 0, "amount": 0 };
            console.log(paraData);
            $.ajax({
                url: "/Tools/CoinUpdate",
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                async: false,
                success: function (data) {
                    alert("课币收回来啦");
                }
            });
        });

        $("#logoutbtn").click(function () {
            $.ajax({
                url: "/SignUp/LogOutSocialAccount",
                type: "POST",
                dataType: "Json",
                cache: false,
                success: function (data) {
                    alert("退出登录");
                }, error: function (e) {
                    //consoleLog(e);
                }
            });

            sitecommon.setCookie(sitecommon.memberIdCookieName, 0);
            sitecommon.removeCookie("sai");
            sitecommon.removeCookie("sid");
            sitecommon.removeCookie(sitecommon.socialIdCookieName);
            sitecommon.removeCookie(sitecommon.socialTypeCookieName);

        });


        $("#unbindmobile").click(function () {
           var mobile = $('#mobile').val();
            var patten = new RegExp(/^[1]+[3,4,5,8]+\d{9}/);
            if (mobile=="" || !patten.test(mobile)) {
                alert("我读书少你不要骗我，你填的是手机号吗？");
                return;
            }
            var paraData = { "type":11, "account": mobile };
            $.ajax({
                url: "/Tools/UnbindAccount",
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                async: false,
                success: function (data) {
                    alert("Oh Yeah! 手机解绑了呢 , 好嗨森");
                }
            });

        });

        $("#unbindsid").click(function () {
            var pass = $('#passcode').val();
            var sid = $('#accountsid').val();
            var patten = new RegExp(/^[2,3,6,8,0]+\d{3}/);
            if (pass == "" || !patten.test(pass)) {
                alert("我只想做一段安静的程序，麻烦你填对码，确认你是认真的好吗");
                return;
            }
            if (sid == "") {
                alert("你不确定不想告诉我你的社交账户id？");
                return;
            }
            var paraData = { "type": 13, "account": sid };
            $.ajax({
                url: "/Tools/UnbindAccount",
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                async: false,
                success: function (data) {
                    alert("痛哭着通知你，你的社交账号自由了");
                }
            });

        });

        $("#unbindmid").click(function () {
            var pass = $('#passcode').val();
            var mid = $('#accountmid').val();
            var patten = new RegExp(/^[1,3,5,7,9]+\d{3}/);
            if (pass == "" || !patten.test(pass)) {
                alert("我只想做一段乖巧的程序，麻烦你填对码，发誓你是认真的好吗");
                return;
            }
            if (mid == "") {
                alert("你不确定不想告诉我你的用户id？");
                return;
            }
            var paraData = { "type": 12, "account": mid };
            $.ajax({
                url: "/Tools/UnbindAccount",
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                async: false,
                success: function (data) {
                    alert("好吧，我放你走，别再理我");
                }
            });



        });
    }

});