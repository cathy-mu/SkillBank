var classedit = classedit || new classedit_Class;
$(document).ready(function () {
    classedit.init();
    photoEditor.init($("#class-coverupload-file"), 403, 238, 178, 0, 0, 0);
});

function classedit_Class() {
    this.classId = $("#classf-hidclassid").val();
    var stepsLeft = 3;
    var firstFormAdding = 1;
    var secondFormAdding = 1;
    var thirdFormAdding = 1;

    this.init = function () {
        this.isEditMode = ($("#class-subcategory-list").length > 0);
        if (this.isEditMode) {
            this.ctlCatePId = $("#classskill-hidcategorypid");//for parent category
            this.ctlCateId = $("#classskill-hidcategoryid");//catagory id for submit
            this.cateIdPrefix = "#class-category-list";
            this.subCateIdPrefix = "#class-subcategory-list";
            this.initCategory();
        }

        this.initEvents();
    };

    // Upload class cover
    this.updateClassCover = function () {
        var option = {
            type: "post",
            dataType: "json",
            url: "/API/UploadCover",
            success: function (data) {
                //classedit.saveClassInfo(12, "/class/c_" + $("imagefilename").val() + $("imagefileext").val());
                $("#classhascover").val(true);
                classedit.formValidation(3);
                $("#btnimg-upload").addClass("saved").removeClass("btn-primary").find("span").text("已保存");
            }
        }
        $("#form-class-photo").ajaxSubmit(option);
    }

    this.initEvents = function () {
        /*if ($(".create-class-hd").length > 0) {
            $(".create-class-hd").scrollToFixed({
                marginTop: 50,
                limit: 0,
                bottom: -1,
                zIndex: 1000,
                baseClassName: 'scroll-to-fixed-fixed'
            });
        }*/

        this.initSavingEvent();
        this.stepValidation();
        //this.initTags();
        this.initCategoryHits("");
        
        $("#class-coverupload").click(function () { $("#class-coverupload-file").trigger("click"); });
        $("#btnimg-upload").click(function () { classedit.updateClassCover(); });

        ////For city dropdowns
        ////$("#classf_city_list li").click(function () { classedit.selectCity(this); });
        //$("#classf_cityanme").keyup(function () { classedit.initCity(); });
        this.formValidation(0);
    }

    this.initSavingEvent = function () {
        /* selfintro  member-gender position*/
        $("#selfintro").blur(function () {
            if ($.trim($(this).val()) != "") {
                classedit.updateMemberInfo(5, $(this).val());
            }
        });
        $("#class-membergender .iCheck-helper").click(function () {
            classedit.updateMemberInfo(6, $(this).parent().parent().hasClass("member-male") ? 1 : 0);
        });

        $("#title").blur(function () {
            if ($.trim($(this).val()) != "") {
                classedit.saveClassInfo(6, $(this).val());
            }
        });
        $("#summary").blur(function () {
            if ($.trim($(this).val()) != "") {
                classedit.saveClassInfo(7, $(this).val());
            }
        });
        $("#detail").blur(function () {
            classedit.saveClassInfo(9, $(this).val());
        });
        $("#class-level .iCheck-helper").click(function () {
            classedit.saveClassInfo(8, $(this).parent().parent().attr("data-value"));
        });
        $("#detail").blur(function () { classedit.saveClassInfo(9, $(this).val()); });

        $("#class-publish").click(function () { classedit.checkPublishClass(); });
        //$("#confirm-publish-sub").click(function () { classedit.publishClass(); });
        $("#confirm-publish-update").click(function () { $('#modal-confirm-publish .close').trigger("click"); $('#tab-class-overview a').trigger("click"); });
    }

    this.changePosition = function () {
        classedit.formValidation(1);
    }

    //this.initTags = function () {
    //    $('#classf-tags').tagsInput({ width: 'auto' });
    //    $('div.tagssuggestion span.tag').click(function () { $('#classf-tags').addSuggestionTags($(this)); });
    //}

    this.checkPublishClass = function () {
        var summaryLength = $("#summary").val().length;
        if (summaryLength < 150) {
            $("#class-confirm-publish").trigger("click");
        } else {
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("publish class");
            } 
            classedit.publishClass();
        }
    }

    this.publishClass = function () {
        classedit.saveClassInfo(14, true);
        $("#class-publish").addClass("btn-default").removeClass("btn-primary");
    }

    this.formValidation = function (tabid) {
        if (tabid == 1 || tabid == 0) {//about me or skill
            var isValid;
            if (classedit.isEditMode) {
                isValid = (classedit.ctlCateId.val() != "");
            } else {
                isValid = ($("#selfintro").val() != "");
            }
            if (isValid) {
                $("#tab-about-me .icon").addClass("success");
                if (firstFormAdding > 0) {
                    firstFormAdding = -1;
                    stepsLeft = stepsLeft + firstFormAdding;
                }
            } else {
                $("#tab-about-me .icon").removeClass("success");
                if (firstFormAdding < 0) {
                    firstFormAdding = 1;
                    stepsLeft = stepsLeft + firstFormAdding;
                }
            }
        }

        if (tabid == 2 || tabid == 0) {//class overview
            if ($("#title").val() != "" && $("#summary").val() != "") {
                $("#tab-class-overview .icon").addClass("success");
                if (secondFormAdding > 0) {
                    secondFormAdding = -1;
                    stepsLeft = stepsLeft + secondFormAdding;
                }
            } else {
                $("#tab-class-overview .icon").removeClass("success");
                if (secondFormAdding < 0) {
                    secondFormAdding = 1;
                    stepsLeft = stepsLeft + secondFormAdding;
                }
            }
        }

        if (tabid == 3 || tabid == 0) {//class cover
            if ($("#classhascover").val() != "") {
                $("#tab-class-photo .icon").addClass("success");
                if (thirdFormAdding > 0) {
                    thirdFormAdding = -1;
                    stepsLeft = stepsLeft + thirdFormAdding;
                }
            } else {
                $("#tab-class-photo .icon").removeClass("success");
                if (thirdFormAdding < 0) {
                    thirdFormAdding = 1;
                    stepsLeft = stepsLeft + thirdFormAdding;
                }
            }
        }

        //Set Tab text and button
        if (stepsLeft != 0) {
            $("#number-steps-left").text(stepsLeft);
            $(".number-steps-tips").addClass("hide");
            $("#number-steps-tip" + stepsLeft).removeClass("hide");
            if ($(".steps-complete:visible").length > 0) {
                $(".steps-left").show();
                $(".steps-complete").hide();
            }
        }
        else if ($(".steps-left:visible").length > 0) {
            $(".steps-left").hide();
            $(".steps-complete").show();
            $("#class-publish.btn-default").addClass("btn-primary").removeClass("btn-default");
        }
    }

    this.stepValidation = function () {
        $("#form-about-me #selfintro").bind("change", function () {
            classedit.formValidation(1);
        });

        $("#form-class-overview  #title,#form-class-overview #summary").bind("keyup", function () {
            classedit.formValidation(2);
        });

        //Textarea charactors limit

        if ($('#title').length > 0) {
            if ($('#selfintro').length > 0) {
                $('#selfintro').limit('600', '#selfintro-charsleft');
            }
            $('#title').limit('30', '#title-charsleft');
            $('#summary').limit('400', '#summary-charsleft');
            $('#detail').limit('2000', '#class-detail-charsleft');
        }
    }
    
    this.initCategory = function () {
        var classCateId = classedit.ctlCateId.val();
        if (classCateId == "" || classCateId == classedit.ctlCatePId.val()) {
        $(classedit.subCateIdPrefix + "-frame").hide();
        }
        //For category dropdowns
        $(this.cateIdPrefix).change(function () {
            var selObj = $(this).children('option:selected');
            classedit.ctlCatePId.val(selObj.val());
            var subData = selObj.attr("data-id");
            if (subData == undefined || subData == "") {
                classedit.ctlCateId.val(selObj.val());
                $(classedit.subCateIdPrefix + "-frame").hide();
            }
            else {
                $(classedit.subCateIdPrefix).next("div").find("a.chosen-single span").text($(classedit.subCateIdPrefix).children('option:eq(0)').text());
                classedit.ctlCateId.val("");
                $(classedit.subCateIdPrefix + "-frame").show();
            }
            classedit.initCategoryHits(selObj.attr("data-name"));
            classedit.formValidation(1);
        });

        $(this.subCateIdPrefix).change(function () {
            classedit.ctlCateId.val($(this).children('option:selected').val());
            classedit.formValidation(1);
        });
    }

    this.initCategoryHits = function (categoryName) {
        if (categoryName == "") {
            categoryName = $("#classf-hidcatename").val();
        }
        $("#summary-hits>p>span").hide();
        $("#summary-hits>p>span[data-name=" + categoryName + "]").show();
        $("#title-hits>p>span").hide();
        $("#title-hits>p>span[data-name=" + categoryName + "]").show();
    }

    //For city drop down suggestion option selected 
    this.selectCity = function (targetObj) {
        classedit.ctlCityId.val($(targetObj).attr("id").replace(classedit.cityIdPrefix, ""));
        //$("#" + classedit.cityIdPrefix + classedit.textSuffix + " label").text($(targetObj).find("label").text());
        $("#classf_cityanme").val($(targetObj).find("label").text());
        $("#" + classedit.cityIdPrefix + "list").slideUp();
    }

    this.initCity = function () {
        var key = $.trim($("#classf_cityanme").val());
        if (key.length > 2) {
            var paraData = { searchKey: key };
            //classedit.ctlCityId.val($(targetObj).attr("id").replace(classedit.cityIdPrefix));
            //$("#" + classedit.cityIdPrefix + classedit.textSuffix + " label").text($(targetObj).find("label").text());
            //$("#" + classedit.cityIdPrefix + "list").slideUp();

            $.ajax({
                url: classedit.SearchCityPath,
                type: "POST",
                dataType: "Json",
                data: paraData,
                cache: false,
                success: function (data) {
                    if (data) {
                        classedit.ctlCity.empty();
                        for (var i = 0; i < data.length; i++) {
                            var tempOption = $("<li></li>").attr("id", classedit.cityIdPrefix + data[i].Id);
                            $("<label></label>").text($.trim(data[i].Text)).appendTo(tempOption);
                            tempOption.appendTo(classedit.ctlCity);
                            var ctrlId = "#" + classedit.cityIdPrefix + "list";
                            $(ctrlId + ":hidden").slideDown();
                            $(ctrlId + " li").unbind().click(function () { classedit.selectCity(this); });
                        }
                    }
                }
            });
        }
    }

    this.selectSubCategory = function (targetObj) {
        classedit.ctlCateId.val($(targetObj).attr("id").replace(classedit.subCateIdPrefix));
        $("#" + classedit.subCateIdPrefix + classedit.textSuffix + " label").text($(targetObj).find("label").text());
        $("#" + classedit.subCateIdPrefix + "list").slideUp();
    }


    /* text tag editor */
    //this.initTags = function () {
    //    $('#classf-tags').tagsInput({ width: 'auto' });
    //    $('div.tagssuggestion span.tag').click(function () { $('#classf-tags').addSuggestionTags($(this)); });
    //}


    /* Character Counter */
    this.initCharacterCount = function () {
        classedit.characterCount($("#classf-intro"), $("#classf-introcounter"), 300);
    }

    this.characterCount = function (targetObj, textObj, maxNum) {
        $(targetObj).keyup(function () {
            var currlen = $(targetObj).val().length;
            var leftNo = maxNum - currlen;
            $(textObj).text(leftNo + "/" + maxNum);
        });
    }


    this.saveClassInfo = function (saveType, saveValue) {
        var savePath;
        var completeStatus = 1;
        var page1Status = $("#tab-about-me .icon").hasClass("success") ? 2 : 0;
        var page2Status = $("#tab-class-overview .icon").hasClass("success") ? 4 : 0;
        var page3Status = $("#tab-class-photo .icon").hasClass("success") ? 8 : 0;
        completeStatus += page1Status + page2Status + page3Status;

        switch (saveType) {
            //case 11://tag
            //case 2://proved
            case 3://category
            case 4://teach
            case 5://skill
            case 8://level
                savePath = "/ClassHelper/UpdateClassSByteInfo";
                break;
            case 14://set to publish
                savePath = "/ClassHelper/UpdateClassBoolInfo";
                break;
            default://6,7,9,10,12
                savePath = "/ClassHelper/UpdateClassTextInfo";
                break;
        }

        var paraData = { updateType: saveType, classId: classedit.classId, infoValue: saveValue };
        consoleLog(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (saveType == 14) {
                    window.location = "/class/publish";
                }
            }/*, error: function (e) {
                console.log(e);
            }*/
        });
    }

    this.updateMemberInfo = function (type, value) {
        var paraData = { saveType: type, saveValue: value };
        consoleLog(paraData);
        var savePath = configSitePath + "/MemberHelper/UpdateMemberInfo";
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                //update saved infor message, or show error message if can't save
            }
        });
    }


}




