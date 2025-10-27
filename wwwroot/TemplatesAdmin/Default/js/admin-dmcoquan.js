$('select[name="CategoryId"]').change(function () {
    window.location = "/Admin/DMCoQuan/Index?CategoryId=" + $(this).val();
});
function ExportExcel() {

    this.flagLoadingExport = true;
    var token = jQuery('input[name="__RequestVerificationToken"]').val();
    var headers = {};
    headers["RequestVerificationToken"] = token;
    $.ajax({
        url: '/Admin/DMCoQuan/ExportExcel',
        type: 'POST',
        data: JSON.stringify(this.SearchItems),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: result => {
            if (result.Success) {
                DownLoadFileByUrl(headers, result.Data, "bao-cao-ket-qua.xlsx");
            }
            this.flagLoadingExport = false;
        }
    });
}

function showCreateUserModal(Ids, Title, FolderUpload) {
    $('#CreateUserModal').modal('show')
    console.log(Ids, Title, FolderUpload);
    $(".create-user-title").html('Bạn muốn tạo tài khoản quản trị cho cơ quan <strong>' + Title + "</strong> Vui lòng nhập tài khoản. Nếu để trống hệ thống sẽ lấy tài khoản mặc định là <strong>" + FolderUpload+"</strong>");
    jQuery("#CreateUserItemTrue").attr("data-ids", Ids);
    jQuery("#CreateUserItemTrue").attr("data-title", Title);
    jQuery("#CreateUserItemTrue").attr("data-folderupload", FolderUpload);

}

function CreateUserItem() {
    $("#CreateUserItemTrue").hide();
    $("#CreateUserItemLoading").show();

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
            $("#CreateUserItemTrue").show();
            $("#CreateUserItemLoading").hide();
        }
    });

}