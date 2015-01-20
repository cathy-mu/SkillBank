var photoEditor = photoEditor || new photoEditor_Class;

function photoEditor_Class() {
    this.uploadFileObj = $("#profile-uploadavatar-file");
    this.imgSizeMW;
    this.imgSizeMH;
    this.imgSizeSW;
    this.imgSizeSH;
    this.minCropSize = "[40, 40]";// // min crop size
    this.imgFileLimit = 2000 * 1024;

    this.init = function (fileObj, boxW, sizeMW, sizeMH, sizeSW, sizeSH, imageLimit) {
        //this.initCrop();
        this.uploadFileObj = fileObj;
        this.boxWidth = boxW;
        this.imgSizeMW = sizeMW;
        this.imgSizeMH = sizeMH;
        this.imgSizeSW = sizeSH;
        this.imgSizeSH = sizeSH;
        this.hasMiniSize = (sizeSW > 0);
        this.imgFileLimit = imageLimit > 0 ? imageLimit : (2000 * 1024);//default max size is 2000*1024
        this.initChangeImage();
        //this.uploadImage();
    };

    this.initChangeImage = function () {
        photoEditor.uploadFileObj.change(function () {fileSelectHandler(this) });
        $("#upload-image").click(function () { fileUploadHandler(this) });
    }

    this.updateInfo = function (e) {
        $('.fileinfo #imagefiletop').val(Math.round(e.x));
        $('.fileinfo #imagefileleft').val(Math.round(e.y));
        $('.fileinfo #imagefilew').val(Math.round(e.w));
        $('.fileinfo #imagefileh').val(Math.round(e.h));

        $(".fileinfo #imagefilesetting").val(Math.round(e.x) + "," + Math.round(e.y) + "," + Math.round(e.w) + "," + Math.round(e.h));


        if (parseInt(e.w) > 0) {
            var rx = photoEditor.imgSizeMW / e.w;
            var ry = photoEditor.imgSizeMH / e.h;
            //通过比例值控制图片的样式与显示
            $("#preview_m").css({
                width: Math.round(rx * $('#preview').width()) + "px",	//预览图片宽度为计算比例值与原图片宽度的乘积
                height: Math.round(ry * $('#preview').height()) + "px",  //预览图片高度为计算比例值与原图片高度的乘积
                marginLeft: "-" + Math.round(rx * e.x) + "px",
                marginTop: "-" + Math.round(rx * e.y) + "px"
            });

            if (photoEditor.hasMiniSize) {
                var rx2 = photoEditor.imgSizeSW / e.w;
                var ry2 = photoEditor.imgSizeSH / e.h;
                $("#preview_s").css({
                    width: Math.round(rx2 * $('#preview').width()) + "px",
                    height: Math.round(ry2 * $('#preview').height()) + "px",
                    marginLeft: "-" + Math.round(rx2 * e.x) + "px",
                    marginTop: "-" + Math.round(ry2 * e.y) + "px"
                });
            }

        }
    };

    function fileSelectHandler(obj) {
        var isIE8 = false;
        // get selected file
        var oFile = photoEditor.uploadFileObj[0];
        if (oFile.files) {
            oFile = oFile.files[0];
        }
        else {
            oFile = oFile.value;
            alert(oFile);
            isIE8 = true;
        }
        // hide all errors
        //$('#imgupload-error').hide();

        if (!isIE8) {
            // check for image type (jpg and png are allowed)
            var rFilter = /^(image\/jpeg|image\/gif|image\/png)$/i;
            if (!rFilter.test(oFile.type)) {
                $('#imgupload-error').text("请选择图像文件");
                return;
            } else {
                var filename = photoEditor.uploadFileObj.val().replace(/.*(\/|\\)/, "");
                var fileExt = (/[.]/.exec(filename)) ? /[^.]+$/.exec(filename) : "";//.toLowerCase()
                $("#imagefileext").val("." + fileExt);
            }
            // check for file size
            if (oFile.size > photoEditor.imgFileLimit) {
                $('#imgupload-error').text("文件太大，请选择一张稍小点的图片(<2M)");
                return;
            }
            $('#imgupload-error').text("");
            // preview element
            //var oImage = document.getElementById('preview');
            var oImage = new Image();
            $(oImage).attr("id", "preview");
            var oImage_pm = new Image();
            $(oImage_pm).attr("id", "preview_m");
            if (photoEditor.hasMiniSize) {
                var oImage_ps = new Image();
                $(oImage_ps).attr("id", "preview_s");
            }

            // prepare HTML5 FileReader
            var oReader = new FileReader();
            oReader.onload = function (e) {

                // e.target.result contains the DataURL which we can use as a source of the image

                oImage.src = e.target.result;
                oImage_pm.src = oImage.src;
                if (photoEditor.hasMiniSize) {
                    oImage_ps.src = oImage.src;
                }

                oImage.onload = function () { // onload event handler

                    // display step 2
                    $('.step2').fadeIn(500);

                    // display some basic image info
                    var sResultFileSize = bytesToSize(oFile.size);
                    $('#filesize').val(sResultFileSize);

                    $('#filetype').val();//oFile.type
                    $('#filedim').val(oImage.naturalWidth + ' x ' + oImage.naturalHeight);

                    // Create variables (in this scope) to hold the Jcrop API and image size
                    var jcrop_api, boundx, boundy;

                    // destroy Jcrop if it is existed
                    if (typeof jcrop_api != 'undefined')
                        jcrop_api.destroy();

                    //initialize Jcrop
                    $('#preview').Jcrop({
                        minSize: photoEditor.minCropSize, // min crop size[32, 32]
                        boxWidth: photoEditor.boxWidth,
                        aspectRatio: photoEditor.imgSizeMW / photoEditor.imgSizeMH, // keep aspect ratio 1:1
                        bgFade: true, // use fade effect
                        bgOpacity: .3, // fade opacity
                        setSelect: [0, 0, photoEditor.imgSizeMW, photoEditor.imgSizeMH],
                        onChange: photoEditor.updateInfo,
                        onSelect: photoEditor.updateInfo,
                        onRelease: clearInfo
                    }, function () {

                        // use the Jcrop API to get the real image size
                        var bounds = this.getBounds();
                        boundx = bounds[0];
                        boundy = bounds[1];

                        // Store the Jcrop API in the jcrop_api variable
                        jcrop_api = this;
                    });
                    if ($('#preview_s_holder').length > 0) {
                        $("#preview_s").css({
                            width: Math.round(photoEditor.imgSizeSW * $('#preview').width() / photoEditor.imgSizeMW) + "px",
                            height: Math.round(photoEditor.imgSizeSH * $('#preview').height() / photoEditor.imgSizeMH) + "px"
                        });
                    }
                };
            };

            // Download by http://www.codefans.net	
            // read selected file as DataURL
            oReader.readAsDataURL(oFile);
            $('#preview-holder').empty().append(oImage).show();
            $('#preview-placeholder').hide();
            $("#btnimg-upload").addClass("btn-primary").removeClass("saved").find("span").text("保存");
            $('#preview_m_holder').empty().append(oImage_pm);
            $('#preview_s_holder').empty().append(oImage_ps);
        }
    }

    function bytesToSize(bytes) {
        var sizes = ['Bytes', 'KB', 'MB'];
        if (bytes == 0) return 'n/a';
        var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
        return (bytes / Math.pow(1024, i)).toFixed(1) + ' ' + sizes[i];
    };

    // clear info by cropping (onRelease event handler)
    function clearInfo() {
        if (photoEditor.hasMiniSize) {
            $("#preview_s").css({ marginLeft: 0, marginTop: 0 });
        }
        $("#preview_m").css({ marginLeft: 0, marginTop: 0 });
       
        $(".fileinfo #imagefilesetting").val("0,0," + photoEditor.imgSizeMW + "," + photoEditor.imgSizeMH);
    };
}




