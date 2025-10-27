var vmAppUSERS= new Vue({
    el: '#app_users',
    data: {
        title: "Tìm kiếm giáo viên",
        SearchItems: { CurrentPage: 0, ItemsPerPage: 10, IdCoQuan: 0, Keyword: '', IdLevel:3},
        ListItems: [],
        ListDMCoQuan: [],
        detailItem: {},
        ListStatus: [],
        msgInfo: "",
        pageCount: 0,
        totalItems: 0,
        flagLoading: false,
        flagLoadingChild: false,
        baseurl: '/',
        headers: [],
        ListDMCap: [
            {Id:2, Title: 'Mẫu Giáo' },
            {Id:3,Title:'Cấp 1'},
            {Id:4,Title:'Cấp II'},
            {Id:5,Title:'Cấp III'},
        ]
    },
    created() {

        $(".app-vue").show();
        //this.getListItems();
        this.getListHome();

    },
    methods: {
       
        getListHome: function () {
            this.ListItems = [];
            this.flagLoading = true;
            $.ajax({
                type: "POST",
                url: this.baseurl + "users/GetListHome",
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (resul) {
                    vmAppUSERS.flagLoading = false;
                    if (resul.Success) {
                        vmAppUSERS.ListItems = resul.Data.ListItems;
                        vmAppUSERS.ListDMCoQuan = resul.Data.ListDMCoQuan;
                    }
                    vmAppUSERS.flagLoading = false;
                },
                error: function () {

                }
            });
        },

        getListItems: function () {

            this.ListItems = [];
            this.flagLoading = true;
            this.ListLinhVuc = [];
            $.ajax({
                type: "POST",
                url: this.baseurl + "users/GetListItems",
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (resul) {
                    vmAppUSERS.flagLoading = false;
                    if (resul.Success) {
                        vmAppUSERS.ListItems = resul.Data.ListItems;
                    }
                },
                error: function () {

                }
            });

        },

        pageChanged: function () {
            this.getListItems();
        },
        ChangeKeyWord: function (e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            if (code == 13) {
                this.SearchChange();
            }
        },
        SearchChange: function () {
            this.SearchItems.current_page = 1;
            this.getListItems();
        },

    }
});


