var vmBackendComment = new Vue({
    el: '#app_backend_orders',
    data: {
        title: "Bình luận bài viết",
        SearchItems: { CurrentPage: 1, ItemsPerPage: 50, Keyword: '', msg: '', ArticleId: 1, ArticleIds: ""},
        msgInfo: "",
        ListItems: [],     
        detailItem: { FullName: "", FullName: '', Ids: '', Id: 0 },
        deleteItem: { FullName: "", FullName: '', Ids: '', Id: 0 },
        flag: false,
        flagLoading: false,
        flagLoadingChild: false,
        flagExportExcel: false,
        pageCount: 0,
        TotalRows: 0,        
        now: new Date(),
        ListSkeleton: [1, 2, 3, 4, 5, 6],        
        msgInfo:''
    },
    created() {
        $(".app-vue").show();
        this.SearchItems.ArticleId = $("#ArticleId").val();
        this.SearchItems.ArticleIds = $("#ArticleIds").val();
    },
    mounted() {
        
        this.getListItems();
    },
    methods: {
        deletedItem: function () {
            var token = jQuery('input[name="__RequestVerificationToken"]').val();
            var headers = {};
            headers["RequestVerificationToken"] = token;

            this.flagLoadingChild = true;
            return $.ajax({
                type: "POST",
                url: '/Admin/Articles/DeletedComment',
                headers: headers,
                data: JSON.stringify(this.deleteItem),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.getListItems();
                    this.flagLoadingChild = false;
                    $("#DeletedCommentModal").modal("hide");

                    if (!resul.Success) {
                        $.toast({
                            heading: resul.Msg,
                            icon: 'error',
                        });
                    } else {

                        $.toast({
                            heading: resul.Msg,
                            icon: 'success',
                            stack: false
                        });
                    }
                },
                error: function () {
                    vmBackendComment.flagLoadingChild = false;
                    vmBackendComment.getListItems();
                }
            });
        },

        SaveItem: function () {
            var token = jQuery('input[name="__RequestVerificationToken"]').val();
            var headers = {};
            headers["RequestVerificationToken"] = token;
            
            this.flagLoadingChild = true;
            return $.ajax({
                type: "POST",
                url: '/Admin/Articles/EditComment',
                headers: headers,
                data: JSON.stringify(this.detailItem),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul=> {
                    this.getListItems();
                    this.flagLoadingChild = false;
                    $("#EditModal").modal("hide");

                    if (!resul.Success) {
                        $.toast({
                            heading: resul.Msg,
                            icon: 'error',
                        });
                    } else {
                        
                        $.toast({
                            heading: resul.Msg,
                            icon: 'success',
                            stack: false
                        });
                    }
                },
                error: function () {
                    vmBackendComment.flagLoadingChild = false;
                    vmBackendComment.getListItems();
                }
            });
        },
        

        showRemoveStatus(i) {
            this.detailItem = this.ListItems[i];
            console.log(this.detailItem);

            if (this.IdGroupUser == 1 && (this.detailItem.StatusId == 9 || this.detailItem.StatusId == 98  || this.detailItem.StatusId == 99 || this.detailItem.StatusId == 100 || this.detailItem.StatusId == 111)) {
                $("#RemoveStatusModal").modal("show");
            } else if (this.IdGroupUser == 2 && (this.detailItem.StatusId == 98 || this.detailItem.StatusId == 99 || this.detailItem.StatusId == 100)) {
                $("#RemoveStatusModal").modal("show");
            }

        },
        ShowEdit: function (i) {
            msgInfo = "";
            this.detailItem = Object.assign({}, this.ListItems[i]);
            this.flagLoadingChild = false;
            $("#EditModal").modal("show");

        },
       
        showDeleted: function (i) {
            msgInfo = "";
            this.deleteItem = this.ListItems[i];
            this.flagLoadingChild = false;
            $("#DeletedCommentModal").modal("show");
        },
        
        UpdateStatus: function (i) {
            msgInfo = "";
            var token = jQuery('input[name="__RequestVerificationToken"]').val();
            var headers = {};
            headers["RequestVerificationToken"] = token;
            this.flagLoadingChild = true;
            this.detailItem = this.ListItems[i];
            return $.ajax({
                type: "POST",
                url: '/Admin/Articles/UpdateStatusComment/' + this.detailItem.Ids,
                headers: headers,
                data: JSON.stringify(this.detailItem),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: result=> {
                    if (result.Success) {
                        this.getListItems();
                        this.flagLoadingChild = false;
                        $(".loading-title").html(result.Msg);
                        $("#LoaddingModal").modal("show");
                    } else {
                        msgInfo = result.Msg;
                    }
                },
                error: err=> {
                    this.flagLoadingChild = false;
                    this.getListItems();
                }
            });

        },
        getListItems: function () {
            this.ListItems = [];
            this.flagLoading = true;
            $.ajax({
                url: '/Admin/Articles/GetListComment',
                type: 'POST',
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: result=> {
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
