﻿// setting
window.ENV = {
    host: "http://" + window.location.host,
    imgHost: "http://skillbank.b0.upaiyun.com"
};

// fns
window.checkImgHost = function(host, url){
  return url.indexOf('http') === -1 ? host + url : url
}
var $ = document.querySelectorAll.bind(document);
Element.prototype.on = Element.prototype.addEventListener;
NodeList.prototype.on = function (event, fn) {
  []['forEach'].call(this, function (el) {
    el.on(event, fn);
  });
  return this;
};

// ajax
function request(type, url, opts, callback) {
  var xhr = new XMLHttpRequest();
  if (typeof opts === 'function') {
    callback = opts;
    opts = null;
  }
  xhr.open(type, url);
  xhr.onreadystatechange = function(){
      if (xhr.readyState == 4) {
      if( (xhr.status >= 200 && xhr.status <300) || xhr.status == 304 ){
        // console.log(xhr.responseText)
      } else {
        console.log(xhr.status);
      }

      resError(xhr.status);
    }
  }
  var fd = [];
  if (type === 'POST' && opts) {
    for (var key in opts) {
      fd.push( key + '=' + opts[key] );
    }
    fd = fd.join('&');
    xhr.setRequestHeader("Content-type","application/x-www-form-urlencoded");
  }
  xhr.onload = function () {
    callback(JSON.parse(xhr.response));
  };
  xhr.send(opts ? fd : null);
}
function resError(status){
  if(status == 401){
      //goToLogin();
      if (window.confirm("你还没有登陆或注册，现在登录或注册吗？")) {
          location.href = '/m/login';
      }
  } else if(status == 400){
    alert('400');
  }
}

function checkLogin() {
    var ctlObj;
    if ($('.bar-nav').length>0) {
        ctlObj = $('.bar-nav')[0];
    } else {
        //ctlObj = ctl;
    }
    var patten = new RegExp(/true/i);
    if (!patten.test(ctlObj.dataset.ismember)) {
        //goToLogin();
        if (window.confirm("你还没有登陆或注册，现在登录或注册吗？")) {
            location.href = '/m/login';
        }
        return false;
    }
    return true;
}

function goToLogin() {
    if (window.confirm("你还没有登陆或注册，现在登录或注册吗？")) {
        location.href = '/m/login';
    }
    return;
}

var get = request.bind(this, 'GET');
var post = request.bind(this, 'POST');


var checkPage = function () {
  addBulletsWeget();
  removeTrashes();
  hackForModals();
  initLogin();
  initBack();

  $(".login-trigger").on('click', function () {
      checkLogin();
  });

  // course list search
  if (document.getElementById('course-search')) {
      switchCourseCat();
      affix();
      toggleLike();
  }

  // course detail page
  if ($('.course-page').length) {
      toggleLike();
  
      // go for comment
      $('.goto').on('click', function (event) {
          event.preventDefault();
          location.href = '#' + this.dataset.for;
      });

      $(".misscoin-trigger").on('click', function () {
          alert("可用课币不足");
      });

      privateMsgForm();
      reservationForm();//book class
      commentForm();
      followMember();
  }

  // personal page
  if ($('.personal-page').length) {
      followMember();
  }
   
  if ($('.login-page').length) {
      $('#sinaloginbtn').on('click', function () {
          var backUrlKey = "backUrl";
          var refPath = sessionStorage.getItem(backUrlKey);
          if (refPath == undefined || refPath == "") {
              refPath = "/m";
          }
          location.href = "https://api.weibo.com/oauth2/authorize?client_id=111240964&response_type=code&redirect_uri=http%3A%2F%2Fwww.skillbank.cn%2Fsignup%3Fmb%3D" + encodeURIComponent(refPath);
      });
  }

  // chat page
  if ($('.chat-page').length) {

      // init textrea auto size
      jQuery('#write-box').textareaAutoSize();

      // bind add msg
      chatForm();
  }

  // register page
  //--Update:redirect url ,validation
  if ($('#register-page').length) {
      var $form = $('form')[0];
      $("#registebtn").on('click', function (event) {
          event.preventDefault();

          var patten = new RegExp(/^[1]+[3,4,5,8]+\d{9}/);
          if (!$form.phone.value || !patten.test($form.phone.value)) {
              $form.phone.classList.add("error");//手机号码格式不对
              return;
          } else {
              $form.phone.classList.remove("error");//手机号码格式不对
          }

          patten = new RegExp(/^\d{6}/);
          if (!$form.code.value || !patten.test($form.code.value)) {
              $form.code.classList.add("error");//验证码格式不对
              return;
          } else {
              $form.code.classList.remove("error");//验证码格式不对
          }

          if (!$form.registebtn.dataset.avatar || !$form.registebtn.dataset.name) {
              alert("未获得社交账号信息，请重新登陆");
              location.href = "/m/login";
              return;
          }

          var data = {
              Mobile: $form.phone.value,
              Code: $form.code.value,
              Avatar: $form.registebtn.dataset.avatar,
              Name: $form.registebtn.dataset.name
          };
          
          post(ENV.host + '/api/member', data, function (fb) {
              console.log(fb);//-1 数据格式不符
              if (fb == 2) {
                  alert("验证失败，请检查验证码或重新发送");
              } else if (fb == 3) {
                  alert("手机号码被占用");
              } else {
                  redirectAfterLogin();
              }
          });

      });

      var $verifyBtn = $('.btn-grey');
      var seconds;
      $verifyBtn.on('click', function () {
          if (seconds > 0) return;
          var patten = new RegExp(/^[1]+[3,4,5,8]+\d{9}/);
          if (!$form.phone.value || !patten.test($form.phone.value)) {
              $form.phone.classList.add('error');//手机号码格式不对
              return;
          } else {
              $form.phone.classList.remove('error');
          }
          var url = ENV.host + '/api/Validation?mobile=' + $form.phone.value;
          seconds = 10;
          post(url, function (fb) {
              if (fb == 1) {
                  // timer
                  var t = setInterval(function () {
                      if (seconds <= 0) {
                          $verifyBtn[0].innerText = '获得验证码';
                          clearInterval(t);
                      } else {
                          $verifyBtn[0].innerText = seconds;
                      }
                      seconds--;
                  }, 1000);
              }
              else {
                  if (fb == 0) {
                      alert("手机号码已被占用");
                  } else if (fb == 2) {
                      alert("手机号码格式不对");
                  }
                  if (t != undefined) {
                      clearInterval(t);
                  }
                  seconds = 0;
              }
          });
      })
  }

};

// start here
checkPage();
window.addEventListener('push', checkPage);

function switchCourseCat() {
    // search cat
    $('#search-cat a').on('click', function () {
        var self = this;
        var geo_opts = {
            enableHighAccuracy: true,
            maximumAge: 30000,
            timeout: 27000
        };
        
        // nearby
        if (this.dataset.by == 0) {
            navigator.geolocation.getCurrentPosition(function (position) {
                var posY = position.coords.latitude;
                var posX = position.coords.longitude;
                var map = new BMap.Map("allmap");
                var point = new BMap.Point(posX, posY);
                var geoc = new BMap.Geocoder();
                var cityName = "";
                geoc.getLocation(point, function (rs) {
                    var addComp = rs.addressComponents;
                    cityName = addComp.city;
                    var url = ENV.host + '/api/ClassList?' + 'by=' + self.dataset.by + '&type=' + self.dataset.type +
                              '&x=' + posX + '&y=' + posY + '&city=' + encodeURIComponent(cityName);
                    savePosition(posX, posY);
                    getCourses(url, self);
                });
            }, function () {
                console.log("Sorry, no position available.")
            }, geo_opts);
        } else if (this.dataset.by == 3) {// search
            var url = ENV.host + '/api/ClassList?' + 'by=' + self.dataset.by + '&type=' + self.dataset.type + '&key=' + encodeURIComponent(self.dataset.key);
            getCourses(url, self);
        } else {// category ,promote
            var url = ENV.host + '/api/ClassList?' + 'by=' + self.dataset.by + '&type=' + self.dataset.type;
            getCourses(url, self);
        }

    });
}

function getCourses(url, el) {
    get(url, function (fb) {
        if (!_.isArray(fb)) return;
        // insert html
        var tpl = $('#course-tpl')[0].innerHTML;
        $('.course-list')[0].innerHTML = _.template(tpl, { courses: fb, imgHost: ENV.imgHost });
        // active tab
        [].forEach.call($('#search-cat a'), function (el) {
            el.classList.remove('active');
        });
        el.classList.add('active');
        //bind like class event
        toggleLike();
    });
}

function toggleLike() {
    $('.toggle-like').on('click', function (e) {
        var isLogin = checkLogin();
        if (isLogin) {
            // e.stopPropagation();
            var $heart = e.target;
            //while (!$heart.matches('.toggle-like')) {
            //    $heart = $heart.parentNode;
            //}

            while (!$heart.classList.contains('toggle-like')) {
                $heart = $heart.parentNode;
            }
            var linkText = $heart.querySelector(".num");
            var isLike = !$heart.classList.contains('liked');

            var data = {
                //MemberId: $card.dataset.member_id,
                ClassId: $heart.dataset.classid,
                IsLike: isLike
            }

            post(ENV.host + '/api/likeclass', data, function (fb) {
                if (!fb) return;
                $heart.classList.toggle('liked');
                linkText.textContent = parseInt(linkText.textContent) + (isLike ? 1 : -1);
            });
        } else {
            return;
        }

    });
}

//    if (!$('.toggle-like').length) return;
//    document.body.addEventListener('click', function (e) {
//        if (e.target.matches('.toggle-like, .toggle-like *')) {
//            checkLogin();
//            // e.stopPropagation();
//            var $heart = e.target;
//            while (!$heart.matches('.toggle-like')) {
//                $heart = $heart.parentNode;
//            }

//            // get card
//            var $card = e.target;
//            while (!$card.matches('[data-classid]')) {
//                $card = $card.parentNode;
//            }
//            var linkText = $heart.querySelector(".num");
//            console.log($heart);
//            console.log(linkText);

//            var isLike = !$heart.matches('.liked');
//            var data = {
//                //MemberId: $card.dataset.member_id,
//                ClassId: $card.dataset.classid,
//                IsLike: !$heart.matches('.liked')
//            }

//            post(ENV.host + '/api/likeclass', data, function (fb) {
//                if (!fb) return;
//                $heart.classList.toggle('liked');
//                linkText.textContent = parseInt(linkText.textContent) + (isLike ? 1 : -1);
//            });

//        }
//    });
//}

function addBulletsWeget(){
  if( !document.getElementById('mySlider') ) return;
  // make bullet live
  var len = $('#mySlider .slide').length;
  var bulletsStr = "";
  var $bulletsContainer = $('#bullets ul')[0];
  var slide = function(event){
    var curr = event.detail.slideNumber;
    var $active = $("#bullets .active")[0];
    var $next = $('#bullets li')[curr];
    $active.classList.remove('active');
    $next.classList.add('active');
  }
  $('#mySlider')[0].addEventListener('slide', slide);

  // add html
  for(var i=0;i<len;i++){
    if(i === 0) {
      bulletsStr += "<li class='active'></li>\n";
    } else{
      bulletsStr += "<li></li>\n";
    }
  }
  //console.log($('#bullets ul').length);
  $bulletsContainer.innerHTML = bulletsStr;
}

function affix(){
  var $searchCat = document.getElementById('search-cat');
  var $wrap = $('#search-cat .search-cat-wrap')[0]
  var offTop = $searchCat.offsetTop
  $('.content')[0].onscroll = function (event) {
    if( this.scrollTop >= offTop ){
      $wrap.classList.add('affix');
      document.body.appendChild( $wrap )
    } else{
      $wrap.classList.remove('affix')
      $searchCat.appendChild( $wrap )
    }
  }
}

function removeTrashes(){
  var $trashes = $('body > .toBeRemoved');
  var len = $trashes.length;
  if(!len) return;
  while(len--){
    document.body.removeChild( $trashes[len] );
  }
}

function hackForModals(){
  // hack for: all modals append to body
  if( $('.content .modal').length ){

    var $modals = $('.content .modal');
    var modalsLen = $modals.length;
    while(modalsLen--){
      document.body.appendChild( $modals[modalsLen] );
    }
  }
}

//function getChatDetail(uid, fid){
//  var url = ENV.host + '/api/chat/' + uid + '?contact=' + fid;
//  var $container = $('.chat-content');
//  var tpl = $('#chat-detail-tpl')[0].innerHTML;
//  get(url, function(fb){
//    if( !_.isArray(fb) ) return;

//    // insert html
//    $container[0].innerHTML = _.template(tpl, {items: fb, uid: uid});
//  });
//}

//--Update:(DT)template , inset position
function chatForm() {
    var $form = $('#form-write');
    var $input = $form[0].querySelector('textarea');
    $form[0].on('submit', function (event) {
        event.preventDefault();
        if (!$input.value) {
            $input.classList.add("error");//手机号码格式不对
        } else {
            $input.classList.remove("error");//手机号码格式不对
            var toId = $form[0].dataset.contactid;
            if (toId != undefined) {
                var data = {
                    ToId: toId,
                    MessageText: $input.value
                };
                data.Avatar = $form[0].dataset.avatar;
                post(ENV.host + '/api/chat', data, function (fb) {
                    if (!fb) return;
                    $('.chat-content')[0].insertAdjacentHTML('beforeend',
                          _.template($('#chat-detail-tpl')[0].innerHTML, { item: data }));
                    $input.value = "";
                    $input.focus();
                });
            }
        }
    });
}

//--Update:error message
function privateMsgForm() {
    var $modal = $('#privateMsg');
    if (!$modal.length) return;
    var $form = $modal[0].querySelectorAll('form');
    var $input = $form[0].querySelector('textarea');
    $form[0].on('submit', function (event) {
        event.preventDefault();
        if (!$input.value) {
            $input.classList.add("error");
        }
        else {
            $input.classList.remove("error");
            var toId = $form[0].dataset.contactid;
            var data = {
                ToId: toId,
                MessageText: $input.value
            };
            post(ENV.host + '/api/chat', data, function (fb) {
                if (!fb) return;
                alert("私信已发送");
                $input.value = "";
                $modal[0].style.display = 'none';
            });
        }
    });
}

//--Update:TO DO：Valid　
function reservationForm() {
    var $modal = $('#reservation');
    if (!$modal.length) return;
    var $form = $modal[0].querySelectorAll('form');
    var $input = $form[0].querySelector('textarea');
    $form[0].on('submit', function (event) {
        event.preventDefault();
        var d = new Date();
        var currmonth = d.getMonth() + 1;
        var currday = d.getDate();
        var str = d.getFullYear() + (currmonth > 9 ? "-" + currmonth : "-0" + currmonth) + (currday > 9 ? "-" + currday : "-0" + currday);
        if (this.date.value <= str) {
            alert("请选择今天之后的日期");
            return false;
        }
        var data = {
            ClassId: $form[0].dataset.classid,
            BookDate: this.date.value,
            Remark: this.remark.value,
            Name: this.name.value
            
        };
        post(ENV.host + '/api/Order', data, function(fb){
            if (!fb) {
                alert("订课请求发送失败");
                return;
            }
            else {
                alert("订课请求已发送");
                this.date.value = "";
            }
        });
        $modal[0].style.display = 'none';

    });
}

//--Update:Add avatar and name , inset position
function commentForm() {
    var $form = $('#comment-form');
    var $input = $form[0].querySelector('input');
    $input.on('focus', function () {
        checkLogin();
    });

    //$input.on('keydown', function (event) {
    //    if (event.keyCode == 13) {
    //        alert("submit");
    //    }
    //});
    
    $form[0].on('submit', function (event) {
        
        event.preventDefault();
        var patten = new RegExp(/true/i);
        if (!patten.test($('.bar-nav')[0].dataset.ismember)) {
            goToLogin();
            return false;
        }

        if ($input.value == "") {
            $input.classList.add("error");
            return false;
        } else {
            $input.classList.remove("error");
            var data = {
                //MemberId: 1,
                ClassId: $form[0].dataset.classid,
                CommentText: $input.value
            };
            post(ENV.host + '/api/comment', data, function(fb){
            if(!fb) return;
            data.Avatar = $form[0].dataset.avatar;
            data.Name = $form[0].dataset.name;
            $('.comment-text')[0].insertAdjacentHTML('afterBegin',
              _.template($('#comment-tpl')[0].innerHTML, { item: data }));
            //$input.focus();
            var tagNum = $(".comment-num").length;
            if (tagNum > 0) {
                var commentNum = parseInt($(".comment-num")[0].innerText) + 1;
                for (var i = 0; i < tagNum; i++) {
                    $(".comment-num")[i].innerText = commentNum;
                }
            }
            $input.value = '';
            });
        }
    });
}

//--Update:Add toggle style, check login, fans number
function followMember() {
    if ($('.follow').length > 0) {
        $('.follow')[0].on('click', function (event) {
            event.preventDefault();
            var patten = new RegExp(/true/i);
            if ($('.bar-nav').length > 0 && !patten.test($('.bar-nav')[0].dataset.ismember)) {
                goToLogin();
                return false;
            }
            var $followbtn = this;
            var isFollow = !this.matches('.followed');
            var data = {
                //MemberId: 1,
                FollowingId: this.dataset.memberid,
                IsFollow: isFollow
            };
            console.log(data);
            post(ENV.host + '/api/followmember', data, function (fb) {
                if (!fb) return;
                $followbtn.classList.toggle('followed');
                $followbtn.textContent = isFollow ? "已关注" : "关注";

                var tagNum = $(".fans-num").length;
                if (tagNum > 0) {
                    var commentNum = parseInt($(".fans-num")[0].innerText) + (isFollow ? 1 : -1);
                    for (var i = 0; i < tagNum; i++) {
                        $(".fans-num")[i].innerText = commentNum;
                    }
                }
            });
        })
    }
}

//Add:Init back url and redirect path
function initLogin() {
    var backUrlKey = "backUrl";
    var backUrl = window.location.href;
    
    var patten = new RegExp(/(\/login|\/signup)/i);
    if (!patten.test(backUrl)) {
        sessionStorage.setItem(backUrlKey, backUrl);
    }
}

function initBack() {
    if ($(".nav-back").length > 0) {
        $(".nav-back")[0].on('click', function () {
            var refurl = document.referrer;
            if (refurl.indexOf(window.location.host) > 0) {
                window.history.back();
            } else {
                window.location.href = "/m";
            }
        });
    }
}

function savePosition(posX, posY) {
    var posXKey = "posx";
    var posYKey = "posy";

    if (posX > 0 && posY > 0) {
        sessionStorage.setItem(posXKey, posX);
        sessionStorage.setItem(posYKey, posY);
    }
}

function getPosX() {
    var posXKey = "posx";
    var result = sessionStorage.getItem(posXKey);
    if (result != undefined && result > 0) {
        return result;
    } else {
        return 0;
    }
}

function getPosY() {
    var posYKey = "posy";
    var result = sessionStorage.getItem(posYKey);
    if (result != undefined && result > 0) {
        return result;
    } else {
        return 0;
    }
}


function redirectAfterLogin() {
    var backUrlKey = "backUrl";
    var refPath = sessionStorage.getItem(backUrlKey);
    if (refPath == undefined || refPath == "") {
        refPath = "/m/";
    }
    window.location.href = refPath;
}