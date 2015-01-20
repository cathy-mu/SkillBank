var classpublish = classpublish || new classpublish_Class;
$(document).ready(function () {
    classpublish.init();
});

function classpublish_Class() {
    this.init = function () {
        this.initEvents();
    };

    
    this.initEvents = function () {
      
        //$("#selfintro").blur(function () {
        //    if ($.trim($(this).val()) != "") {
        //        classpublish.updateMemberInfo(5, $(this).val());
        //    }
        //});
        $("#class-membergender .iCheck-helper").click(function () {
            classpublish.updateMemberInfo(6, $(this).parent().parent().hasClass("member-male") ? 1 : 0);
        });

        $("#profileinfo-save").click(function () {
            if ($.trim($("#selfintro").val()) != "") {
                classpublish.updateMemberInfo(5, $("#selfintro").val());
            }
            //classpublish.updateMemberInfo(6, $(this).parent().parent().hasClass("member-male") ? 1 : 0);
            $("#profileinfo-save").addClass("saved").removeClass("btn-primary").find("span").text("已保存");
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




