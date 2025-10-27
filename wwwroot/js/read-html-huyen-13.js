$(document).ready(function () {
    return $.ajax({
        type: "GET",
        url: "/ReadHtml/Notification_Huyen_Root?IdCoQuan=13",
        //data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            buildHtmlHuyen2(res, "site-huyenkrongnang");

        },
        error: function () {

        }
    });
});


function buildHtmlHuyen2(res, id) {

    str_html = "";
    if (res.Success) {
        if (res.Data.length > 0) {
            for (let i = 0; i < res.Data.length; i++) {
                str_html = str_html + "<div class='partial-lien-ket-tin-tuc-item'>";
                str_html = str_html + '<a href="' + res.Data[i].Link + '"><i class="fas fa-check"></i>' + res.Data[i].Title + '</label>' + '</a>';
                str_html = str_html + "</div>";
            }
            $("#" + id).html(str_html);

            $('#' + id).mCustomScrollbar({
                theme: 'dark-3'
            });
        }
    } else {
        $("#" + id).html(str_html);
    }
}



/*
$(document).ready(function () {
    return $.ajax({
        type: "GET",
        url: "/ReadHtml/Hot_Huyen_Root?IdCoQuan=13",
        //data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            buildHtmlHuyen2(res, "site-ctttinhdaklak");

        },
        error: function () {

        }
    });
});*/