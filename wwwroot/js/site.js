$(document).ready(function () {
    return $.ajax({
        type: "GET",
        url: "/SiteVisit/index",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $("#SiteVisit_Yesterday").html(data.Yesterday);            
            $("#SiteVisit_DateOfWeek").html(data.DateOfWeek);
            $("#SiteVisit_DateOfMonth").html(data.DateOfMonth);
            $("#SiteVisit_DateNow").html(data.DateNow);
            $("#SiteVisit_Total").html(data.Total);
        },
        error: function () {

        }
    });
});

function SendQuestion(Id, Ids) {
    $('#msgErrSurvey').hide();
    $('#msgSuccessSurvey').hide();
    var token = jQuery('input[name="__RequestVerificationToken"]').val();
    var headers = {};
    headers["RequestVerificationToken"] = token;

    var datailItem = { SurveyId: Id, SurveyIds: Ids, Id: $("input[name='question_customer']:checked").val(), Ids: '', IdCoQuan: 0, Title: '', Token: '' };

    let keyCapchar = $("#RecaptchaSettings_SiteKey").val();

    datailItem.Token = token;

    return $.ajax({
        type: "POST",
        url: '/Home/SendQuestion',
        headers: headers,
        data: JSON.stringify(datailItem),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.Success == true) {

                $('#msgSuccessSurvey').html(result.Msg);
                $('#msgSuccessSurvey').show();
                localStorage.setItem("flagQuestion", datailItem.SurveyId);

            } else {
                $('#msgErrSurvey').html(result.Msg);
                $('#msgErrSurvey').show();
            }
            var ht = '';
            var total = 0;
            if (result.Data != null && result.Data.length > 0) {

                result.Data.forEach((item) => {
                    total = total + item.TotalRows;
                });

                // after
                result.Data.forEach((item) => {
                    ht = ht + `<div class="survey-chart-item"><span class="survey-chart-title">` + item.QuestionTitle
                        + `<span class="survey-chart-total">(` + item.TotalRows + `)</span>` + `</span>`;

                    ht = ht + `<div>
                        <div class="progress">
                          <div class="progress-bar" role="progressbar" style="width: `+ item.TotalRows / total * 100 + `%;" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100">` + (item.TotalRows / total * 100).toFixed(2) + `%</div>
                        </div>
                    </div>`;
                    ht = ht + `</div>`;
                });
                $("#SurveyChartBody").html(ht);
            }
            $('#SurveyChart').modal('show');
        },
        error: function () {


        }
    });

    /*
    grecaptcha.ready(function () {
        grecaptcha.execute(keyCapchar, { action: 'submit' }).then(function (token) {

            
        });
    });*/
}

function ViewSurveyChart(Id, Ids) {
    $('#msgErrSurvey').hide();
    $('#msgSuccessSurvey').hide();
    var token = jQuery('input[name="__RequestVerificationToken"]').val();
    var headers = {};
    headers["RequestVerificationToken"] = token;

    var datailItem = { SurveyId: Id, SurveyIds: Ids, Id: $("input[name='question_customer']:checked").val(), Ids: '', IdCoQuan: 0, Title: '', Token: '' };

    let keyCapchar = $("#RecaptchaSettings_SiteKey").val();

    datailItem.Token = token;

    return $.ajax({
        type: "POST",
        url: '/Home/ViewSurveyChart',
        headers: headers,
        data: JSON.stringify(datailItem),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {

            if (result.Success == true) {


            } else {

            }

        },
        error: function () {


        }
    });

}
function SendNewsLetter() {
    var token = jQuery('#NewsLetterForm input[name="__RequestVerificationToken"]').val();
    var headers = {};
    headers["RequestVerificationToken"] = token;

    var Email = $("#newsletter-email").val();

    if (Email == null || Email == "") {
        ResetFormNewsLetter();
        $("#newsletter-info").html("Email không được để trống");
        $("#newsletter-info").show();

    } else {
        $("#newsletter-send").hide();
        $("#newsletter-nosend").show();

        var myModal = new bootstrap.Modal(document.getElementById('thanks-popup'), {});

        $.ajax({
            url: '/Home/NewsLetter',
            headers: headers,
            type: 'POST',
            data: JSON.stringify(Email),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {
                ResetFormNewsLetter();
                if (data.Success == true) {

                    myModal.show()
                } else {
                    $("#newsletter-info").html(data.Msg);
                    $("#newsletter-info").show();
                }

            },
            error: function () {
                ResetFormNewsLetter();
            }
        });
    }


}

function ResetFormNewsLetter() {
    $("#newsletter-send").show();
    $("#newsletter-nosend").hide();
    $("#newsletter-info").hide();
}


//----Leftmenu
//$(".lmenu ul li ul li ul .itsme").parent().parent().parent().parent().addClass("selected");

$(".leftmenu-list-group li").hover(function () {
    $(this).find('.leftmenu-dropdown-menu:first').css({ visibility: "visible", display: "none", height :"auto"}).stop().fadeIn(400);
    console.log("leftmenu-dropdown-menu hover");
}, function () {
    $(this).find('.leftmenu-dropdown-menu:first').stop().hide(200);
    console.log("leftmenu-dropdown-menu stop");
});




function toggleDisplayBotAI() {
    const div = document.getElementById('botai-frame');
    if (div.style.display === 'block') {
        div.style.display = 'none';
    } else {
        div.style.display = 'block';
    }
}

function addClass(elementId, className) {
    $('#' + elementId).addClass(className);
}

function toggleClass(elementId, className) {
    $('#' + elementId).toggleClass(className);
}

//jQuery(document).ready(function () {
    
//    function checkWindowScrollBottom() {
//        const $window = $(window);
//        const $document = $(document);
        
//        // Get scroll values
//        const scrollTop = $window.scrollTop();
//        const windowHeight = $window.height();
//        const documentHeight = $document.height();
//        const distanceFromBottom = documentHeight - (scrollTop + windowHeight);
        
//        // Check if at bottom (with tolerance)
//        const tolerance = 300;
//        const isAtBottom = distanceFromBottom <= tolerance;
        
//        console.log('windowHeight = '+windowHeight+"; scrollTop="+scrollTop+"; documentHeight="+documentHeight);

//        if (isAtBottom) {
//            $('#vnpt_ai_livechat_content').addClass('scroll-chat-bottom-0');
//            console.log('At bottom! Class added.');            
//        } else {
//            $('#vnpt_ai_livechat_content').removeClass('scroll-chat-bottom-0');
//            console.log('Not at bottom. Distance:', distanceFromBottom + 'px');
//        }
//    }
    
//    $(window).on('scroll', checkWindowScrollBottom);
//    checkWindowScrollBottom();
//});

function SubmitSearch() {
    $('form#search-all').submit();
}
