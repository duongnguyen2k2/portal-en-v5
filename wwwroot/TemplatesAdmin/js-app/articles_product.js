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
});

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
//$("#ExportExcel").click(function () {

//	$('.loading-title').html('');
//	let str_title = "Đang xuất dữ liệu báo cáo Từ ngày: <strong>" + $("#ShowShowStartDate").val() + "</strong> Tời ngày: <strong>" + $("#ShowShowEndDate").val() + "</strong>";
//	$('.loading-title').html(str_title);

//	var form = serializeToJson($("#SearchFrorm").serializeArray());
//	var token = jQuery('input[name="__RequestVerificationToken"]').val();
//	var url = "/Admin/Articles/TaoFileBaoCao";
//	var headers = {};
//	headers["RequestVerificationToken"] = token;

//	var result = $.ajax({
//		type: "POST",
//		url: url,
//		headers: headers,
//		data: JSON.stringify(form),
//		contentType: "application/json; charset=utf-8",
//		dataType: "json",
//		success: function (data) {
//			DownLoadFileByUrl(headers, data.Data, "danh-sach-bai-viet.xlsx");
//		},
//		error: function () {
//			$('#LoaddingModal .loadding-title').html("Lỗi khi tải file ...");
//			console.log(data.Msg);

//		}
//	});
//});

//function CreateAudio(Id, Ids) {
//	var form = $('#AjaxDeleteForm');
//	var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
//	var headers = {};
//	headers["RequestVerificationToken"] = token;
//	$("#create_audio_" + Id).hide();
//	$("#loading_audio_" + Id).show();


//	console.log("CreateAudio");

//	return $.ajax({
//		type: "POST",
//		url: '/Admin/Articles/CreateAudio?Ids=' + Ids,
//		headers: headers,
//		data: JSON.stringify({}),
//		contentType: "application/json; charset=utf-8",
//		dataType: "json",
//		success: data => {
//			$("#create_audio_" + Id).show();
//			$("#loading_audio_" + Id).hide();
//			if (data.Success == true) {
//				$("#SuccessModal").modal('show');
//				$(".modal-success-title").html(data.Msg);
//				setTimeout(function () { window.location.reload(); }, 2000);
//			} else {
//				$(".modal-error-title").html(data.Msg);
//				$("#ErrorModal").modal('show');
//			}
//		},
//		error: function () {
//			$("#create_audio_" + Id).show();
//			$("#loading_audio_" + Id).hide();
//		}
//	});
//}

function RedirectToAddArticle() {
	var params = {};
	var IdsProduct = "";
	//debugger;

	if (location.search) {
		var parts = location.search.substring(1).split('&');

		for (var i = 0; i < parts.length; i++) {
			var nv = parts[i].split('=');
			if (!nv[0]) continue;
			params[nv[0]] = nv[1] || true;
		}
	}
	IdsProduct = params.Ids;
	console.log("RedirectToAddArticle: " + IdsProduct);
	window.location = '/Admin/ArticlesProduct/SaveItem?IdsProduct=' + IdsProduct;
}

function GetItemDetail(Ids, IdGroup) {
	//console.log("GetItemDetail");
	$('#DetailArticle').modal('show');
	$.ajax({
		url: "/Admin/ArticlesProduct/GetItem?Id=" + Ids,
		method: 'GET',
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (result) {
			$("#article-title").html(result.Title);
			$("#article-content").html(result.FullText);

			var str = `<div class="d-flex justify-content-between"> <div class="mr-3" style="width:500px"> <input class="form-control" value="` + `/` + result.Alias + `-` + result.Id + `.html` + `" /> </div> <div class="">`;
			str = str + '<button type="button" class="btn btn-secondary mr-2" data-dismiss="modal"><i class="fas fa-times"></i> Đóng</button><a href="/Admin/ArticlesProduct/SaveItem/' + result.Ids + '" class="btn btn-primary"><i class="fa fa-edit"></i> Sửa</a>';
			if (IdGroup == 2 || IdGroup == 1) {
				if (result.Status == true) {
					str = str + ' <button onclick="UpdateStatus(\'ArticlesProduct\',\'' + result.Ids + '\',false)" type="button" class="btn btn-danger">Hủy Duyệt Bài</button>';
				} else {
					str = str + ' <button onclick="UpdateStatus(\'ArticlesProduct\',\'' + result.Ids + '\',true)" type="button" class="btn btn-primary"><i class="fas fa-check-double"></i> Duyệt Bài</button>';
				}
			}

			str = str + `</div></div>`;
			$("#footer-detail-article").html(str);
		}
	});
}