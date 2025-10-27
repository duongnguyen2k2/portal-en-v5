var url = "/Admin/USGroups/";
var IdNhomQuyen = 0;
$(function () {
	load_dsChucNang(IdNhomQuyen);
	$('#myTable').on('click', 'tbody tr', function (event) {
		$(this).addClass('table-primary').siblings().removeClass('table-primary');
		$("#idmsg").text("").removeClass("alert alert-dismissible fade show alert-success");
		IdNhomQuyen = $(this)[0].id;
		load_dsChucNang(IdNhomQuyen);
	});
});

function load_dsChucNang(Id) {
	return $.ajax({
		type: "POST",
		url: url + 'JsonMenus',
		data: JSON.stringify({
			Id: Id
		}),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {
			$("#dsChucNang tbody").empty();
			$.each(msg, function (i, jsondata) {
				console.log(jsondata.Icon);
				var row = $("<tr/>")
					.append($("<td/>").text(i + 1).addClass("text-center"))
					.append($("<td/>").append($("<i>").addClass(jsondata.Icon)).append("&nbsp;&nbsp;&nbsp;").append($("<a/>").text(jsondata.TreeMenu)))
					.append($("<td/>").addClass("text-center").append($('<input type="checkbox" id=' + jsondata.IdParent + ' value=' + jsondata.Id + ' />').attr("checked", jsondata.Status).change(chonChucNang)));
				$("#dsChucNang tbody").append(row);
			});
		}
	});
}
let dschon = "";
function capnhat_quyen() {
	var arrList = [];
	dschon = "";
	$('#dsChucNang tr').each(function (i, row) {
		var $actualrow = $(row);
		var $chkbox = $(this).find('input[type="checkbox"]');
		if ($chkbox.length && $chkbox.prop('checked')) {
			dschon += + ",";
			arrList.push($chkbox.val());
		}
	});
	dschon = arrList.toString();
	console.log(dschon);

	return $.ajax({
		type: "POST",
		url: url + 'JsonSaveMenus',
		data: JSON.stringify({
			Id: IdNhomQuyen,
			ListMenuId: dschon
		}),
		contentType: "application/json; charset=utf-8",
		dataType: "json",
		success: function (msg) {
			if (msg[0].N > 0) {
				$("#idmsg").text('Cập nhật thành công.').addClass("alert alert-dismissible fade show alert-success");
				setTimeout(() => $("#idmsg").text("").removeClass("alert alert-dismissible fade show alert-success"), 3000);
			} else {
				$("#idmsg").text('Vui lòng chọn nhóm quyền.').addClass("alert alert-dismissible fade show alert-danger");
			}
		}
	});
}
function chonChucNang() {
	var idChucNang = this.value;
	var idCha = this.id;
	checkAllCon(idChucNang, idCha, this.checked);
}
function checkAllCon(idChucNang, idCha, event) {
	var flagCha = false;
	$('#dsChucNang tr').each(function (i, row) {
		var chkbox = $(this).find('input[type="checkbox"]');
		if (!event) {
			flagCha = checkedCon(idCha);
		}
		console.log(flagCha);
		if (chkbox.length) {
			if (chkbox.val() == idCha || chkbox[0].id == idChucNang) {
				chkbox[0].checked = event;
			}
			if (flagCha && chkbox.val() == idCha) {
				chkbox[0].checked = true;
				flagCha = false;
			}
		}
	});
}
function checkedCon(idCha) {
	var flag = false;
	$('#dsChucNang tr').each(function (i, row) {
		var chkbox = $(this).find('input[type="checkbox"]');
		if (chkbox.length && chkbox.prop('checked')) {
			if (chkbox[0].id == idCha) {
				flag = true;
				return;
			}
		}
	});
	return flag;
}