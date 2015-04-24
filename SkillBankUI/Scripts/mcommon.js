// setting
window.ENV = {
    host: "http://" + window.location.host,
    imgHost: 'http://skillbank.b0.upaiyun.com'
};

var browser = {
    versions: function () {
        var u = navigator.userAgent, app = navigator.appVersion;
        return {
            trident: u.indexOf('Trident') > -1, //IE内核
            presto: u.indexOf('Presto') > -1, //opera内核
            webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核
            gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1,//火狐内核
            mobile: !!u.match(/AppleWebKit.*Mobile.*/), //是否为移动终端
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器
            iPhone: u.indexOf('iPhone') > -1, //是否为iPhone或者QQHD浏览器
            iPad: u.indexOf('iPad') > -1, //是否iPad
            webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
            weixin: u.indexOf('MicroMessenger') > -1, //是否微信 （2015-01-22新增）
            qq: u.match(/\sQQ/i) == " qq" //是否QQ
        };
    }(),
    language: (navigator.browserLanguage || navigator.language).toLowerCase()
};

function parseURL(url) {
    var parser = document.createElement('a'),
      searchObject = {},
      queries, split, i;
    // Let the browser do the work
    parser.href = url;
    // Convert query string to object
    queries = parser.search.replace(/^\?/, '').split('&');
    for (i = 0; i < queries.length; i++) {
        split = queries[i].split('=');
        searchObject[split[0]] = split[1];
    }
    return {
        protocol: parser.protocol,
        host: parser.host,
        hostname: parser.hostname,
        port: parser.port,
        pathname: parser.pathname,
        search: parser.search,
        searchObject: searchObject,
        hash: parser.hash
    };
}

// fns
window.checkImgHost = function (host, url) {
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

function closestParent(el, selector) {
    if (el.matches) {
        while (!el.matches(selector)) {
            el = el.parentNode;
        }
    } else {
        var className = selector.slice(1);
        while (!el.classList.contains(className)) {
            el = el.parentNode;
        }
    }
    return el
}

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
  if ((type === 'POST' || type === 'PUT') && opts) {
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
      if (typeof (mixpanel) != "undefined") {
          mixpanel.track("register alert");
      }
      if (window.confirm("亲，先来注册一下吧？")) {
          if (typeof (mixpanel) != "undefined") {
              mixpanel.track("register alert yes");
          }
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
        if (window.confirm("亲，先来注册一下吧？")) {
            location.href = '/m/login';
        }
        return false;
    }
    return true;
}

function goToLogin() {
    if (typeof (mixpanel) != "undefined") {
        mixpanel.track("register alert");
    }
    if (window.confirm("亲，先来注册一下吧？")) {
        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("register alert yes");
        }
        location.href = '/m/login';
    }
    return;
}

var get = request.bind(this, 'GET');
var post = request.bind(this, 'POST');
var put = request.bind(this, 'PUT');


var checkPage = function () {
  addBulletsWeget();
  removeTrashes();
  hackForModals();
  initLogin();
  initBack();
  customRadio();
  bindCloseEventToModal();
  fixedPositionBug();

  if (typeof (mixpanel) != "undefined") {
      mixpanel.track("page view");
      $(".menutab-find").on('click', function () {
          mixpanel.track("menu find");
      });
      $(".menutab-message").on('click', function () {
          mixpanel.track("menu message");
      });
  }

  //$(".menutab-course").on('click', function () {
  //    if (typeof (mixpanel) != "undefined") {
  //        mixpanel.track("menu course");
  //    }
  //    alert("此功能将稍后开放");
  //});

  //$(".menutab-mypage").on('click', function () {
  //    if (typeof (mixpanel) != "undefined") {
  //        mixpanel.track("menu mypage");
  //    }
  //    alert("此功能将稍后开放");
  //});

  $(".login-trigger").on('click', function () {
      checkLogin();
  });

  // course list search
  if (document.getElementById('course-search')) {
      switchCourseCat();
      affix();
      toggleLike();
      bindClassListTrackingEvent();
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
      bindClassDetailTrackingEvent();
  }

  // personal page
  if ($('.personal-page').length) {
      followMember();
      privateMsgForm();
      bindPersonalTrackingEvent();
  }

  //login page
  if ($('.login-page').length) {
      $('#sinaloginbtn').on('click', function () {
          if (typeof (mixpanel) != "undefined") {
              mixpanel.track("sina signup");
          }
          var backUrlKey = "backUrl";
          var refPath = sessionStorage.getItem(backUrlKey);
          if (refPath == undefined || refPath == "") {
              refPath = "/m";
          }
          location.href = "https://api.weibo.com/oauth2/authorize?client_id=111240964&response_type=code&redirect_uri=http%3A%2F%2Fwww.skillbank.cn%2Fsignup%3Fmb%3D" + encodeURIComponent(refPath);
      });
  }

  if ($('#message-page').length) {
      $('.message-tab').on('click', function () {
          $('.notification-tab')[0].classList.toggle('active');
          $('.message-tab')[0].classList.toggle('active');
          //$('.message-tab')[0].classList.remove("control-item-no-js");
          //$('.message-tab')[0].classList.add("control-item");
          $('#item2mobile')[0].classList.toggle('active');
          $('#item1mobile')[0].classList.toggle('active');
          post(ENV.host + "/api/alter?id=" + this.dataset.maxid, function () { jQuery(".red-dot").fadeOut(); });
      });
  }

  // chat page
  if ($('.chat-page').length) {

      // init textrea auto size
      jQuery('#write-box').textareaAutoSize();

      $('#write-box')[0].on('keyup', function (event) {
          var $submit = $('#form-write button')[0];
          if (this.value.length) {
              $submit.removeAttribute('disabled');
          } else {
              $submit.setAttribute('disabled', '');
          }
      })

      // bind add msg
      chatForm();
  }

  // register page
  //--Update:redirect url ,validation
  if ($('#register-page').length) {
      if (typeof (mixpanel) != "undefined") {
          mixpanel.track("signup page");
      }

      var $form = $('form')[0];
      $("#registebtn").on('click', function (event) {
          event.preventDefault();

          var patten = new RegExp(/^[1]+\d{10}/);
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

          if (typeof (mixpanel) != "undefined") {
              mixpanel.track("click signup");
          }

          var data = {
              Mobile: $form.phone.value,
              Code: $form.code.value,
              Avatar: $form.registebtn.dataset.avatar,
              Name: $form.registebtn.dataset.name
          };
          
          post(ENV.host + '/api/registe', data, function (fb) {
              console.log(fb);//-1 数据格式不符
              if (fb == 2) {
                  alert("验证失败，请检查验证码或重新发送");
              } else if (fb == 3) {
                  alert("手机号码被占用");
              } else {
                  if (typeof (mixpanel) != "undefined") {
                      mixpanel.track("submit signup");
                  }
                  redirectAfterLogin();
              }
          });

      });

      var $verifyBtn = $('.btn-grey');
      var seconds;
      $verifyBtn.on('click', function () {
          if (seconds > 0) return;
          var patten = new RegExp(/^[1]+\d{10}/);
          if (!$form.phone.value || !patten.test($form.phone.value)) {
              $form.phone.classList.add('error');//手机号码格式不对
              return;
          } else {
              $form.phone.classList.remove('error');
          }
          var url = ENV.host + '/api/Validation?mobile=' + $form.phone.value;
          seconds = 60;
          post(url, function (fb) {
              if (fb == 1) {
                  // timer
                  var t = setInterval(function () {
                      if (seconds <= 0) {
                          $verifyBtn[0].innerText = '获得验证码';
                          clearInterval(t);
                      } else {
                          $verifyBtn[0].innerText = seconds+"后可重发";
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

    // add course page
  if ($('.add-course-page.step-1').length) {
      bindHashChangeToSteps();
      selectSkill();
      changeCity();
      checkAllFillInStep1();
      initPubClassStep1();
  }

    // add course page 2
  if ($('.add-course-page.step-2').length) {
      var maxlen = 100;
      limitedText(maxlen);
      checkAllFillIn(maxlen);
      initPubClassStep2();
  }

    // add course page 3
  if ($('.add-course-page.step-3').length) {
      //chooseCover();
      $('.step-3 .btn-transparent').on('click', function () {
          //update cover and go to preview page
          initPubClassStep3(false);
          alert("go to preview page");
      });
      $('.step-3 .next').on('click', function () {
          location.href = "#success";
          //update cover and publish
          initPubClassStep3(true);
          //$('.step-name')[0].style.display = 'none';
          $('h3')[0].innerHTML = '内容已填完并被保存';
      });
  }

  
    // profile edit page
  if ($('.add-course-page.step-private').length) {
      checkAllFillInPrivate();
      //chooseAvatar();
      $('#profile-savebtn').on('click', function () { updateProfile(); });
  }

    // course manage 
  if ($('.courses-manage-page').length) {
  }
  if ($('.courses-learning-page').length || $('.courses-teaching-page').length) {
      confirmManage();
      checkAllEvaluateModal();

      // reservation modal
      $('#contact.modal .accept-reserve').on('click', function () {
          accpetOrder(this.parentNode);
      });
      
      // set active card
      $('.teaching.card').on('click', function () {
          [].forEach.call($('.teaching.card'), function (el) {
              el.classList.remove('active');
          });
          this.classList.add('active');
      });

      //Change phone number for contact popup
      $('.contact-btn').on('click', function () {
          var phone = this.parentNode.parentNode.parentNode.dataset.phone;
          $("#phone")[0].classList.add("active");
          $("#phone #makecall")[0].href = "tel:" + phone;
          $("#phone #sendsms")[0].href = "sms:" + phone;
      });
      // show modal.  for conflict's sake
      $('[data-modal]').on('click', function () { $('#' + this.dataset.modal)[0].classList.add('active'); });

      limitedTextLen($("#evaluate textarea")[0], 200);
  }

  if ($('.courses-learning-page').length) {
      //Only for learning page
      payClassCoin();
      limitedTextLen($("#paycoin textarea")[0], 200);
  }

  if ($('.setting-page').length) {
      initLikeEvent();
  }

};



// start here
checkPage();
window.addEventListener('push', checkPage);

function bindHashChangeToSteps() {
    var changeContent = function () {
        console.log(location.hash);
        var stepName = location.hash.slice(1);
        if (!stepName) return;
        $('.steps.active')[0].classList.remove('active');
        $('.content.active')[0].classList.remove('active');
        $('.steps.step-' + stepName)[0].classList.add('active');
        $('.content.step-' + stepName)[0].classList.add('active');
    }
    changeContent();
    window.onhashchange = changeContent;
}

function switchCourseCat() {
    var load = function () {
        console.log(location.hash);
        if (!location.hash.slice(1)) return;
        var query = parseURL(location.hash.slice(1)).searchObject;
        var geo_opts = {
            enableHighAccuracy: true,
            maximumAge: 30000,
            timeout: 27000
        };

        // nearby skill
        if (query.by == 0) {
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
                    var url = ENV.host + '/api/ClassList?' + 'by=' + query.by + '&type=' + query.type +
                              '&x=' + posX + '&y=' + posY + '&city=' + encodeURIComponent(cityName);
                    savePosition(posX, posY);
                    getCourses(url);
                });
            }, function () {
                console.log("Sorry, no position available.")
            }, geo_opts);
        } else if (query.by == 3) {// search
            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("search");
            }
            var url = ENV.host + '/api/ClassList?' + 'by=3&type=1&key=' + encodeURIComponent(query.key);
            //var url = ENV.host + '/api/ClassList?' + 'by=' + query.by + '&type=' + query.type + '&key=' + encodeURIComponent(query.key);
            console.log(url);
            getCourses(url);
        } else {// category ,promote
            var url = ENV.host + '/api/ClassList?' + 'by=' + query.by + '&type=' + query.type;
            console.log(url);
            getCourses(url);
        }
    }
    load();
    window.onhashchange = load;
}

function getCourses(url) {
    var $loading = $('.loading');
    var $cats = $('.search-cat-wrap a');
    $loading[0].style.display = 'block';
    get(url, function (fb) {
        if (!_.isArray(fb)) return;
        // insert html
        var tpl = $('#course-tpl')[0].innerHTML;
        $('.course-list')[0].innerHTML = _.template(tpl, { courses: fb, imgHost: ENV.imgHost });
        // active tab
        [].forEach.call($cats, function (el) {
            el.classList.remove('active');
        });
        var $active = _.find($cats, function ($cat) {
            return location.hash == $cat.getAttribute('href');
        });
        $active.classList.add('active');
        $loading[0].style.display = 'none';
    });
}

function bindCloseEventToModal() {
    if (!$('.modal').length) return;
    // jQuery(document).on('touchend click', function(e){
    document.body.addEventListener('touchend', function (e) {
        if (e.target.classList.contains('content')) {
            var $modal = e.target.parentNode;
            $modal.classList.remove('active');
        }
    })
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

            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("like");
            }

        } else {
            return;
        }

    });
}

function addBulletsWeget() {
    if (!document.getElementById('mySlider')) return;
    // make bullet live
    var len = $('#mySlider .slide').length;
    var bulletsStr = "";
    var $bulletsContainer = $('#bullets ul')[0];
    var slide = function (event) {
        var curr = event.detail.slideNumber;
        var $active = $("#bullets .active")[0];
        var $next = $('#bullets li')[curr];
        $active.classList.remove('active');
        $next.classList.add('active');
    }
    $('#mySlider')[0].addEventListener('slide', slide);

    if ($('#bullets ul').length) {
        // add html
        for (var i = 0; i < len; i++) {
            if (i === 0) {
                bulletsStr += "<li class='active'></li>\n";
            } else {
                bulletsStr += "<li></li>\n";
            }
        }
        $bulletsContainer.innerHTML = bulletsStr;
    }
}

function affix() {
    var $searchCat = document.getElementById('search-cat');
    var $wrap = jQuery('.search-cat-wrap');
    var offTop = $searchCat.offsetTop;
    var toLeft;

    $('.content')[0].onscroll = function (event) {
        if (this.scrollTop >= offTop) {
            $wrap.addClass('affix');

            // to left as the same after append
            toLeft = jQuery('.search-cat-wrap .inner')[0].scrollLeft;
            if (!jQuery('body > .search-cat-wrap').length) $wrap.detach().appendTo('body');
            jQuery('.search-cat-wrap .inner')[0].scrollLeft = toLeft;
        } else {
            $wrap.removeClass('affix')

            // to left as the same after append
            toLeft = jQuery('.search-cat-wrap .inner')[0].scrollLeft;
            if (jQuery('body > .search-cat-wrap').length) $wrap.detach().appendTo($searchCat);
            jQuery('.search-cat-wrap .inner')[0].scrollLeft = toLeft;
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
            $input.classList.add("error");
        } else {
            $input.classList.remove("error");
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

                    if (typeof (mixpanel) != "undefined") {
                        mixpanel.track("chat on chatpage");
                    }
                    // scroll to newest msg
                    $('.content')[0].scrollTop = 1000000;
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
                $modal[0].classList.remove('active');
                if (typeof (mixpanel) != "undefined") {
                    mixpanel.track("chat");
                }
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
            this.date.classList.add("error");
            alert("请选择今天之后的日期");
            return false;
        } else {
            this.date.classList.remove("error");
        }

        if (this.name.value == "") {
            this.name.classList.add("error");
            return false;
        } else {
            this.name.classList.remove("error");
        }

        //var patten = new RegExp(/^[1]+[3,4,5,8]+\d{9}/);
        var patten = new RegExp(/^[1]+\d{10}/);
        if (!this.phone.value || !patten.test(this.phone.value)) {
            this.phone.classList.add("error");//手机号码格式不对
            return;
        } else {
            this.phone.classList.remove("error");//手机号码格式不对
        }

        var data = {
            ClassId: $form[0].dataset.classid,
            BookDate: this.date.value,
            Remark: this.remark.value,
            Name: this.name.value,
            Phone: this.phone.value
        };

        post(ENV.host + '/api/Order', data, function(fb){
            if (!fb) {
                alert("订课请求发送失败");
                return;
            }
            else {
                alert("订课请求已发送");
                $form[0].date.value = "";
                if (typeof (mixpanel) != "undefined") {
                    mixpanel.track("book class");
                }
            }
        });
        
        //$modal[0].style.display = 'none';
        $modal[0].classList.remove('active');

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

            if (typeof (mixpanel) != "undefined") {
                mixpanel.track("leave comment");
            }

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
    if (!$('.follow').length) return;
    $('.follow')[0].on('click', function (event) {
        event.preventDefault();
                
        var patten = new RegExp(/true/i);
        if ($('.bar-nav').length > 0 && !patten.test($('.bar-nav')[0].dataset.ismember)) {
            goToLogin();
            return false;
        }

        var self = this;
        var followId = this.dataset.memberid;
        if (followId === "-1") {
            myAlert("亲，自恋不可以哦", 2);
            return false;
        } 
        var toggleName = 'btn-olive';
        var isFollow = self.classList.contains(toggleName);
        
        var data = {
            FollowingId: followId,
            IsFollow: isFollow
        };
        post(ENV.host + '/api/followmember', data, function (fb) {
            if (!fb) return;
            self.classList.toggle(toggleName);
            self.classList.toggle("btn-grey");
            self.textContent = isFollow ? "已关注" : "关注";

            var tagNum = $(".fans-num").length;
            if (tagNum > 0) {
                var commentNum = parseInt($(".fans-num")[0].innerText) + (isFollow ? 1 : -1);
                for (var i = 0; i < tagNum; i++) {
                    $(".fans-num")[i].innerText = commentNum;
                }
            }
        });

        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("follow");
        }
    })
}

function showRangeVal(el) {
    var val = el.value;
    jQuery(el).siblings('.num')
      .text(val)
      .css({
          left: val + '%',
          marginLeft: -val * 30 * 0.01 + 'px'
      })
}



//Added functioins
function bindClassListTrackingEvent() {
    if (typeof (mixpanel) != "undefined") {
        $(".classlist-avatar").on('click', function () {
            mixpanel.track("avatar on classlist");
        });

        $(".classlist-cover").on('click', function () {
            mixpanel.track("cover on classlist");
        });
    }
}

function bindClassDetailTrackingEvent() {
    if (typeof (mixpanel) != "undefined") {
        $(".classdetail-avatar").on('click', function () {
            mixpanel.track("avatar on classlist");
        });

        $(".ico-coin").on('click', function () {
            mixpanel.track("popup coin");
        });

        $(".classdetail-book").on('click', function () {
            mixpanel.track("popup book");
        });
        
        $(".classdetail-chat").on('click', function () {
            mixpanel.track("popup chat on classdetail");
        });
    }
}

function bindPersonalTrackingEvent() {
    if (typeof (mixpanel) != "undefined") {
        $(".personal-chat").on('click', function () {
            mixpanel.track("popup chat on personal");
        });

        $(".personal-cover").on('click', function () {
            mixpanel.track("cover on personal");
        });
    }
}


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


function customRadio() {
    $('.custom-radio input[type=radio]').on('change', function () {
        var $icon = this.parentNode.querySelector('.icon');
        [].forEach.call($('.custom-radio .icon'), function (el) {
            el.classList.remove('selected');
        })
        $icon.classList.add('selected');
    })
}

function limitedText(maxlen) {
    var $textarea = $('.limitedText textarea')[0];
    var $num = $('.limitedText .warning span')[0];
    var $nextBtn = $('.main .next')[0];
    var changeNum = function () {
        var len = this.value.length
        if (len < maxlen) {
            $num.innerHTML = maxlen - len;
        } else {
            $num.innerHTML = 0;
        }
    }
    $textarea.on('change', changeNum);
    $textarea.on('keyup', changeNum);
}

function limitedTextLen($textarea, length) {
    if ($textarea) {
        var changeText = function () {
            var len = this.value.length;
            if (len > length) {
                this.value = this.value.substr(0, length);
            }
        }
        $textarea.on('change', changeText);
        $textarea.on('keyup', changeText);
    }
}

//update validation and init check
function checkAllFillInStep1() {
    var $form = $('.step-1 form')[0];
    var $nextBtn = $('.step-1 .main .next')[0];
    var checkInputs = function () {
        if ($nextBtn) {
            var ifAllFillIn = $form.city.value && ($form.cityid.value !== 0) &&
                              $form['skill-cat'].value &&
                              $form.categoryid.value;
            $nextBtn.classList[ifAllFillIn ? 'remove' : 'add']('disabled');
        };
        [].forEach.call($('.step-1 input[name=city], .step-1 input[name=cityid], .step-1 select[name=skill-cat], .step-1 select[name=skill-sub-cat]'),
          function (el) {
              el.on('change', checkInputs);
              el.on('keyup', checkInputs);
          }
        );
    }
    checkInputs();//init check event for class edit
}

//update validation and init check
function checkAllFillIn(maxlen) {
    var $form = $('.step-2 form')[0];
    var $nextBtn = $('.step-2 .main .next')[0];
    var checkInputs = function () {
        if ($nextBtn) {
            var ifAllFillIn = $('input[type="radio"]:checked').length &&
                              $form.courseName.value &&
                              $form.highlight.value &&
                              $form.intro.value.length >= maxlen ? true : false;
            $nextBtn.classList[ifAllFillIn ? 'remove' : 'add']('disabled');
        };
        [].forEach.call($('.custom-radio input[type=radio], input[name=courseName],' +
          'textarea[name=highlight], textarea[name=intro]'),
          function (el) {
              el.on('change', checkInputs);
              el.on('keyup', checkInputs);
          }
        );
    }
    checkInputs();//init check event for class edit
}

function initPubClassStep1() {
    $('.step-1 .btn.next').on('click', function () {
        var $form = $('.step-1 form')[0];
        var data = {
            "ClassId": $('#classid')[0].value,
            "CityId": ($form.cityid.value == "" ? "0" : form.cityid.value),
            "Category": $form.categoryid.value,
            "Skill": $('#skilllevel')[0].textContent,
            "Teach": $('#teachlevel')[0].textContent
        };
        
        updateClassInfo(data);
    });
}

function initPubClassStep2() {
    $('.step-2 .btn.next').on('click', function () {
        var $form = $('.step-2 form')[0];
        var data = {
            "ClassId": $('#classid')[0].value,
            "Level": $('.step-2 input[type="radio"]:checked')[0].value,
            "Title": $form.courseName.value,
            "Summary": $form.intro.value,
            "WhyU": $form.highlight.value,
            "Location": $form.location.value,
            "Period": $form.period.value,
            "Available": $form.available.value,
            "Remark": $form.remark.value
        };

        updateClassInfo(data);
    });
}

function initPubClassStep3(isPublish) {
    var $form = $('.step-3 form')[0];
    var data = {
        "ClassId": $('#classid')[0].value,
        "Cover": $form.savekey.value,
        "IsPublish": isPublish
    };
    console.log(data);
    updateClassInfo(data);
}


//Add student/teacher review (disable resubmit), TO DO:Test API later
function checkAllEvaluateModal() {
    var $form = $('#evaluate form')[0];
    $('#evaluate .btn-evaluate')[0].on('click', function () {
        $subbtn = this;
        if (!$subbtn.matches('.disable') && $('#evaluate input[type="radio"]:checked').length) {
            if (validHasValue($form.message)) {
                $subbtn.classList.add("disable");
                var feedback = $('#evaluate input[type="radio"]:checked')[0].value;
                var comment = $form.message.value;

                var data = {
                    "OrderId": $('.teaching.active')[0].dataset.orderid,
                    "Feedback": feedback,
                    "Comment": comment,
                    "IsStudent": ($subbtn.dataset.student==="1")
                };
                console.log(data);
                post(ENV.host + '/api/orderreview', data, function (fb) {
                    if (fb) {
                        $('#evaluate')[0].classList.remove('active');
                        toDidabledState($('.teaching.active')[0], "授课结束");
                        $form.message.value = "";
                    } else {
                        alert("提交失败或已被评价");
                    }
                    $subbtn.classList.remove("disable");
                });
            }
        }
    });
}

function chooseCover() {
    var $uploader = $('.course input[type=file]')[0];
    $uploader.on('change', function () {
        if (!this.files[0]) return;
        if (!/image\/\w+/.test(this.files[0].type)) {
            alert("请选择图片格式文件");
            return;
        }
        $('#imagefileext')[0].value = ("." + this.files[0].name.match(/[^\.]+$/)).toLowerCase();
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) {
            $('.cover-holder')[0].innerHTML = '<img src="' + oFREvent.target.result + '" />'
            $("#preview-cover")[0].src = oFREvent.target.result;

            var width = $('.cover-holder img')[0].width;
            var minHeight = parseInt(width * 3 / 4);
            var height = $('.cover-holder img')[0].height;
            height = (height > minHeight) ? minHeight : height;
            $('#imagefilesetting')[0].value = '0,0,' + width + ',' + height;

            $('.step-name')[0].style.display = 'none';
            [].forEach.call($('.right .btn'), function (el) {
                el.classList.remove('disabled');
            });
            $('.right .btn')[1].classList.add('border-none');
            $('.right .btn')[1].classList.add('btn-olive');
            $('h3')[0].innerHTML = '内容已填完并被保存';
        };
    });
}

function chooseAvatar() {
    var $uploader = $('.edit-avatar input[type=file]')[0];
    $uploader.on('change', function () {
        if (!this.files[0]) return;
        if (!/image\/\w+/.test(this.files[0].type)) {
            alert("请选择图片格式文件");
            return;
        }
        $('#imagefileext')[0].value = ("." + this.files[0].name.match(/[^\.]+$/)).toLowerCase();
        var oFReader = new FileReader();
        oFReader.readAsDataURL(this.files[0]);
        oFReader.onload = function (oFREvent) {
            $('.edit-avatar img')[0].outerHTML = '<img class="avatar" src="' + oFREvent.target.result + '" />';
            var width = $('.edit-avatar img')[0].width;
            var height = $('.edit-avatar img')[0].height;
            width = (height > width) ? width : height;
            console.log($('.edit-avatar img')[0]);
            $('#imagefilesetting')[0].value = '0,0,' + width + ',' + width;

            [].forEach.call($('.right .btn'), function (el) {
                el.classList.remove('disabled');
            });
            $('.right .btn')[0].classList.add('border-none');
            $('.right .btn')[0].classList.add('btn-olive');
        };
    });
}

function checkAllFillInPrivate() {
    var $form = $('form')[0];
    var $btn1 = $('.main .right .btn')[0];
    //var $btn2 = $('.main .right .btn')[1];
    var checkInputs = function () {
        var ifAllFillIn = $('input[type="radio"]:checked').length &&
                          $form.realname.value &&
                          $form.city.value &&
                          $form.introSelf.value ? true : false;
        $btn1.classList[ifAllFillIn ? 'remove' : 'add']('disabled');
        $btn1.classList[ifAllFillIn ? 'add' : 'remove']('border-none');
        $btn1.classList[ifAllFillIn ? 'add' : 'remove']('btn-olive');
    };
    [].forEach.call($('.custom-radio input[type=radio], input[name=signature], input[name=realname],' +
      'input[name=city], textarea[name=introSelf]'),
      function (el) {
          el.on('change', checkInputs);
          el.on('keyup', checkInputs);
      }
    );
}

function checkAllFillInImage(isAvatar) {
    if (isAvatar) {
        var $form = $('form')[0];
        var $btn1 = $('.main .right .btn')[0];
        var ifAllFillIn = $('input[type="radio"]:checked').length &&
                          $form.realname.value &&
                          $form.city.value &&
                          $form.introSelf.value ? true : false;
        $btn1.classList[ifAllFillIn ? 'remove' : 'add']('disabled');
        $btn1.classList[ifAllFillIn ? 'add' : 'remove']('border-none');
        $btn1.classList[ifAllFillIn ? 'add' : 'remove']('btn-olive');
    } else {
        [].forEach.call($('.right .btn.disabled'), function (el) {
            el.classList.remove('disabled');
        });
        $('.right .btn')[1].classList.add('border-none');
        $('.right .btn')[1].classList.add('btn-olive');
    }
       
}

function PreviewImage() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("uploadImage").files[0]);

    oFReader.onload = function (oFREvent) {
        document.getElementById("uploadPreview").src = oFREvent.target.result;
    };
}

function selectSkill() {
    var options_tpl = '<% _.forEach(list, function(item) { %><option value=<%- item.value %> > <%- item.name %> </option><% }); %>';;
    var $skillCat = $('#skill-cat')[0];
    var $skillSubCat = $('#skill-sub-cat')[0];
    var $category = $('#categoryid')[0];
    
    function renderSubCat(subCats) {
        var subitems = subCats.split(",");//.substr(1, subCats.length)
        var citiesOptions = "";
        for (i = 1; i < subitems.length;i++)
        {
           var optItem = subitems[i].split(";");
           citiesOptions += "<option value=\"" + optItem[1] + "\">" + optItem[0] + "</option>";
        }
        $skillSubCat.innerHTML = citiesOptions;
    }
    
    $skillCat.on('change', function () {
        var subcats = this.options[this.selectedIndex].dataset.subcats;
        if (subcats && subcats != undefined) {
            renderSubCat(subcats);
            $category.value = "";
            $skillSubCat.style.display = 'block';
        } else {
            $category.value = this.value;
            $skillSubCat.style.display = 'none';
        }
    });

    $skillSubCat.on('change', function () {
        $category.value = this.value;
    });
}

function changeCity() {
    var $cityInput = $('.step-1 #city')[0];
    $cityInput.on('change', function () {
        var $cityidInput = $('.step-1 #cityid')[0];
        var $nextBtn = $('.step-1 .main .next')[0];
        post(ENV.host + '/api/city?cityName=' + $cityInput.value, function (fb) {
            if (fb > 0) {
                $cityInput.classList.remove("error");
                $cityidInput.value = fb;
                $nextBtn.classList.remove('disabled');
            } else {
                $cityInput.classList.add("error");
                $cityidInput.value = 0;
                $nextBtn.classList.add('disabled');
                return;
            }
        });
    });
}

function fixedPositionBug() {
    if (browser.versions.iPhone || browser.versions.iPad) {
        [].forEach.call($('textarea,input,select'), function (el) {
            el.on('focus', function () {
                [].forEach.call($('.bar'), function (el) {
                    el.style.position = 'absolute';
                });
            });
            el.on('blur', function () {
                [].forEach.call($('.bar'), function (el) {
                    el.style.position = '';
                });
            });
        });
    }
}

function myAlert(msg, seconds) {
    var $warning = $('.my-alert')[0];
    var $inner = $('.my-alert .inner')[0];
    seconds = seconds ? seconds : 2;
    $inner.innerHTML = msg;
    $warning.style.display = 'flex';
    var t = setTimeout(function () {
        $warning.style.display = '';
    }, seconds * 1000)
}


// Profile page update info
function updateProfile() {
    //event.preventDefault();
    //uploadImage(0);
    var $form = $('form')[0];
    $form.city.classList.remove("error");
    var self = event.target;
    var name = $form.realname.value;
    var avatar = ($form.savekey.value == "") ? "" : ($form.savekey.value);
    var city = $form.city.value;
    var intro = $form.introSelf.value;
    var gender = ($("input[name='sex'][value='male']:checked").length > 0);
    var data = {
        "Avatar": avatar,
        "Name": name,
        "CityName": city,
        "Intro": intro,
        "Gender": gender
    };
    
    put(ENV.host + '/api/member', data, function (fb) {
        if (fb === 1) {
            //window.location.href = "/m/personal";
            myAlert("保存成功", 2);
            $('#profile-savebtn')[0].classList.add("disabled");
        }
        else if (fb === 2) {
            myAlert("保存失败,请稍后再试", 2);
            return;
        } else if (fb === 3) {
            $form.city.classList.add("error");
            return;
        }
    });
}

function initLikeEvent() {
    $('.follow').on('click', function (event) {
        event.preventDefault();

        var self = this;
        var toggleName = 'btn-olive';
        var isFollow = self.classList.contains(toggleName);
        var followId = this.dataset.memberid;
        if (followId === "-1") {
            myAlert("亲，自恋不可以哦", 2);
            return false;
        }

        var data = {
            FollowingId: followId,
            IsFollow: isFollow
        };

        post(ENV.host + '/api/followmember', data, function (fb) {
            if (!fb) return;
            self.classList.toggle(toggleName);
            self.classList.toggle("btn-grey");
            self.textContent = isFollow ? "已关注" : "加关注";
        });

        if (typeof (mixpanel) != "undefined") {
            mixpanel.track("follow");
        }
    })
}

//For order status change (Teaching & Learning page)
function confirmManage() {
    [].forEach.call($('.confirm'), function (el) {
        el.on('click', function () {
            if (el.classList.contains('confirm-refuse-reserve')) {
                if (window.confirm("拒绝订课?")) updateOrderStatus(el, 2);
            } else if (el.classList.contains('content')) {
            } else if (el.classList.contains('accept-draw-back')) {
                if (window.confirm("接受退币?")) updateOrderStatus(el, 7);
            } else if (el.classList.contains('refuse-draw-back')) {
                if (window.confirm("拒绝退币?")) updateOrderStatus(el, 8);
            } else if (el.classList.contains('apply-draw-back')) {
                if (window.confirm("申请退币?")) updateOrderStatus(el, 6);
            } else if (el.classList.contains('cancel-book')) {
                if (window.confirm("取消订课?")) updateOrderStatus(el, 3);
            }
        })
    });
}

function toDidabledState(el, desc) {
    var $card = closestParent(el, '.card');
    var $tpl = $('#teaching-disabled-tpl')[0].innerHTML;
    $tpl = _.template($tpl, { person: $card.dataset, text: desc });
    $card.outerHTML = $tpl;
}

function toAcceptedState($card) {
    var $tpl = $('#teaching-accepted-tpl')[0].innerHTML;
    $tpl = _.template($tpl, { person: $card.dataset });
    $card.outerHTML = $tpl;
}

function accpetOrder($form) {
    var $name = $form.name;
    var $phone = $form.phone;
    var $card = $('.teaching.active')[0];
    var datas = $card.dataset;
    var orderId = datas.orderid
    if (validHasValue($name) && validPhone($phone)) {
        var data = {
            "Status": 4,
            "MemberId": datas.memberid,
            "Title": datas.course,
            "Name": datas.name,
            "Phone": datas.phone,
            "Email": datas.email,
            "MyName": $name.value,
            "MyPhone": $phone.value
        };

        console.log(data);
        put(ENV.host + '/api/order/' + orderId, data, function (fb) {
            if (fb === 2) {
                alert("学生课币不足，接受订课失败");
            } else if (fb === 3) {
                alert("订单状态已经改变，请刷新");
            } else if (fb === -1) {
                alert("操作失败");
            } else {
                $('#contact')[0].classList.remove('active');
                toAcceptedState($card);
            }
            return;
        });
    } else {
        return;
    }
}

function updateOrderStatus(el, status) {
    var $card = closestParent(el, '.card');
    var datas = $card.dataset;
    var orderId = datas.orderid;

    var data = {
        "Status": status,
        "MemberId": datas.memberid,
        "Title": datas.course,
        "Name": datas.name,
        "Phone": datas.phone,
        "Email": datas.email
    };
    console.log(data);
    put(ENV.host + '/api/order/' + orderId, data, function (fb) {
        if (fb === 3) {
            alert("订单状态已经改变，请刷新");
            return;
        } else if (fb === -1) {
            alert("操作失败");
            return;
        } else {
            if (status === 2) {//T
                toDidabledState(el, "未被接受");
            } else if (status === 3) {//S
                toDidabledState(el, "已被取消");
            } else if (status === 6) {//S
                toDidabledState(el, "退币申请");
            } else if (status === 7) {//T
                toDidabledState(el, "退币成功");
            } else if (status === 8) {//T
                toDidabledState(el, "退币失败");
            }
        }
    });

    //if (typeof (mixpanel) != "undefined") {
    //    mixpanel.track("follow");
    //}
}

function updateClassInfo(data) {
    put(ENV.host + '/api/course/' + data.ClassId, data, function (fb) {
        if (fb > 0) {
            if (data.City != null) {
                $('#preview-city')[0].innerText = data.City;
            } else if (data.Title != null) {
                $('#preview-title1')[0].innerText = data.Title;
                $('#preview-title2')[0].innerText = data.Title;
            }
            return;
        } else {
            myAlert("保存失败", 2);
        }
    });
}

//TO DO:Test API later
function payClassCoin() {
    var $form = $('#paycoin form')[0];
    if ($form.length > 0) {
        $('#paycoin .btn-paycoin')[0].on('click', function () {
            $subbtn = this;
            if (!$subbtn.matches('.disable') && $('#evaluate input[type="radio"]:checked').length) {
                if (validHasValue($form.message)) {
                    $subbtn.classList.add("disable");
                    var feedback = $('#evaluate input[type="radio"]:checked')[0].value;
                    var comment = $form.message.value;
                    var $card = $('.teaching.active')[0];
                    var datas = $card.dataset;

                    var data = {
                        "Status": 9,
                        "MemberId": datas.memberid,
                        "Title": datas.course,
                        "Name": datas.name,
                        "Phone": datas.phone,
                        "Email": datas.email,
                        "Feedback": feedback,
                        "Comment": comment,
                    };
                    console.log(data);

                    put(ENV.host + '/api/order/' + datas.orderid, data, function (fb) {
                        if (fb === 3) {
                            alert("订单状态已经改变，请刷新");
                            return;
                        } else if (fb === -1) {
                            alert("操作失败");
                            return;
                        } else {
                            $('#paycoin')[0].classList.remove('active');
                            toDidabledState($card, "授课结束");
                            $form.message.value = "";
                            return;
                        }
                        $subbtn.classList.remove("disable");
                    });
                }
            }
        });
    }
}

// Upload images para:type 0 avatar 1publish 2preview
function uploadImage(type) {
    var url;
    var isPublish = false;
    if (type === 0) {
        $input = $('.edit-avatar input[type=file]')[0];
        url = "/API/UploadAvatar";
    } else if (type === 1) {
        $input = $('.course-list input[type=file]')[0];
        url = "/API/UploadCover";
    } else if (type === 2) {
        $input = $('.course-list input[type=file]')[0];
        url = "/API/UploadCover";
        isPublish = true;
    }

    if (!$input.files.length) return;
    var formData = new FormData();
    var file = $input.files[0];
    formData.append('file', file);
    formData.append('imagefileext', $('#imagefileext')[0].value);
    formData.append('imagefilesetting', $('#imagefilesetting')[0].value);
    formData.append('imagefilename', $('#imagefilename')[0].value);
    if (isPublish) {
        formData.append('ispublish', "1");
    }
    console.log(formData);
    uploadFile(formData, url, function (data) {
        if (type === 0) {
            alert("保存成功");
            myAlert("保存成功", 2);
        } else if (type === 1) {
            alert("go to classpreview");
            //location.href = '/m/classpreview';
        } else if (type === 2) {
            alert("发布成功");
            myAlert("发布成功", 2);
        }
    });
}

function uploadFile(formData, url, callback) {
    var xhr = new XMLHttpRequest();
    xhr.open('POST', url);
    xhr.onload = function () {
        callback(JSON.parse(xhr.response));
    };
    xhr.send(formData);
}

function getUpCloudOptions(isAvatar) {
    post(ENV.host + '/api/UpCloud?fileName=' + $('#imagefilename')[0].value + $('#imagefileext')[0].value + "&isAvatar=" + isAvatar, function (fb) {
            if (fb) {
            console.log(fb);
            $('#policy')[0].value = fb.Policy;
            $('#signature')[0].value = fb.Signature;
            $('#savekey')[0].value = fb.SaveKey;
        }
    });
    checkAllFillInImage(isAvatar);
}

//validation functions
function validHasValue(el) {
    if (el.value) {
        el.classList.remove("error");
        return true;
    } else {
        el.classList.add("error");
        return false;
    }
}

function validPhone(el) {
    var patten = new RegExp(/^[1]+\d{10}/);
    if (el.value && patten.test(el.value)) {
        el.classList.remove("error");
        return true;
    } else {
        el.classList.add("error");//手机号码格式不对
        return false;
    }
}







