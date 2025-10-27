

$(document).ready(function () {
    return $.ajax({
        type: "GET",
        url: "/ReadHtml/DakLak",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            buildHtmlTinhXa(res, "site-ctttinhdaklak");
        },
        error: function () {

        }
    });
});
/*
$(document).ready(function () {
    return $.ajax({
        type: "GET",
        url: "/ReadHtml/Xa",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            buildHtmlTinhXa(res, "site-xa");
        },
        error: function () {

        }
    });
});*/



function buildHtmlTinhXa(res, id) {
    str_html = "";
    if (res.Success) {
        
        if (res.Data.length > 0) {
            for (let i = 0; i < res.Data.length; i++) {
                str_html = str_html + "<div class='partial-lien-ket-tin-tuc-item'>";
                str_html = str_html + '<a href="' + res.Data[i].Link + '"><i class="fas fa-check"></i>' + res.Data[i].Title + '<label>' + res.Data[i].Time + '</label>' + '</a>';
                str_html = str_html + "</div>";
            }
            $("#" + id).html(str_html);
            console.log('#' + id);
            $('#' + id).mCustomScrollbar({
                theme: 'dark-3'
            });
        }
    } else {
        $("#" + id).html(str_html);
    }
}