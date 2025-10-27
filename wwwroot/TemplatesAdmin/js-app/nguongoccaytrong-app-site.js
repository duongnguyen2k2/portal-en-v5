

var vmNguonGocCayTrong = new Vue({
    el: '#app-nguongoccaytrong',
    data: {
        title: "Tìm kiếm Nguồn gốc cây trồng",
       
        SearchItems: { CurrentPage: 1, ItemsPerPage: 10, IdPhuongXa: 0, IdQuanHuyen:0, IdTinhThanh:42, Keyword: '', TotalItems: 0, PageCount:0 ,Status:-1},
        flagLoading: false,
        flagLoadingChild: false,
        headers: {},
        ListErr: [],
        ListItems: [],
        ListDMTinhThanh: [],
        ListDMQuanHuyen: [],
        ListDMPhuongXa: [],
        ListSkeleton: [1,2,3,4,5,6],
        msgInfo: '',
        msgErrChild: '',
         
        validDetailItem: { Title: "", FullText: "", Ordering:"" },
        headers: [],
    },
   
    mounted() {
        document.getElementById("app-nguongoccaytrong").style.display = "block";
        var token = jQuery('input[name="__RequestVerificationToken"]').val();
        this.headers["RequestVerificationToken"] = token;

        this.GetListQuanHuyen();
        //this.getListJson();
     
    },
    methods: {
        
        
        getListJson: function () {
            this.flagLoading = true;
            $.ajax({
                type: "POST",
                url: '/NguonGocCayTrong/GetListJson',
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul=> {
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

        GetListQuanHuyen() {
            $.ajax({
                type: "GET",
                url: '/Home/GetListJsonDMQuanHuyen?IdTinhThanh=' + this.SearchItems.IdTinhThanh,
                headers: this.headers,
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoading = false;
                    if (resul.Success) {

                        this.ListDMQuanHuyen = resul.Data;

                    } else {

                    }

                },
                error: function () {

                }
            });
        },
             
        GetListDMPhuongXa() {
            $.ajax({
                type: "GET",
                url: '/Home/GetListJsonDMPhuongXa?IdQuanHuyen=' + this.SearchItems.IdQuanHuyen,
                headers: this.headers,
                data: JSON.stringify({}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoading = false;
                    if (resul.Success) {

                        this.ListDMPhuongXa = resul.Data;
                       
                    } else {
                       
                    }
                    
                },
                error: function () {

                }
            });
        },
        pageChanged() {
            this.getListJson();
        },

        ChangeKeyWord: function (event) {
            console.log(event);
            if (event.which == 13) {
                this.SearchChange();
            }
        },
        SearchChange() {
            this.getListJson();
        },
    }
});

Vue.component('paginate', VuejsPaginate);


