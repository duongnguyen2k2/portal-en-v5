

$(".viewFile").click(function () {
    var url = $(this).attr("val");
    $("#myFrame").attr("src", url);
    $("#myFrame").show();
})

function getAudio(Id, Ids) {

    var token = jQuery('#FormArticleSmartVoid input[name="__RequestVerificationToken"]').val();
    console.log(token);
    var headers = {};
    headers["RequestVerificationToken"] = token;
    $("#btn-play").hide();
    $("#btn-play-loading").show();
    var result = $.ajax({
        type: "GET",
        headers: headers,
        url: "/Articles/GetAudio?Id=" + Id + "&Ids=" + Ids,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: res => {

            if (res.Code == "v2") {
                location.reload();
            }
        },
        error: function () {
            location.reload();
        }
    }).done(data => {
        console.log("done", data);

        if (data.Code == "v1" && data.Success == true) {
            setTimeout(function () {
                console.log("Donwload Audio 1");
                $.ajax({
                    type: "POST",
                    url: '/Articles/DownloadAudioV1?Ids=' + Ids,
                    headers: headers,
                    data: JSON.stringify(data.Data),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resultV1 => {
                        location.reload();
                    },
                    error: function () {
                        location.reload();
                    }
                });
            }, 4000);
        }

    });;
}


async function DownloadAudioV1(data) {
    console.log("Donwload Audio 2");
    return $.ajax({
        type: "POST",
        url: '/Articles/DownloadAudioV1?Ids=' + Ids,
        headers: headers,
        data: JSON.stringify(data.Data),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: resultV1 => {

            location.reload();
        },
        error: function () {
            location.reload();
        }
    });
}

function LikeArticle(Id, Ids) {

    var token = jQuery('#FormArticleLike input[name="__RequestVerificationToken"]').val();
    console.log(token);
    var headers = {};
    headers["RequestVerificationToken"] = token;
   
    return $.ajax({
        type: "GET",
        headers: headers,
        url: "/Articles/LikeArticle?Id="+Id+"&Ids="+Ids,        
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: res=> {
            if (res.Success) {
                var ArticleId = $("#ArticleId").val();
                localStorage.setItem(res.Data + "_code_" + ArticleId, "1");
                $("#article-like").hide();
                $("#article-unlike").show();
                $('#LikeArticleModal').modal('show');
            } else {

            }

        },
        error: function () {

        }
    });
}

/*
$('#PageDetail img').not('.notshowfancybox').each(function () {
    var src = $(this).attr('src');
    $(this).after('<a class="fancybox" data-fancybox="group" href="' + src + '"><img src="' + src + '"></a>');
    $(this).hide();
})

$("[data-fancybox]").fancybox({
   
});*/

// Disable copy content, prevent blacked out text, prevent "I" key, "J" key, "S" key + macOS, "U" key, "F12" key
jQuery(document).ready(function () {
    
    var ArticleId = $("#ArticleId").val();
    console.log("ArticleId:"+ArticleId);
    if (ArticleId != undefined && ArticleId != "" & ArticleId > 0) {
        var CodeLike = localStorage.getItem(window.Site.CODE + "_code_" + ArticleId);
        if (CodeLike == "1") {
            $("#article-like").hide();
            $("#article-unlike").show();
        } else {
            $("#article-like").show();
            $("#article-unlike").hide();
        }
    } else {
        $("#article-unlike").hide();
        $("#article-like").hide();
    }
    
	/*
    jQuery(function () {
        jQuery(this).bind("contextmenu", function (event) {
            event.preventDefault();
            //alert('Right click disable in this site! Chuột phải đã được vô hiệu hóa trên site này !')
        });
    });
    (function () {
        'use strict';
        let style = document.createElement('style');
        style.innerHTML = '*{ user-select: none !important; }';

        document.body.appendChild(style);
    })();
    window.onload = function () {
        document.addEventListener("contextmenu", function (e) {
            e.preventDefault();
        }, false);
        document.addEventListener("keydown", function (e) {

            if (e.ctrlKey && e.shiftKey && e.keyCode == 73) {
                disabledEvent(e);
            }

            if (e.ctrlKey && e.shiftKey && e.keyCode == 74) {
                disabledEvent(e);
            }

            if (e.keyCode == 83 && (navigator.platform.match("Mac") ? e.metaKey : e.ctrlKey)) {
                disabledEvent(e);
            }

            if (e.ctrlKey && e.keyCode == 85) {
                disabledEvent(e);
            }

            if (event.keyCode == 123) {
                disabledEvent(e);
            }
        }, false);
        function disabledEvent(e) {
            if (e.stopPropagation) {
                e.stopPropagation();
            } else if (window.event) {
                window.event.cancelBubble = true;
            }
            e.preventDefault();
            return false;

        }
    }
	*/
});


function PrintArticle() {
    //shop-detail-container
    var divName = "PageDetail";
    var printContents = document.getElementById(divName).innerHTML;
    var originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
}




var vmArticleComment = new Vue({
    el: '#section-comment',
    data: {
        title: "Bình luận bài viết",
        SearchItems: { CurrentPage: 1, ItemsPerPage: 50, Keyword: '', msg: '', ArticleId: 1, ArticleIds: "", Description:"" },
        msgInfo: "",
        ListItems: [],
        detailItem: { FullName: "", FullName: '', Ids: '', Id: 0, ArticleId: 0, ArticleIds: "", Captcha:""},                
        flagLoading: false,
        flagLoadingChild: false,        
        flagLoadingCaptcha: false,        
        pageCount: 0,
        TotalRows: 0,
        now: new Date(),
        ListSkeleton: [1, 2, 3, 4, 5, 6],
        msgInfo: '',
        urlCaptcha: '/images/captcha_none.webp'
    },
    created() {
        $(".app-vue").show();
        this.SearchItems.ArticleId = $("#ArticleId").val();
        this.SearchItems.ArticleIds = $("#ArticleIds").val();
        this.detailItem.ArticleIds = $("#ArticleIds").val();
        this.detailItem.ArticleId = $("#ArticleId").val();
        this.ResetCaptcha();
    },
    mounted() {

        this.getListItems();
    },
    methods: {


        showAdd: function (i) {
            msgInfo = "";
            this.detailItem = this.ListItems[i];
            this.flagLoadingChild = false;
            $("#DeletedModal").modal("show");

        },

        ResetCaptcha: function () {
            msgInfo = "";
            var token = jQuery('#comment_post_form input[name="__RequestVerificationToken"]').val();
            var headers = {};
            headers["RequestVerificationToken"] = token;
            this.flagLoadingCaptcha = true;
            return $.ajax({
                type: "GET",
                url: '/Captcha/Index/?u=ArticlesComment',
                headers: headers,                
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: result => {
                    if (result.Success) {
                        this.flagLoadingCaptcha = false;
                        this.urlCaptcha = result.Data;
                    } else {

                    }
                    
                },
                error: err => {
                    this.flagLoadingCaptcha = false;
                    this.getListItems();
                }
            });

        },
        SendComment: function () {
           

            if (this.detailItem.Description == null || this.detailItem.Description == "") {
                alert("Vui lòng nhập bình luận bài viết");
            } else if (this.detailItem.Captcha == null || this.detailItem.Captcha == "") {
                alert("Vui lòng nhập mã captcha để gửi bình luận");
            } else {
                msgInfo = "";
                var token = jQuery('#comment_post_form input[name="__RequestVerificationToken"]').val();
                var headers = {};
                headers["RequestVerificationToken"] = token;
                this.flagLoadingChild = true;

                return $.ajax({
                    type: "POST",
                    url: '/Articles/SendComment/' + this.detailItem.Ids,
                    headers: headers,
                    data: JSON.stringify(this.detailItem),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result => {
                        this.flagLoadingChild = false;  
                        $("#comment-article-content").html(result.Msg);
                        if (result.Success) {
                            this.getListItems();
                            $("#comment-title-modal").html("Bình luận thành công");
                            $('#txtComment').prop('readonly', true);
                            this.detailItem.Description = "";
                            this.detailItem.Captcha = "";                            
                            $('.block_btn_send').hide();

                        } else {
                            msgInfo = result.Msg;
                            $("#comment-title-modal").html("Lỗi khi nhập bình luận");
                            this.ResetCaptcha();
                            $('#CommentArticleModal').modal('show');
                        }
                        
                    },
                    error: err => {
                        $("#comment-title-modal").html("Lỗi khi nhập bình luận");
                        $("#comment-article-content").html(result.Msg);
                        this.flagLoadingChild = false;
                        this.getListItems();
                    }
                });
            }
        },
        getListItems: function () {
            this.ListItems = [];
            var token = jQuery('#comment_post_form input[name="__RequestVerificationToken"]').val();
            var headers = {};
            headers["RequestVerificationToken"] = token;

            this.flagLoading = true;
            $.ajax({
                url: '/Articles/GetListComment',
                type: 'POST',
                headers: headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: result => {
                    if (result.Success) {
                        this.ListItems = result.Data;
                        if (this.ListItems.length > 0) {
                            this.TotalRows = (this.ListItems[0]).TotalRows;
                            this.pageCount = Math.ceil(this.TotalRows / this.SearchItems.ItemsPerPage);
                        } else {
                            this.TotalRows = 0;
                            this.pageCount = 0;
                        }
                        console.log(this.ListItems);
                    } else {
                        this.TotalRows = 0;
                        this.pageCount = 0;
                    }
                    this.flagLoading = false;


                }
            });

        },

        pageChanged() {
            this.getListItems();
        },
        SearchChange() {

            this.SearchItems.CurrentPage = 1;
            this.getListItems();
        },

        ChangeKeyword(e) {
            console.log(e);
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                this.SearchChange();
            }
        },
    }
});
Vue.component('paginate', VuejsPaginate);
