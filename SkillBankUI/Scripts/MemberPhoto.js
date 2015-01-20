var memberphoto = memberphoto || new memberphoto_Class;
$(document).ready(function () { memberphoto.init(); photoEditor.init($("#profile-uploadavatar-file"), 396, 180, 180, 40, 40, 0); });

function memberphoto_Class() {

    this.init = function () {
        sitecommon.showLoginPop(false);

        $("#profile-uploadavatar").click(function () { $("#profile-uploadavatar-file").trigger("click"); });
        $("#btnimg-upload").click(function () { memberphoto.updateMemberAvatar(); });
    }


    // Update member info
    this.updateMemberAvatar = function () {
        var option = {
            type: "post",
            dataType: "json",
            url: "/API/UploadAvatar",
            success: function (data) {
                $("#btnimg-upload").addClass("saved").removeClass("btn-primary").find("span").text("已保存");
            }
        }
        $('form').ajaxSubmit(option);
    }
}