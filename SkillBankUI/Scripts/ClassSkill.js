var classskill = classskill || new classskill_Class;
$(document).ready(function () { classskill.init(); });

function classskill_Class() {
    this.init = function () {
        this.ctlCatePId = $("#classskill-hidcategorypid");//for parent category
        this.ctlCateId = $("#classskill-hidcategoryid");//catagory id for submit
        this.cateIdPrefix = "#class-category-list";
        this.subCateIdPrefix = "#class-subcategory-list";
        this.ctlCityId = $("#classskill-hidcityid");
        this.cityCtlId = "classskill-city";
        this.SearchCityPath = "/ClassHelper/SearchCity";
        this.initEvents();
        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("list skill");
        }
    };

    this.initEvents = function () {
        $("#classf-createbtn").click(function () { classskill.createNewClass(); });
        $("#" + this.cityCtlId).keyup(function () { classskill.ctlCityId.val(0); $("#" + this.cityCtlId).removeClass("inputerror"); });
        this.initCategory();
    }

    this.initCategory = function () {
        $(classskill.subCateIdPrefix + "-frame").hide();
        //For category dropdowns
        $(this.cateIdPrefix).change(function () {
            var selObj = $(this).children('option:selected');
            classskill.ctlCatePId.val(selObj.val());
            var subData = selObj.attr("data-id");
            if (subData == undefined || subData == "") {
                classskill.ctlCateId.val(selObj.val());
                $(classskill.subCateIdPrefix + "-frame").hide();
            }
            else {
                classskill.ctlCateId.val("");
                $(classskill.subCateIdPrefix).next("div").find("a.chosen-single span").text($(classskill.subCateIdPrefix).children('option:eq(0)').text());
                $(classskill.subCateIdPrefix + "-frame").show();
            }
        });

        $(this.subCateIdPrefix).change(function () {
            classskill.ctlCateId.val($(this).children('option:selected').val());
        });
    }
         
    this.searchCity = function () {
        var isValid = false;
        var cityId = classskill.ctlCityId.val();
        $("#classskill-hidcityhasclass").val("");
        if (classskill.ctlCityId.val() == 0) {
            var key = $.trim($("#" + this.cityCtlId).val());
            if (key.length > 0) {
                var paraData = { searchKey: key, isMatch: true };
                var savePath = classskill.SearchCityPath;
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
                            classskill.ctlCityId.val(data[0].Id);
                            $("#classskill-hidcityhasclass").val(data[0].HasClass ? "" : "1");
                            isValid = true;
                        }
                    }
                });
            }
        } else if (cityId != undefined && cityId > 0) {
            isValid = true;
        }
        //return false;
        return isValid;
    }

    this.createNewClass = function () {
        var isFirstInCity = 0;
        var memberId = sitecommon.getMemberId();
        var categoryId = classskill.ctlCateId.val();
        var cityCtl = classskill.ctlCityId; 
          
        
        if (categoryId == "") {
            if (classskill.ctlCatePId.val() == "") {
                $(classskill.cateIdPrefix).next("div.chosen-container").find("a").addClass("selecterror");
            }
            else {
                $(classskill.subCateIdPrefix).next("div.chosen-container").find("a").addClass("selecterror");
                $(classskill.cateIdPrefix).next("div.chosen-container").find("a.selecterror").removeClass("selecterror");
            }
            return false;
        } else {
            $(classskill.cateIdPrefix).next("div.chosen-container").find("a.selecterror").removeClass("selecterror");
            $(classskill.subCateIdPrefix).next("div.chosen-container").find("a.selecterror").removeClass("selecterror");
        }

        
        var validCity = classskill.searchCity();
        if (validCity) {
            $("#" + this.cityCtlId).removeClass("inputerror");
        } else {
            $("#" + this.cityCtlId).addClass("inputerror");
            return false;
        }
        var cityId = this.ctlCityId.val();
       
        var skillLevel = $("#skill-slider-input").val();
        var teachLevel = $("#level-slider-input").val();
        
        //login member , create class now
        if (memberId != null && memberId > 0) {
            var savePath = "/ClassHelper/AddClass";
            var cityNoClass = ($("#classskill-hidcityhasclass").val() == "1");
            var paraData = { "categoryId": categoryId, "skillLevel": skillLevel, "teachLevel": teachLevel, "cityId": cityId, "cityNoClass": cityNoClass };
            consoleLog(paraData);
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("add class"); //click btn on list skill
            }
            $.ajax({
                url: configSitePath + savePath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                    var locationPath = "";
                    var result = data.Data;
                    if (result.type == 1) {
                        location.href = configSitePath + "/class/add/" + result.classId;
                    } else  {
                        location.href = configSitePath + "/class/add/" + result.classId + "?edit=1";// should load exist class info
                    }
                }
            });
        } 
    }
    
}



