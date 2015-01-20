$(document).ready(function () {
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
                $.cookie(memberIdCookieName, memberId, { expires: 30, path: '/' });
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

    }
});