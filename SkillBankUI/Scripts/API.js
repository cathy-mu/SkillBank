var apitest = apitest || new apitest_Class;

$(document).ready(function () { apitest.init(); });

function apitest_Class() {
    this.init = function () {
     
        $("#addchatbtn").click(function () {
            apitest.saveChatMessage();
        });

        $("#followbtn").click(function () {
            apitest.updateFollowTag();
        });

        $("#likebtn").click(function () {
            apitest.updateLikeTag();
        });

        $("#getclassbtn").click(function () {
            apitest.getClassList();
        });

        $("#addorderbtn").click(function () {
            apitest.addOrder();
        });

        $("#sendcodebtn").click(function () {
            apitest.sendValidCode();
        });

        $("#creatememberbtn").click(function () {
            apitest.createMember();
        });


    };

    this.saveChatMessage = function () {

        var fromId = $("#chat-fid").val();
        var toId = $("#chat-tid").val();
        var message = $("#chat-message").val();
        var paraData = { "fromId": fromId, "toId": toId, "MessageText": message };
        var savePath = "/API/Chat";
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.updateLikeTag = function () {

        var memberId = $("#like-mid").val();
        var classId = $("#like-cid").val();
        alert(classId);
        var isLike = ($("#like-isl").val() == 1);
        var paraData = { "MemberId": memberId, "ClassId": classId, "IsLike": isLike };
        var savePath = "/API/LikeClass";
        console.log(paraData);
        //$.ajax({
        //    url: savePath,
        //    type: "POST",
        //    dataType: "Json",
        //    data: paraData,
        //    cache: false,
        //    success: function (data) {
        //        alert(data);
        //    }
        //});
    }


    this.updateFollowTag = function () {

        var fromId = $("#follow-mid").val();
        var toId = $("#follow-fid").val();
        var isFollow = ($("#follow-isf").val() == 1);
        var paraData = { "memberId": fromId, "FollowingId": toId, "IsFollow": isFollow };
        var savePath = "/API/FollowMember";
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.updateLikeTag = function () {

        var memberId = $("#like-mid").val();
        var classId = $("#like-cid").val();
        var isLike = ($("#like-isl").val() == 1);
        var paraData = { "memberId": memberId, "classId": classId, "IsLike": isLike };
        var savePath = "/API/LikeClass";
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }


    this.addOrder = function () {
        var memberId = $("#order-mid").val();
        var classId = $("#order-cid").val();
        var bookDate = $("#order-date").val();
        var remark = $("#order-remark").val();
        var name = $("#order-name").val();
        var mobile = $("#order-mobile").val();
        var paraData = { "memberId": memberId, "ClassId": classId, "BookDate": bookDate, "Remark": remark, "name": name, "mobile": mobile };
        var savePath = "/API/Order";
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.sendValidCode = function () {
        var mobile = $("#valid-mobile").val();
        var paraData = { "mobile": mobile };
        var savePath = "/API/Validation?mobile="+mobile;
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.createMember = function () {
        var name = $("#member-name").val();
        var code = $("#member-code").val();
        var mobile = $("#member-mobile").val();
        var account = $("#member-account").val();
        var avatar = $("#member-avatar").val();
        var type = 1;
        var paraData = { "Mobile": mobile, "Name": name, "Type": type, "Account": account, "Avatar": avatar, "Code": code };
        var savePath = "/API/Member";
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                //alert(data);
            }
        });
    }

    this.getClassList = function () {

        $.getJSON("http://api.map.baidu.com/geocoder/v2/?ak=RZG8k5YBPYBRvU64kWTVLDsK&callback=renderReverse&location=39.9835000,116.322987&output=xml&pois=1",
                function (data) {
                    // On success, 'data' contains a list of products. 
                    $.each(data, function (key, val) {
                        var str = val.status;
                        console.log(str);
                    });

                });

        //$.getJSON("http://www.skillbank.cn/api/ClassList/?by=1&type=1",
        //        function (data) {
        //            // On success, 'data' contains a list of products. 
        //            $.each(data, function (key, val) {
        //                var str = val.Title + ' : ' + val.ClassId;
        //                console.log(str);
        //            });
                    
        //        });
    }

}