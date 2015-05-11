var memberinfo = memberinfo || new memberinfo_Class;
$(document).ready(function () { memberinfo.init(); });

function memberinfo_Class() {
    this.init = function () {
        this.ctlCityId = $("#profileinfo-hidcityid");
        this.cityCtlId = "profileinfo-city";
        this.SearchCityPath = "/ClassHelper/SearchCity";
        this.initEvents();

    };

    this.initEvents = function () {
        sitecommon.showLoginPop(false);
        $("#" + this.cityCtlId).keyup(function () { memberinfo.ctlCityId.val(0); $("#" + this.cityCtlId).removeClass("inputerror"); });
        $("#profileinfo-save").click(function () { memberinfo.updateMemberInfo(); });
        $("input").click(function () { $("#profileinfo-save").removeClass("saved").addClass("btn-primary").find("span").text("保存"); });
    }

    this.validMobile = function (mobile) {
        var patten = new RegExp(/^[1]+\d{10}$/);
        return (mobile != "" && patten.test(mobile));
    }
        
    this.searchCity = function () {
        var isValid = false;
        var cityId = memberinfo.ctlCityId.val();
        if (cityId == 0) {
            var key = $.trim($("#" + this.cityCtlId).val());
            if (key.length > 0) {
                var paraData = { searchKey: key, isMatch: true };
                var savePath = memberinfo.SearchCityPath;
                $.ajax({
                    url: savePath,
                    type: "POST",
                    dataType: "Json",
                    data: paraData,
                    cache: false,
                    async: false,
                    success: function (data) {
                        consoleLog(data);
                        if (data && data.length == 1) {
                            $("#" + this.cityCtlId).val(data[0].Text);
                            memberinfo.ctlCityId.val(data[0].Id);
                            isValid = true;
                        }
                    }
                });
            }
        } else if (cityId !=undefined &&  cityId > 0) {
            isValid = true;
        }
        return isValid;
    }

    this.getInputValue = function (ctlid) {
        var value = $.trim($(ctlid).val());
        if (value == "") {
            $(ctlid).addClass("inputerror");
            return "";
        } else {
            $(ctlid + ".inputerror").removeClass("inputerror");
            return value;
        }
    }
    
    this.getSelectValue = function (ctlid) {
        var value = $.trim($(ctlid).val());
        if (value == "") {
            $(ctlid).next("div.chosen-container").find("a").addClass("selecterror");
            return "";
        } else {
            $(ctlid).next("div.chosen-container").find("a.selecterror").removeClass("selecterror");
            return value;
        }
    }


    // Update member info
    this.updateMemberInfo = function () {
        //TO DO:City selector
        
        var name = $("#profileinfo-name").val().replace(/[ ]/g, "");
        var patten = new RegExp(/(skillbank|技能银行|客服)/);
        
        if (name == "" || patten.test($.trim(name))) {
            $("#profileinfo-name").addClass("inputerror");
            return false;
        } else {
            $("#profileinfo-name").removeClass("inputerror");
        }

        var email = this.getInputValue("#profileinfo-email");
        if (email == "") {
            return false;
        }

        var phone = this.getInputValue("#profileinfo-phone");
        if (!memberinfo.validMobile(phone)) {
            $("#profileinfo-phone").addClass("inputerror");
            return false;
        }

        var intro = this.getInputValue("#profileinfo-intro");
        if (intro == "") {
            return false;
        }

        var isMale = this.getSelectValue("#profileinfo-gender");
        if (isMale == "") {
            return false;
        }

        var year = this.getSelectValue("#profileinfo-year");
        if (year == "") {
            return false;
        }
        var month = this.getSelectValue("#profileinfo-month");
        if (month == "") {
            return false;
        }

        var day = $("#profileinfo-day").val();
        if (day == "" || !sitecommon.validDate(year, month, day)) {
            $("#profileinfo-day").next("div.chosen-container").find("a").addClass("selecterror");
            return false;
        } else {
            $("#profileinfo-day").next("div.chosen-container").find("a.selecterror").removeClass("selecterror");
        }
        var validCity = memberinfo.searchCity();
        if (validCity) {
            $("#" + this.cityCtlId).removeClass("inputerror");
        } else {
            $("#" + this.cityCtlId).addClass("inputerror");
            return false;
        }
        var cityId = this.ctlCityId.val();

        var paraData = { "name": name, "isMale": isMale, "year": year, "month": month, "day": day, "email": email, "phone": phone, "cityId": cityId, "intro": intro };
        consoleLog(paraData);
        $.ajax({
            url: configSitePath + "/MemberHelper/UpdateMemberProfile",
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                $("#profileinfo-save").addClass("saved").removeClass("btn-primary").find("span").text("已保存");
            }/*, error: function (e) {
                console.log(e);
            }*/
        });
    }


}




