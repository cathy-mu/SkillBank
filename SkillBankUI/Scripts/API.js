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
            apitest.updatePassword();
        });

        $("#addcourse").click(function () {
            apitest.addCourseInfo();
        });

        $("#updatecourse").click(function () {
            apitest.updateCourseInfo(false);
        });

        $("#pubcourse").click(function () {
            apitest.updateCourseInfo(true);
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
        var name = $("#memberinfo-name").val();
        var gender = $("#memberinfo-gender").val();
        var avatar = $("#memberinfo-avatar").val();
        var city = $("#memberinfo-city").val();
        var intro = $("#memberinfo-intro").val();
        var id = 1;
        var paraData = { "Id": id, "Name": name, "Gender": gender, "Intro": intro, "CityName": city, "Avatar": avatar };
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

    this.updatePassword = function () {
        var pass = $("#memberinfo-pass").val();
        var code = $("#memberinfo-code").val();
        var mobile = $("#memberinfo-mobile").val();
        var id = 1;

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


    this.updateCourseInfo = function (isPublish) {
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
        
        var paraData = { "CityName": cityName, "Title": title, "Summary": summary, "level": level, "skill": skill, "teach": teach, "category": category, "WhyU": whyu, "location": location, "period": period, "available": available, "remark": remark };
        console.log(paraData);

        var savePath = "/API/Course/" + courseId;
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

        var paraData = { "CityName": cityName, "Title": title, "Summary": summary, "level": level, "skill": skill, "teach": teach, "category": category, "WhyU": whyu, "location": location, "period": period, "available": available, "remark": remark };
        console.log(paraData);

        var savePath = "/API/Course/";
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
        
    this.verfyMobileCode = function () {
        var mobile = $("#valid-mobile").val();
        var code = $("#valid-code").val();
        var memberid = $("#valid-memberid").val();

        var paraData = { "mobile": mobile, "code": code, "memberid": memberid };
        var savePath = "/api/verification";
        
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
    
    this.sendValidCode = function (type) {
        var mobile = $("#valid-mobile").val();
        var memberid = $("#valid-memberid").val();

        var paraData = { "type": type, "mobile": mobile, "code": "", "memberid": memberid };
        var savePath = "/api/verification";
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
        //var name = $("#member-name").val();
        //var code = $("#member-code").val();
        //var mobile = $("#member-mobile").val();
        //var account = $("#member-account").val();
        //var avatar = $("#member-avatar").val();
        //var type = 1;
        ////var paraData = { "Mobile": mobile, "Name": name, "Type": type, "Account": account, "Avatar": avatar, "Code": code }; 
        //var paraData = { "Mobile": mobile, "Name": name, "Type": 2, "Avatar": avatar, "Code": code , "Pass": "123sdg$%"};
        //var savePath = "/API/Registe";
        //console.log(paraData);
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
        
     
        //var paraData = { "Mobile": mobile, "Name": name, "Type": type, "Account": account, "Avatar": avatar, "Code": code }; 
        var paraData = { account: "1885646861", socialType: "1", memberName: "cathytest", email: "cathy.test@hotmail.com", avatar: "http://tp2.sinaimg.cn/1885646861/180/1298686485/0"};
        var savePath = "/MemberHelper/CreateMember";
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