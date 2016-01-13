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

        $("#sendcodebtn").click(function () {
            apitest.sendValidCode(1);
        });

        $("#sendmobilebtn").click(function () {
            apitest.sendValidCode(5);
        });

        $("#verifybtn").click(function () {
            apitest.verfyMobileCode();
        });               

        $("#creatememberbtn").click(function () {
            apitest.createMember();
        });

        $("#updatemember").click(function () {
            apitest.updateMember();
        });

        $("#updateprofile").click(function () {
            apitest.updateProfile();
        });

        $("#updatepassword").click(function () {
            apitest.updatePassword(false);
        });

        $("#resetpassword").click(function () {
            apitest.updatePassword(true);
        });

        $("#addcourse").click(function () {
            apitest.addCourseInfo();
        });

        $("#updatecourse1").click(function () {
            apitest.updateCourseInfo(1);
        });

        $("#updatecourse2").click(function () {
            apitest.updateCourseInfo(2);
        });

        $("#pubcourse").click(function () {
            apitest.updateCourseInfo(3);
        });

        $("#addorder").click(function () {
            apitest.addOrder();
        });

        $("#updateorder").click(function () {
            apitest.updateOrder();
        });

        $("#updateorderdate").click(function () {
            apitest.updateOrderDate();
        });

        $("#studentreviewbtn").click(function () {
            apitest.addReview(true);
        });

        $("#teacherreviewbtn").click(function () {
            apitest.addReview(false);
        });

        $("#coinupdatebtn").click(function () {
            apitest.getShareClassCoins();
        });

    };

    this.addReview = function (isStudent) {
        var orderId = $("#review-orderid").val();
        var comment = $("#review-comment").val();
        var feedback = $("#review-feedback").val();
        
        var classId = 0;
        if (orderId == undefined || orderId == "") {
            alert("please enter order id");
            return false;
        }

        if (feedback != 1 && feedback != 2 && feedback != 3) {
            alert("please enter feedback between 1 and 3");
            return false;
        }

        var paraData = { "OrderId": orderId, "IsStudent": isStudent, "Feedback": feedback,  "Comment": comment };
        console.log(paraData);
        var savePath = "/API/OrderReview";
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

    this.updateMember = function () {
        var name = $("#memberinfo-name").val();
        var gender = $("#memberinfo-gender").val();
        var avatar = $("#memberinfo-avatar").val();
        var city = $("#memberinfo-city").val();
        var intro = $("#memberinfo-intro").val();


        var paraData = { "Name": name, "Gender": gender, "Intro": intro, "CityName": city, "Avatar": avatar };
        console.log(paraData);

        var savePath = "/API/Member/";
        $.ajax({
            url: savePath,
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.updateProfile = function () {
        var id = $("#memberinfo-id").val();
        var name = $("#memberinfo-name").val();
        var gender = $("#memberinfo-gender").val();
        var avatar = $("#memberinfo-avatar").val();
        var city = $("#memberinfo-city").val();
        var intro = $("#memberinfo-intro").val();
        //var paraData = { "Id": id, "Name": name, "Gender": gender, "Intro": intro, "CityName": city, "Avatar": avatar };
        var paraData = { "Name": "cathy1", "Gender": 1, "CityName": "上海市" };
        //var paraData = { "Mobile": "13564813923", "Code": "999999", "Password": "abcd1234" };
        //var paraData = { "Intro": "Here is my test introduction for API" };

        var paraData = { "PosX": 116.225426, "PosY": 40.148060 };
        var savePath = "/API/profile/" + id;
        console.log("path : "+savePath);
        console.log("para : " + paraData);
        $.ajax({
            url: savePath,
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                console.log("result : " + data);
            }
        });
    }
    
    this.updatePassword = function (isReset) {
        var pass = $("#memberinfo-pass").val();
        var code = $("#memberinfo-code").val();
        var mobile = $("#memberinfo-mobile").val();
        var id = isReset?0:$("#memberinfo-id").val();

        var paraData = { "Id": id, "Password": pass, "Mobile": mobile, "Code": code };
        console.log(paraData);

        var savePath = "/API/profile/"+id;
        $.ajax({
            url: savePath,
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }


    this.updateCourseInfo = function (step) {
        var courseId = $("#course-id").val();
        var cityName = $("#course-city").val();
        var title = $("#course-title").val();
        var summary = $("#course-summary").val();
        var whyu = $("#course-whyu").val();
        var available = $("#course-available").val();
        var location = $("#course-location").val();
        var period = $("#course-period").val();
        var category = $("#course-category").val();
        var level = $("#course-level").val();
        var skill = $("#course-skill").val();
        var teach = $("#course-teach").val();
        var remark = $("#course-remark").val();
        var tags = $("#course-tags").val();
        var online = $("#course-online").val();

        if (step == 1) {
            var paraData = {
                "CityName": cityName,
                "Title": title, //"Summary": summary,
                "category": category, "level": level, "skill": skill, "teach": teach
                //,"category": category,
                //"WhyU": whyu, "location": location, "period": period, "available": available, "remark": remark,
                //"Tags":tags, "HasOnline":(online=="1")
            };
        } else if (step == 2) {
            var paraData = {
                "Summary": summary,
                "WhyU": whyu,
                "location": location, "period": period, "available": available, "remark": remark,
                "Tags": tags,
                "HasOnline": online == "1"
            };
        } else {
        }
        console.log(paraData);
        var savePath = "/API/Course/" + courseId;

        $.ajax({
            url: savePath,
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                console.log(data);
            }
        });
    }

    this.addCourseInfo = function () {
        var cityName = $("#course-city").val();
        var title = $("#course-title").val();
        var summary = $("#course-summary").val();
        var whyu = $("#course-whyu").val();
        var available = $("#course-available").val();
        var location = $("#course-location").val();
        var period = $("#course-period").val();
        var category = $("#course-category").val();
        var level = $("#course-level").val();
        var skill = $("#course-skill").val();
        var teach = $("#course-teach").val();
        var remark = $("#course-remark").val();
        var tags = $("#course-tags").val();
        var online = $("#course-online").val();
        var category = $("#course-category").val();

        
        var paraData = {
            "CityName": cityName,
            "Title": title, //"Summary": summary,
            "category": category, "level": level, "skill": skill, "teach": teach
        };
   
        console.log(paraData);

        var savePath = "/API/Class/";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                console.log(data);
            }
        });
    }

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

    
    //For orders
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

    this.updateOrder = function () 
    {
        var orderId = $("#order-id").val();
        var memberId = $("#order-mid").val();
        var classId = $("#order-cid").val();
        var statusId = $("#order-status").val();
        var remark = $("#order-remark").val();
        var name = $("#order-name").val();
        var mobile = $("#order-mobile").val();
        var title = $("#order-title").val();
        var paraData = {
            "memberId": memberId,
            "Status": statusId,
            "Title": title,
            "Name": name,
            "Phone": mobile,
            "Email": "test@send.com",
            "Feedback": "feedback",
            "Comment": "comment",
            "MyId": 1
        };
        //var paraData = { "memberId": memberId, "Status": statusId, "name": name, "Phone": mobile };
        var savePath = "/API/Order/" + orderId;
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.updateOrderDate = function () {
        var orderId = $("#order-id").val();
        var paraData = {
            "Status": 0,
            "BookDate": "2015-10-10"
        };
        var savePath = "/API/Order/" + orderId;
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                alert(data);
            }
        });
    }

    this.getShareClassCoins = function () {
        var memberid = $("#coin-memberid").val();
        var paraData = { MemberId: memberid, UpdateType: 0 };
        console.log(paraData);

        $.ajax({
            url: 'http://www.skill-bank.com/api/coins',
            type: "PUT",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (data) {
                    alert("success");
                }
            }
        });

    }
            
    this.verfyMobileCode = function () {
        var mobile = $("#valid-mobile").val();
        var code = $("#valid-code").val();
        var memberid = $("#valid-memberid").val();

        //var paraData = { "mobile": mobile, "code": code, "memberid": memberid };
        var paraData = { "Type": 3, "mobile": "13917782601", "code": "999999", "memberid": 62 };
        var savePath = "/api/Validation";
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                console.log(data);
            }
        });
    }
    
    this.sendValidCode = function (type) {
        var mobile = $("#valid-mobile").val();
        var memberid = $("#valid-memberid").val();

        //var paraData = { "type": type, "mobile": mobile, "code": "", "memberid": memberid };
        var paraData = { "Type": 6, "mobile": "13127853967" };
        var savePath = "/api/Validation";
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
        //var paraData = { "Mobile": mobile, "Name": name, "Type": 2, "Avatar": avatar, "Code": code, "Pass": "123456", "DeviceToken": "abcdeabcdeabcde", "UnionId": "123451234512345" };
        //var paraData = { "Account": "13500000000", "Name": name, "Type": 2, "Avatar": avatar, "Code": code };
        var paraData = { "Account": "0000000001", "Name": "TestName", "Type": 2, "Avatar": "my avatar", "Type": 5 };
        var savePath = "/API/Registe";
        //var paraData = { "Mobile": "13700000000", "Name": "TestName", "Type": 2, "Avatar": "my avatar", "Code": "999999", "Pass": "mypass" };
        console.log(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                console.log(data);
                if (data.Result != undefined) {
                    data = data.Result;
                }
                console.log(data);
            }
        });
        
     
        ////var paraData = { "Mobile": mobile, "Name": name, "Type": type, "Account": account, "Avatar": avatar, "Code": code }; 
        //var paraData = { account: "1885646861", socialType: "1", memberName: "cathytest", email: "cathy.test@hotmail.com", avatar: "http://tp2.sinaimg.cn/1885646861/180/1298686485/0"};
        //var savePath = "/MemberHelper/CreateMember";
        //console.log(paraData);
        //$.ajax({
        //    url: savePath,
        //    type: "POST",
        //    dataType: "Json",
        //    data: paraData,
        //    cache: false,
        //    success: function (data) {
        //        console.log(data);
        //    }
        //});
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