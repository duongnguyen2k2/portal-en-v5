function UpdateSessionMenu(Id) {
    return $.ajax({
        type: "GET",
        url: '/Admin/Home/SetSeccionMenu/' + Id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data);
        },
        error: function () {
            console.log('dsds');
        }
    });
}

$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

jQuery('#DeleteModal').on('show.bs.modal', function (event) {
    var button = jQuery(event.relatedTarget); // Button that triggered the modal
    var recipient = button.data('whatever'); // Extract info from data-* attributes
    var ids = button.data('ids');
    var modal = jQuery(this);
    jQuery("#DeleteItemTrue").attr("data-ids", ids);
    modal.find('.delete-title').text('Bạn có chắc chắn xóa ' + recipient + '?')
})



$("#DeleteItemTrue").click(function () {
    //var url = "/Admin/@ControllerName/";
    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var ids = jQuery("#DeleteItemTrue").attr("data-ids");
    var url = jQuery("#DeleteItemTrue").attr("data-url");
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: url + 'DeleteItem/' + ids,
        headers: headers,
        data: JSON.stringify({ "Ids": ids }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
});


function UpdateStatus(Controller, Ids, Status, flag = 'Status') {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateStatus?Ids=' + Ids + "&Status=" + Status + '&flag=' + flag,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function UpdateFeatured(Controller, Ids, Featured) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateFeatured?Ids=' + Ids + "&Featured=" + Featured,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function UpdateFeaturedHome(Controller, Ids, FeaturedHome) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateFeaturedHome?Ids=' + Ids + "&FeaturedHome=" + FeaturedHome,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function UpdateStaticPage(Controller, Ids, StaticPage) {

    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    return $.ajax({
        type: "POST",
        url: '/Admin/' + Controller + '/UpdateStaticPage?Ids=' + Ids + "&StaticPage=" + StaticPage,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            window.location.reload();
        },
        error: function () {
            window.location.reload();
        }
    });
}

function MySearchForm() {
    jQuery("#MySearchForm").submit();
}


$(function () {
    $('.namepr').keyup(function () { $('.titlepr').val($('.namepr').val()); });
});
$(function () {
    $('.Changetitle').keyup(function () { $('.slug').val(string_to_slug($('.Changetitle').val())); });
});

function string_to_slug(str) {
    str = str.replace(/^\s+|\s+$/g, ''); // trim
    str = str.toLowerCase();

    // remove accents, swap ñ for n, etc
    var from = "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴáàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ";
    var to = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaeeeeeeeeeeeeeeeeeeeeeeoooooooooooooooooooooooooooooooooouuuuuuuuuuuuuuuuuuuuuuiiiiiiiiiiddyyyyyyyyyy";
    for (var i = 0, l = from.length; i < l; i++) {
        str = str.replace(new RegExp(from.charAt(i), 'g'), to.charAt(i));
    }

    str = str.replace(/[^a-z0-9 -]/g, '') // remove invalid chars
        .replace(/\s+/g, '-') // collapse whitespace and replace by -
        .replace(/-+/g, '-'); // collapse dashes
    return str;
}

function ChangeDMCoQuanLanguage(Culture, Lang, Link) {
    console.log("#Link_" + Lang);
    console.log(Link);
    $("#Link_" + Lang).val(Link);
    $("#SaveItem_" + Lang).click();
    console.log("SaveItem_" + Lang);

}
function ChangeLanguage(Culture, Lang, Link) {
    console.log("#Link_" + Culture);
    console.log(Link);
    $("#Link_" + Culture).val(Link);
    $("#SaveItem_" + Culture).click();
    console.log("SaveItem_");

}

function GetListDMTinhThanh(Level = 1) {
    var Id = $("#Model_IdCoQuan" + Level).val();
    $.ajax({
        type: "GET",
        url: "/Admin/DMTinhThanh/GetJson?Id=" + Id + '&Def=0',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            console.log(data.Data);
            $("#Model_IdCoQuan" + (Level + 1)).html(data.Data);
        },
        error: function () {

        }
    });
}

function GetListDMQuanHuyen(IdTinhThanh) {
    var Id = $("#Model_IdCoQuan" + Level).val();
    $.ajax({
        type: "GET",
        url: "/Admin/DMQuanHuyen/GetListJson?IdTinhThanh=" + IdTinhThanh,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {

            $("#Model_IdCoQuan" + (Level + 1)).html(data.Data);
        },
        error: function () {

        }
    });
}

function GetListDMPhuongXa(IdQuanHuyen, IdHtml) {
    var Id = IdQuanHuyen;
    if (IdQuanHuyen == 0) {
        Id = $("#Item_IdQuanHuyen").val();
    }

    $.ajax({
        type: "GET",
        url: "/Admin/DMPhuongXa/GetListJson?IdQuanHuyen=" + Id,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.Data != null && data.Data.length > 0) {
                var html = "--- Chọn Phường Xã ---";
                for (let i = 0; i < data.Data.length; i++) {
                    html = html + `<option value="` + data.Data[i].Id + `">` + data.Data[i].Ten + `</option>`;
                }
                $("#" + IdHtml).html(html);
            }
            //$("#Model_IdCoQuan" + (Level + 1)).html(data.Data);
        },
        error: function () {

        }
    });
}

function serializeToJson(serializer) {
    var _string = '{';
    for (var ix in serializer) {
        var row = serializer[ix];
        _string += '"' + row.name + '":"' + row.value + '",';
    }
    var end = _string.length - 1;
    _string = _string.substr(0, end);
    _string += '}';
    console.log('_string: ', _string);
    return JSON.parse(_string);
}


function DownLoadFileByUrl(headers, url, file_name) {
    $.ajax({
        url: "/Admin/ManagerFiles/DownloadFile",
        method: 'GET',
        headers: headers,
        dataType: 'binary',
        data: 'url=' + url,
        success: function (result) {

            setTimeout(function () { $('#LoaddingModal').modal('hide'); }, 500);
            var url = URL.createObjectURL(result);
            var $a = $('<a />', {
                'href': url,
                'download': file_name,
                'text': "click"
            }).hide().appendTo("body")[0].click();
            setTimeout(function () {
                URL.revokeObjectURL(url);
            }, 10000);
        }
    });
}



function ChangeLanguage(Culture, Lang, Link) {
    console.log("#Link_" + Culture);
    console.log(Link);
    $("#Link_" + Culture).val(Link);
    $("#SaveItem_" + Culture).click();
    console.log("SaveItem_");

}