

var vmArticles = new Vue({
    //fyjhngvhnvnvn
    el: '#app-articles',
    data: {
        title: "Quản lý tour",
        detailItem: { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0, FullText: "" },
        deleteItem: { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0 },
        SearchItems: { CurrentPage: 1, ItemsPerPage: 30, Keyword: '', TotalItems: 0, PageCount: 0, Status: -1, CatId: -1, IdCoQuan: -1 },
        flagLoadingCat: false,
        flagLoadingCoQuan: false,
        flagLoading: false,
        flagLoadingChild: false,
        ListErr: [],
        ListItems: [],
        msgInfo: '',
        msgErrChild: '',
        detailModal: null,
        deleteModal: null,
        itemDetailsModal: null,
        validDetailItem: { Title: "", Sku: "", Ordering: "", TimeDay: "", PriceDeal: "", Price: "" },
        headers: [],
        ListSkeleton: [1, 2, 3, 4],
        IdGroupUser: 0,
        ListArticleCat: [],
        ListCoQuan: [],
        key: '123',
    },

    mounted() {
        document.getElementById("app-articles").style.display = "block";
        var token = jQuery('input[name="__RequestVerificationToken"]').val();
        this.headers["RequestVerificationToken"] = token;

        /*this.deleteModal = new bootstrap.Modal(document.getElementById('delete-modal'), {});
        this.IdGroupUser = $("#User_IdGroup").val();*/
        this.itemDetailsModal = new bootstrap.Modal(document.getElementById('item-details-modal'), {});

        this.getListArticleCat();
        this.getListCoQuan();
        this.getListJson();
    },
    methods: {

        closeItemDetailsModal: function () {
            this.itemDetailsModal.hide();
        },

        showAdd: function () {
            this.resetFormDetail();
            this.detailModal.show();
            $("#DetailItem_Title").focus();
        },

        getListJson: function () {
            this.flagLoading = true;
            console.log("Search Item: ", this.SearchItems)
            $.ajax({
                type: "POST",
                url: '/ApiArticlesTransfer/GetByCat',
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoading = false;
                    if (resul.Success) {

                        this.ListItems = resul.Data;
                        if (this.ListItems != null && this.ListItems.length > 0) {
                            this.SearchItems.TotalItems = (this.ListItems[0]).TotalRows;
                            this.SearchItems.PageCount = Math.ceil(this.SearchItems.TotalItems / this.SearchItems.ItemsPerPage);
                        } else {
                            this.SearchItems.TotalItems = 0;
                            this.SearchItems.PageCount = 0;
                        }

                    } else {
                        this.SearchItems.TotalItems = 0;
                        this.SearchItems.PageCount = 0;
                    }
                    console.log(this.SearchItems);
                },
                error: function () {

                }
            });
        },

        getListArticleCat: function () {
            this.flagLoadingCat = true;
            $.ajax({
                type: "GET",
                url: '/ApiArticlesTransfer/GetListCategoriesArticles',
                headers: this.headers,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingCat = false;
                    if (resul.Success) {
                        this.ListArticleCat = resul.Data;
                    }
                },
                error: function () {

                }
            });
        },

        getListCoQuan: function () {
            this.flagLoadingCoQuan = true;
            $.ajax({
                type: "GET",
                url: '/ApiArticlesTransfer/GetListDMCoQuan',
                headers: this.headers,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingCoQuan = false;
                    if (resul.Success) {
                        this.ListCoQuan = resul.Data;
                    }
                },
                error: function () {

                }
            });
        },

        ChangeKeyWord: function (event) {
            console.log(event);
            if (event.which == 13) {
                this.SearchChange();
            }
        },

        SearchChange() {
            this.getListJson();
            console.log("search change!");
        },

        openItemDetails: function (id) {
            this.flagLoadingChild = true;
            return $.ajax({
                type: "GET",
                url: '/ApiArticlesTransfer/GetItem?id=' + id,
                headers: this.headers,
                /*data: JSON.stringify(this.detailItem),*/
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    //this.getListJson();
                    this.flagLoadingChild = false;

                    if (!resul.Success) {
                        $.toast({
                            heading: resul.Msg,
                            icon: 'error',
                        });
                    } else {
                        console.log("before assgin: ", this.detailItem);
                        this.detailItem = resul.Data;
                        $('#article-detail').html(resul.Data.FullText);
                        console.log("affter assign: ", this.detailItem);
                        console.log(typeof resul.Data);
                        $.toast({
                            heading: `Mở <strong>` + this.detailItem.Title + `</strong> thành công`,
                            icon: 'success',
                            stack: false
                        });
                        this.itemDetailsModal.show();
                    }

                },
                error: function () {
                    this.flagLoadingChild = false;
                    $.toast({
                        heading: `Lỗi khi mở dữ liệu`,
                        icon: 'error',
                        stack: false
                    });

                }
            });

        },

        saveItem: function () {
            this.flagLoadingChild = true;
            console.log(this.detailItem)
            return $.ajax({
                type: "POST",
                url: '/ApiArticlesTransfer/StoreItem',
                headers: this.headers,
                data: JSON.stringify(this.detailItem),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingChild = false;
                    if (resul.Success) {
                        //this.detailModal.hide();
                        $.toast({
                            heading: resul.Msg,
                            icon: 'success',
                            stack: false
                        });
                        this.itemDetailsModal.hide();
                        this.getListJson();
                    } else {
                        $.toast({
                            heading: resul.Msg,
                            icon: 'error',
                        });
                    }
                },
                error: function () {


                }
            });
        },
    
        pageChanged() {
            this.getListJson();
        },
    }
});

Vue.component('paginate', VuejsPaginate);


