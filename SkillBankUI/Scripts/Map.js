var mapObj;
var keywordCtlId = "location";
var addressCtlId = "addressoption";
var marker = new Array();
var windowsArr = new Array();
//基本地图加载
function mapInit() {
    if ($("#iCenter").length > 0) {
        $("#" + keywordCtlId).focus(function () { showTip("enter"); })
        var cityName = $("#classf-hidcityname").val();
        mapObj = new AMap.Map("iCenter", {
            city: cityName,
            level: 13  //地图显示的比例尺级别  
        }
        );
        mapObj.plugin(["AMap.ToolBar"], function () {
            toolBar = new AMap.ToolBar();
            //Hide direction cicle
            toolBar.hideDirection();
            mapObj.addControl(toolBar);
        });
        
        //init keyword search for options
        if (navigator.userAgent.indexOf("MSIE") > 0) {
            document.getElementById(keywordCtlId).onpropertychange = autoSearch;
            }
        else {
            document.getElementById(keywordCtlId).oninput = autoSearch;
        }
    }
}

function showTip(tipclass) {
    if ($("#map-hits").length > 0) {
        $("#map-hits p:not('." + tipclass + "')").hide();
        $("#map-hits p."+tipclass).fadeIn();
    }
}

//输入提示
function autoSearch() {
    clearMapMarks();
    var cityName = $("#classf-hidcityname").val();
    var keywords = $("#" + keywordCtlId).val();
    var auto;
    
    //加载输入提示插件
    mapObj.plugin(["AMap.Autocomplete"], function () {
        var autoOptions = {
            city: cityName//"上海"
        };
        auto = new AMap.Autocomplete(autoOptions);
        //查询成功时返回查询结果
        if (keywords.length > 0) {
            AMap.event.addListener(auto, "complete", autocomplete_CallBack);
            auto.search(keywords);
        }
        else {
            document.getElementById(addressCtlId).style.display = "none";
        }
    });

    
}

function autocomplete_CallBack(data) {
    var resultStr = "";
    var tipArr = [];
    tipArr = data.tips;
    if (tipArr != null && tipArr != "undefined" && tipArr.length > 0) {
        for (var i = 0; i < tipArr.length; i++) {
            resultStr += "<div id='divid" + (i + 1) + "' onmouseover='openMarkerTipById1(" + (i + 1)
                        + ",this)' onclick='selectResult(" + i + ")' onmouseout='onmouseout_MarkerStyle(" + (i + 1)
                        + ",this)' style=\"font-size: 13px;cursor:pointer;padding:5px 5px 5px 5px;\">" + tipArr[i].name + "<span style='color:#C1C1C1;'>" + tipArr[i].district + "</span></div>";
        }
    }
    else {
        resultStr = " π__π 亲,人家真的找不到结果啊!<br />要不试试：<br />1.请确保所有字词拼写正确。<br />2.尝试不同的关键字。<br />3.尝试更宽泛的关键字。";
    }
    document.getElementById(addressCtlId).innerHTML = resultStr;
    document.getElementById(addressCtlId).style.display = "block";
    showTip("select");
}

//鼠标移入时样式
function openMarkerTipById1(pointid, thiss) {
    thiss.style.background = '#CAE1FF';
}
//鼠标移开后样式恢复
function onmouseout_MarkerStyle(pointid, thiss) {
    thiss.style.background = "";
}

//选择输入提示关键字
function selectResult(index) {
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        document.getElementById(keywordCtlId).onpropertychange = null;
        document.getElementById(keywordCtlId).onfocus = focus_callback;
    }
    //截取输入提示的关键字部分
    var text = document.getElementById("divid" + (index + 1)).innerHTML.replace(/<[^>].*?>.*<\/[^>].*?>/g, "");
    document.getElementById(keywordCtlId).value = text;
    document.getElementById(addressCtlId).style.display = "none";
    placeSearch();//call search on map by keyword
}

function focus_callback() {
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        document.getElementById(keywordCtlId).onpropertychange = autoSearch;
    }
}

function placeSearch() {
    var cityName = $("#classf-hidcityname").val();
    var MSearch;
    //console.log("--3--" + cityName);
    mapObj.plugin(["AMap.PlaceSearch"], function () {
        MSearch = new AMap.PlaceSearch({ //构造地点查询类
            pageSize: 10,
            pageIndex: 1,
            city: cityName
        });
        AMap.event.addListener(MSearch, "complete", keywordSearch_CallBack);//返回地点查询结果
        MSearch.search($("#" + keywordCtlId).val()); //关键字查询
    });
}
//添加marker&infowindow    
function addmarker(i, d, sum) {
    var lngX = d.location.getLng();
    var latY = d.location.getLat();
    var markerOption = {
        map: mapObj,
        icon: "http://webapi.amap.com/images/" + (i + 1) + ".png",
        position: new AMap.LngLat(lngX, latY),
        extData: { address: d.address, id: d.id }
    };
    var mar = new AMap.Marker(markerOption);
    marker.push(new AMap.LngLat(lngX, latY));

    var infoWindow = new AMap.InfoWindow({
        content: "<h3><font color=\"#00a6ac\">&nbsp;&nbsp;" + (i + 1) + ". " + d.name + "</font></h3>" + TipContents(d.address),
        size: new AMap.Size(300, 0),
        autoMove: true,
        offset: new AMap.Pixel(0, -30)
    });
    windowsArr.push(infoWindow);
    if (sum == 1) {
        setAddressInfo(d.address, d.location.lng, d.location.lat, 1);
     }
    else {
        var aa = function (e) {
            var posInfo = mar.getPosition();
            infoWindow.open(mapObj, mar.getPosition());
            var addressInfo = mar.getExtData();
            setAddressInfo(addressInfo.address, posInfo.lng, posInfo.lat, 1);
        };
        AMap.event.addListener(mar, "click", aa);
        showTip("click");
    }
}

function clearMapMarks() {
    mapObj.clearMap();
    setAddressInfo("", "", "",0)
}

function setAddressInfo(address, posx, posy, setaddress) {
    $("#map-address").val(address);
    var keywords = $("#" + keywordCtlId).val();
    //$("#" + keywordCtlId).val(address);
    $("#mapposx").val(posx);
    $("#mapposy").val(posy);
    if (setaddress == 1 && $.trim(posx) != "" && $.trim(posy) != "") {
        updateMemberPosition(posx + "," + posy, keywords);
        showTip("saved");
    }
}

//回调函数
function keywordSearch_CallBack(data) {
    //clearMapMarks();
    var resultStr = "";
    var poiArr = data.poiList.pois;
    var resultCount = poiArr.length;
    for (var i = 0; i < resultCount; i++) {
        //resultStr += "<div id='divid" + (i + 1) + "' onmouseover='openMarkerTipById1(" + i + ",this)' onmouseout='onmouseout_MarkerStyle(" + (i + 1) + ",this)' style=\"font-size: 12px;cursor:pointer;padding:0px 0 4px 2px; border-bottom:1px solid #C1FFC1;\"><table><tr><td><img src=\"http://webapi.amap.com/images/" + (i + 1) + ".png\"></td>" + "<td><h3><font color=\"#00a6ac\">名称: " + poiArr[i].name + "</font></h3>";
        //resultStr += TipContents(poiArr[i].address) + "</td></tr></table></div>";
        addmarker(i, poiArr[i], resultCount);
    }
    mapObj.setFitView();
    //document.getElementById("result").innerHTML = resultStr;
}

//Address tip box
function TipContents(address) {  //窗体内容
    if (address == "" || address == "undefined" || address == null || typeof address == "undefined") {
        address = "暂无";
    }
    var str = "&nbsp;&nbsp;地址：<span class='address'>" + address + "</span>";
    return str;
}

function openMarkerTipById1(pointid, thiss) {  //根据id 打开搜索结果点tip
    thiss.style.background = '#CAE1FF';
    //windowsArr[pointid].open(mapObj, marker[pointid]);
}
function onmouseout_MarkerStyle(pointid, thiss) { //鼠标移开后点样式恢复
    thiss.style.background = "";
}

function updateMemberPosition(posValue, addressValue) {
    var paraData = { saveType: 8, saveValue: posValue, saveValue2: addressValue };
    consoleLog(paraData);
    var savePath = configSitePath + "/MemberHelper/UpdateMemberInfo";
    $.ajax({
        url: savePath,
        type: "POST",
        dataType: "Json",
        data: paraData,
        cache: false,
        success: function (data) {
            consoleLog(data);
            if ($("#form-about-me").length > 0) {
                classedit.changePosition();
            }
        }
    });
}

$(document).ready(function () {
    mapInit();
    //$("#location").keyup(function () { placeSearch(); });
    //$("#update_memberinfo").click(function () { updateMemberInfo(); });
});
