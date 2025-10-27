

$(document).ready(function () {
    return $.ajax({
        type: "GET",
        url: "/ReadHtml/SoGiaoDuc",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            buildHtml(res, "site-sgd");
            
        },
        error: function () {

        }
    });

   
});


$(document).ready(function () {
  
    return $.ajax({
        type: "GET",
        url: "/ReadHtml/BoGiaoDuc",
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (res) {
            buildHtml(res, "site-bgd");
        },
        error: function () {

        }
    });
});
/*
function buildHtml(ListItems,id) {
    str_html = "";
    if (ListItems.length > 0) {
        for (let i = 0; i < ListItems.length; i++) {
            str_html = str_html + "<li>";
            str_html = str_html + '<a href="' + ListItems[i].Link + '"><i class="fas fa-check"></i>' + ListItems[i].Title + '<lable>' + ListItems[i].Time + '</lable>' + '</a>';
            str_html = str_html + "</li>";
        }
    }
    $("#"+id).html(str_html);
    $("#" + id).mCustomScrollbar({
        theme: 'dark-3'
    });
    return str_html;
}*/

function buildHtml(res, id) {
    str_html = "";
    if (res.Success) {
        console.log("SoGiaoDuc");
        if (res.Data.length > 0) {
            for (let i = 0; i < res.Data.length; i++) {
                str_html = str_html + "<div class='partial-lien-ket-tin-tuc-item'>";
                str_html = str_html + '<a target="_blank" href="' + res.Data[i].Link + '"><i class="fas fa-check"></i>' + res.Data[i].Title + '<label>' + res.Data[i].Time + '</label>' + '</a>';
                str_html = str_html + "</div>";
            }
            $("#" + id).html(str_html);
            $('#'+id).mCustomScrollbar({
                theme: 'dark-3'
            });
        }
    } else {
        $("#"+id).html(str_html);
    }
}