 $(function () {
	$('#ShowStartDate').datetimepicker({
		format: 'DD/MM/YYYY',

	});
	$('#ShowEndDate').datetimepicker({
		format: 'DD/MM/YYYY',
	});
});

$('document').ready(function () {

		$('#DetailArticle').on('hidden.bs.modal', function (e) {
			$("#article-title").html("Chi Tiết Bài Viết");
			$("#article-content").html('<div class="text-center"><div class="lds-dual-ring"></div></div>');
			var str = '<button type="button" class="btn btn-secondary" data-dismiss="modal"><i class="fas fa-times"></i> Đóng</button>';
			$("#footer-detail-article").html(str);
		});
		
		jQuery('#PublicArticleModal').on('show.bs.modal', function (event) {
			var button = jQuery(event.relatedTarget); // Button that triggered the modal
			var recipient = button.data('whatever'); // Extract info from data-* attributes
			var ids = button.data('ids');
			var modal = jQuery(this);
			jQuery("#ArticleStatusItemTrue").attr("data-ids", ids);
			jQuery("#article-content-tmp").html("Bạn muốn chuyển bài viết tạm sang chính thức?");
		});


});
function ShowUpdateArticlesTransfer(Ids) {
    $('#ArticlesTransfer').modal('show');
    $.ajax({
        url: "/Admin/Articles/ArticlesTransfer?Id=" + Ids,
        method: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#article-transfer-title").html(result.Title);
            $("#article-transfer-content").html(result.FullText);
            var str = '<button type="button" class="btn btn-secondary mr-2" data-dismiss="modal"><i class="fas fa-times"></i> Đóng</button>';
            str = str + ' <button onclick="UpdateArticlesTransfer(\'' + Ids + '\',' + result.Id + ')" type="button" class="btn btn-success"><i class="fas fa-cloud-download-alt"></i> Cập nhật tin liên thông</button>';
            $("#footer-detail-article-transfer").html(str);
        }
    });
}

function UpdateArticlesTransfer(Ids, RootNewsId) {
    $("#articlestransfer-loading").show();
    $("#ArticlesTransfer .alert").removeClass("alert-danger");
    $("#ArticlesTransfer .alert").removeClass("alert-success");
    $.ajax({
        url: "/Admin/Articles/UpdateArticlesTransfer?Id=" + Ids + "&RootNewsId=" + RootNewsId,
        method: 'POST',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#articlestransfer-loading").hide();
            $("#ArticlesTransfer .alert").show();
            if (result.Success) {
                if (result.Data > 0) {
                    $("#ArticlesTransfer .alert").addClass("alert-success");
                    $(".articlestransfer-alert-msg").html("cập nhật tin liên thông thành công");
                } else if (result.Data == -1) {
                    $("#ArticlesTransfer .alert").addClass("alert-danger");
                    $(".articlestransfer-alert-msg").html("Tin liên thông đã được cập nhật mới nhất.");
                }
            } else {
                $("#ArticlesTransfer .alert").addClass("alert-danger");
                $(".articlestransfer-alert-msg").html(result.Msg);
            }
            $('#ArticlesTransfer .modal-body').scrollTop(0);

        }
    });
}
/*
function CreateAudio(Id, Ids) {
    $("#create_audio_" + Id).hide();
    $("#loading_audio_" + Id).show();

    $("#SuccessModal").show();
    
    $.ajax({
        url: "/Admin/Articles/CreateAudio?Id=" + Id + "&Ids=" + Ids,
        method: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#create_audio_" + Id).show();
            $("#loading_audio_" + Id).hide();
           
            $('#ArticlesTransfer .modal-body').scrollTop(0);

        }
    });
}
*/
$("#ExportExcel").click(function () {
    
    $('.loading-title').html('');
    let str_title = "Đang xuất dữ liệu báo cáo Từ ngày: <strong>" + $("#ShowShowStartDate").val() + "</strong> Tời ngày: <strong>" + $("#ShowShowEndDate").val() +"</strong>";
    $('.loading-title').html(str_title);

    var form = serializeToJson($("#SearchFrorm").serializeArray());
    var token = jQuery('input[name="__RequestVerificationToken"]').val();
    var url = "/Admin/Articles/TaoFileBaoCao";
    var headers = {};
    headers["RequestVerificationToken"] = token;

    var result = $.ajax({
        type: "POST",
        url: url,
        headers: headers,
        data: JSON.stringify(form),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            DownLoadFileByUrl(headers, data.Data, "danh-sach-bai-viet.xlsx");
        },
        error: function () {
            $('#LoaddingModal .loadding-title').html("Lỗi khi tải file ...");
            console.log(data.Msg);

        }
    });
});

async function CreateAudio(Id, Ids) {
    var form = $('#AjaxDeleteForm');
    var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    $("#create_audio_" + Id).hide();
    $("#loading_audio_" + Id).show();


    console.log("CreateAudio");

    var result = await $.ajax({
        type: "POST",
        url: '/Admin/Articles/CreateAudio?Ids=' + Ids,
        headers: headers,
        data: JSON.stringify({}),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: data => {

            if (data.Code == "v2") {
                $("#create_audio_" + Id).show();
                $("#loading_audio_" + Id).hide();
                if (data.Success == true) {

                    $("#SuccessModal").modal('show');
                    $(".modal-success-title").html(data.Msg);
                    setTimeout(function () { window.location.reload(); }, 2000);
                }
                else {
                    $(".modal-error-title").html(data.Msg);
                    $("#ErrorModal").modal('show');
                }
            } else {
                //return data;
                /*
                async setTimeout(function () {
                    result2 = await $.ajax({
                        type: "POST",
                        url: '/Admin/Articles/DownloadAudioV1?Ids=' + Ids,
                        headers: headers,
                        data: JSON.stringify(data.Data),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: resultV1 => {
                            $("#create_audio_" + Id).show();
                            $("#loading_audio_" + Id).hide();
                            if (data.Success == true) {
                                $("#SuccessModal").modal('show');
                                $(".modal-success-title").html(data.Msg);
                                setTimeout(function () { window.location.reload(); }, 2000);
                            } else {
                                $(".modal-error-title").html(data.Msg);
                                $("#ErrorModal").modal('show');
                            }
                        },
                        error: function () {
                            $("#create_audio_" + Id).show();
                            $("#loading_audio_" + Id).hide();
                        }
                    });
                }, 3000);*/
            }


        },
        error: function () {
            $("#create_audio_" + Id).show();
            $("#loading_audio_" + Id).hide();
        }
    }).done(data => {
        console.log("done", data);

        if (data.Code == "v1" && data.Success == true) {
            setTimeout(function () {
                console.log("Donwload Audio 1");
                $.ajax({
                    type: "POST",
                    url: '/Admin/Articles/DownloadAudioV1?Ids=' + Ids,
                    headers: headers,
                    data: JSON.stringify(data.Data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resultV1 => {
                        $("#create_audio_" + Id).show();
                        $("#loading_audio_" + Id).hide();
                        if (data.Success == true) {
                            $("#SuccessModal").modal('show');
                            $(".modal-success-title").html(data.Msg);
                            setTimeout(function () { window.location.reload(); }, 2000);
                        } else {
                            setTimeout(function () {
                                var abc1 = DownloadAudioV1();
                            }, 4000);
                        }
                    },
                    error: function () {
                        $("#create_audio_" + Id).show();
                        $("#loading_audio_" + Id).hide();
                    }
                });
            }, 4000);
        }

    });
    /*
  var result2 = result.then(function (data) {
      console.log(data);
    
      $.ajax({
          type: "POST",
          url: '/Admin/Articles/DownloadAudioV1?Ids=' + Ids,
          headers: headers,
          data: JSON.stringify(data.Data),
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: resultV1 => {
              $("#create_audio_" + Id).show();
              $("#loading_audio_" + Id).hide();
              if (data.Success == true) {
                  $("#SuccessModal").modal('show');
                  $(".modal-success-title").html(data.Msg);
                  setTimeout(function () { window.location.reload(); }, 2000);
              } else {
                  $(".modal-error-title").html(data.Msg);
                  $("#ErrorModal").modal('show');
              }
          },
          error: function () {
              $("#create_audio_" + Id).show();
              $("#loading_audio_" + Id).hide();
          }
      });
  
  });*/
}

async function DownloadAudioV1(data) {
    console.log("Donwload Audio 2");
    return $.ajax({
        type: "POST",
        url: '/Admin/Articles/DownloadAudioV1?Ids=' + Ids,
        headers: headers,
        data: JSON.stringify(data.Data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: resultV1 => {
            $("#create_audio_" + Id).show();
            $("#loading_audio_" + Id).hide();
            if (data.Success == true) {
                $("#SuccessModal").modal('show');
                $(".modal-success-title").html(data.Msg);
                setTimeout(function () { window.location.reload(); }, 2000);
            } else {
                $(".modal-error-title").html(data.Msg);
                $("#ErrorModal").modal('show');
            }
        },
        error: function () {
            $("#create_audio_" + Id).show();
            $("#loading_audio_" + Id).hide();
        }
    });
}


function UpdateTTLT(Id, IdRoot) {
    $('#GetComplete').modal('show');
    $.ajax({
        url: "/Admin/Articles/UpdatedBaiVietLT?Id=" + Id + "&IdRoot=" + IdRoot,
        method: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $('#trang-thai').html('<h3> ' + result + ' </h3>')
        }
    });
}

function GetItemDetail(Ids, IdGroup) {
    $('#DetailArticle').modal('show');
    $.ajax({
        url: "/Admin/Articles/GetItem?Id=" + Ids,
        method: 'GET',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            $("#article-title").html(result.Title);
            $("#article-content").html(result.FullText);
            var audio = '';
            if (result.FileItem != null && result.FileItem != '') {
                audio = `<div class="detail-audio-file" id="footer-detail-article-audio"><audio controls>
                                <source src="`+ result.FileItem + `" type="audio/mpeg">
                            </audio></div>`;
                
            }

            var str = audio+`<div class="d-flex justify-content-between"> <div class="mr-3" style="width:500px"> <input class="form-control" value="` + `/` + result.Alias + `-` + result.Id + `.html` + `" /> </div> <div class="">`;
            str = str + '<button type="button" class="btn btn-secondary mr-2" data-dismiss="modal"><i class="fas fa-times"></i> Đóng</button><a href="/Admin/Articles/SaveItem/' + result.Ids + '" class="btn btn-primary"><i class="fa fa-edit"></i> Sửa</a>';
            if (IdGroup == 2 || IdGroup == 1) {
                if (result.Status == true) {
                    str = str + ' <button onclick="UpdateStatus(\'Articles\',\'' + result.Ids + '\',false)" type="button" class="btn btn-danger">Hủy Duyệt Bài</button>';
                } else {
                    str = str + ' <button onclick="UpdateStatus(\'Articles\',\'' + result.Ids + '\',true)" type="button" class="btn btn-primary"><i class="fas fa-check-double"></i> Duyệt Bài</button>';
                }
            }

            str = str + `</div></div>`;
            $("#footer-detail-article").html(str);
        }
    });
}


