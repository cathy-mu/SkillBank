var uploadSetting;

var uploader = new plupload.Uploader({
    runtimes: 'html5,flash,silverlight,html4',
    browse_button: 'pickfiles', // you can pass in id...
    container: document.getElementById('uploadimg-container'), // ... or DOM Element itself
    url: 'http://v0.api.upyun.com/skillbank',
    flash_swf_url: '../js/Moxie.swf',
    silverlight_xap_url: '../js/Moxie.xap',
    //resize: { width: 100, quality: 90 },
    //multipart_params: uploadSetting.multipart_params,
    filters: {
        max_file_size: '10mb',
        mime_types: [
            { title: "Image files", extensions: "jpg,gif,png,jpeg,bmp"}
        ]
    },

    preinit: {
        Init: function (up, info) {
        },

        UploadFile: function (up, file) {
            console.log('[UploadFile]', file);
            uploadSetting.multipart_params = {
                'Filename': document.getElementById('savekey').value,
                'Content-Type': '',
                'policy': document.getElementById('policy').value,
                'signature': document.getElementById('signature').value
            };
            uploader.setOption("multipart_params", uploadSetting.multipart_params);
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
            console.log("add image");
            //plupload.each(files, function (file) {
            //    console.log('  File:', file);
            //});
            getUploadSetting(files[0]);
        },
        /*
        UploadProgress: function (up, file) {
            console.log(file.percent);
            document.getElementById("progress").innerHTML = file.percent + "%";
        },
        */
        UploadComplete: function (up, files) {
            // Called when all files are either uploaded or failed
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
            isAvatar: true,
            resize: { width: 180, quality: 90, preserve_headers: false }
        };
    } else {
        uploadSetting = {
            isAvatar: false,
            resize: { width: 640, quality: 95, preserve_headers: false }
        };
    }
    
    uploader.setOption("resize", uploadSetting.resize);
}

function getUploadSetting(file) {
    console.log("设置图片src");
    if (!file) return;
    if (!/image\//.test(file.type)) {
        alert("请选择图片格式文件");
        return;
    }
    $('#imagefileext')[0].value = ("." + file.name.match(/[^\.]+$/)).toLowerCase();

    previewImage(file, function (imgsrc) {
        document.getElementById('uploadimg').src = imgsrc;
        if (!uploadSetting.isAvatar)
        {
            document.getElementById('preview-cover').src = imgsrc;
        }
        //console.log(previewImg.naturalWidth);
        //console.log(previewImg.naturalHeight);
    });
    
    getUpCloudOptions(uploadSetting.isAvatar);
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
            //preloader.downsize(300, 300);//先压缩一下要预览的图片,宽300，高300
            var imgsrc = preloader.type == 'image/jpeg' ? preloader.getAsDataURL('image/jpeg', 80) : preloader.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
            callback && callback(imgsrc); //callback传入的参数为预览图片的url
            preloader.destroy();
            preloader = null;
        };
        preloader.load(file.getSource());
    }
}


uploader.init();
initUploadSetting();
//getUploadSetting();
