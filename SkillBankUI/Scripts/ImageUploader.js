var uploadSetting = {
    imagetype: 1,
    resize: { width: 640, quality: 95, preserve_headers: false },
};

var uploader = new plupload.Uploader({
    runtimes: 'html5,flash,silverlight,html4',
    browse_button: 'pickfiles', // pass triggr id...
    container: document.getElementById('uploadimg-container'), // image holder container
    url: 'http://v0.api.upyun.com/skillbank',
    flash_swf_url: '../js/Moxie.swf',
    silverlight_xap_url: '../js/Moxie.xap',
    resize: uploadSetting.resize,
    filters: {
        max_file_size: '10mb',
        mime_types: [
            { title: "Image files", extensions: "jpg,gif,png,jpeg,bmp" }
        ]
    },

    preinit: {
        Init: function (up, info) {
        },

        UploadFile: function (up, file) {
            var multipartParams = {
                'Filename': document.getElementById('savekey').value,
                'Content-Type': '',
                'policy': document.getElementById('policy').value,
                'signature': document.getElementById('signature').value
            };
            uploader.setOption("multipart_params", multipartParams);
        }
    },

    init: {
        PostInit:
            function () {
                var tigger = document.getElementsByClassName("fileupload-trigger");
                if (tigger.length) {
                    tigger[0].onclick = function () {
                        if (document.getElementById('savekey').value) {
                            uploader.start();
                        }
                        return false;
                    };
                    if (tigger[1]) {
                        tigger[1].onclick = function () {
                            if (document.getElementById('savekey').value) {
                                uploader.start();
                            }
                            return false;
                        };
                    }
                }
            },
        FilesAdded: function (up, files) {
            //upload just last one file
            if (up.files.length > 1) {
                up.splice(0, up.files.length - 1);
            }

            getUploadSetting(files[0]);
        },
        BeforeUpload: function (up, file) {
            showLoadingIcon(true);
        },
        /*
        UploadProgress: function (up, file) {
            document.getElementById("progress").innerHTML = file.percent + "%";
        },
        */
        UploadComplete: function (up, files) {
            // Called when all files are either uploaded or failed
            showLoadingIcon(false);
            myAlert("保存成功", 2);
            console.log('[UploadComplete]');
        },
        Error: function (up, err) {
            console.log(err.code + ": " + err.message);
        }
    }
});

function initUploadSetting() {
    if (document.getElementsByClassName('edit-avatar')[0]) {
        uploadSetting = {
            imagetype: 2,
            resize: { width: 360, quality: 95, preserve_headers: false }
        };
    }
    //uploader.setOption("resize", uploadSetting.resize);
    uploader.init();
    
}
function getUploadSetting(file) {
    if (!file) return;
    if (!/image\//.test(file.type)) {
        alert("请选择图片格式文件");
        return;
    }
    $('#imagefileext')[0].value = ("." + file.name.match(/[^\.]+$/)).toLowerCase();

    previewImage(file, function (imgsrc) {
        var preImg = document.getElementById('uploadimg');
        preImg.src = imgsrc;
        if (document.getElementById('preview-cover')) {
            document.getElementById('preview-cover').src = imgsrc;
        }
        //initUploadSetting(preImg.naturalWidth, preImg.naturalHeight);
        getUpCloudOptions(uploadSetting.imagetype);
    });

}


function previewImage(file, callback) {//file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数 
    if (file.type == 'image/gif') {//gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
        var fr = new mOxie.FileReader();
        fr.onload = function () {
            callback(fr.result);
            fr.destroy();
            fr = null;
        }
        fr.readAsDataURL(file.getSource());
    } else {
        var preloader = new mOxie.Image();
        preloader.onload = function () {
            //if (uploadSetting.imagetype===1) {
            //    preloader.downsize(640, 480);//先压缩一下要预览的图片,宽300，高300
            //}
            var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
            callback && callback(imgsrc); //callback传入的参数为预览图片的url
            preloader.destroy();
            preloader = null;
        };
        preloader.load(file.getSource());
    }
}

