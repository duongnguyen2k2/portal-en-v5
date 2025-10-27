/*
function AddFile() {
    console.log(ListLinkArticle);
    var Item = { Title: '', Link: '', Status: true };
    if (ListLinkArticle == null) {
        ListLinkArticle = [];
    }
    ListLinkArticle.push(Item);
    BuildHtmlLinkArticle();

}
function DeleteFile(i) {
    ListLinkArticle[i].Status = false;
    BuildHtmlLinkArticle();
}
function BuildHtmlFile() {
    if (ListLinkArticle != null && ListLinkArticle.length > 0) {
        $(".link-article-list").html('');

        for (var i = 0; i < ListLinkArticle.length; i++) {
            if (ListLinkArticle[i].Status) {
                str = '<div class="link-article-item"><div class="row">';
                str = str + '<div class="col-lg-3"><input onchange="UpdateDataLinkArticle(' + i + ',1,event)" name="ListLinkArticle[' + i + '].Title" class="form-control" placeholder="Tiêu Đề" value="' + ListLinkArticle[i].Title + '" /></div>';
                str = str + '<div class="col-lg-3"><input onchange="UpdateDataLinkArticle(' + i + ',2,event)" name="ListLinkArticle[' + i + '].Link"  class="form-control" placeholder="Link" value="' + ListLinkArticle[i].Link + '"/></div>';
                str = str + '<div class="col-lg-2"><span class="btn btn-danger" onclick="DeleteLinkArticle(' + i + ')">Xóa</span></div>';
                str = str + '<input type="hidden" name="ListLinkArticle[' + i + '].Status" value="' + ListLinkArticle[i].Status + '"/></div></div>';
                $(".link-article-list").append(str);
            }
        }
    }
}*/
function ThemFile() {
    var max = parseInt($("#maxfile").val()) + 1;
    var htmlFile = `<div class="file-item">
            <input type = "hidden" name = "ListFile[`+ max + `].FilePath" class="FileName" id = "FileName_` + max + `" >
            <input type = "text" name = "ListFile[`+ max + `].Title" class="TitleDocumentName" id = "Title_` + max + `" >
            <a href="javascript:openCustomRoxy2(`+ max + `)" type="button" class="btn btn-primary">Chọn File</a>
            <span class="lblFileName" id="lblFileName_`+ max + `"></span>
            <span class="text-danger xoafile">Xóa</span>
        </div>`;

    $(".file-list").append(htmlFile);

    $("#maxfile").val(max);
}