var message = message || new message_Class;

$(document).ready(function () { message.init(); });

function message_Class() {
    this.init = function () {
        $(".message-delete").click(function () { message.setMessageAsDeleted($(this)); });
        sitecommon.showLoginPop(false);
    };

    this.setMessageAsDeleted = function (delObj) {
        var savePath = "/MessageHelper/SetMessageAsDeleted";
        var contactor = delObj.attr("data-memberid");
        var messageId = delObj.attr("data-messageid");
        var paraData = { maxId: messageId, contactorId: contactor };

        consoleLog(paraData);
        $.ajax({
            url: savePath,
            type: "POST",
            dataType: "Json",
            data: paraData,
            cache: false,
            success: function (data) {
                if (data == "true") {
                    delObj.parent().parent().parent().parent().fadeOut();
                }
            }
        });
    }

    //this.initScroll = function () {
    //    $('#content').scrollPagination({
    //        'contentPage': '/Message/PageContent', // the url you are fetching the results, you can use html url, put static text in html
    //        'contentData': { pageId: 1, pageSize: 10 }, // these are the variables you can pass to the request, for example: children().size() to know which page you are
    //        'scrollTarget': $(window), // who gonna scroll? in this example, the full window
    //        'heightOffset': 10, // it gonna request when scroll is 10 pixels before the page ends
    //        'beforeLoad': function () { // before load function, you can display a preloader div
    //            $('#loading').fadeIn();
    //        },
    //        'afterLoad': function (elementsLoaded) { // after loading content, you can use this function to animate your new elements
    //            $('#loading').fadeOut();
    //            var i = 0;
    //            $(elementsLoaded).fadeInWithDelay();
    //            if ($('#content').children().size() > 100) { // if more than 100 results already loaded, then stop pagination (only for testing)
    //                $('#nomoreresults').fadeIn();
    //                $('#content').stopScrollPagination();
    //            }
    //        }
    //    });

    //    // code for fade in element by element
    //    $.fn.fadeInWithDelay = function () {
    //        var delay = 0;
    //        return this.each(function () {
    //            $(this).delay(delay).animate({ opacity: 1 }, 200);
    //            delay += 100;
    //        });
    //    };
    //}

}

