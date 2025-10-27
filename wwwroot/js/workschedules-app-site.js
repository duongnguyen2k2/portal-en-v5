

var vmWorkSchedules  = new Vue({
    el: '#app-workschedules',
    data: {
        title: "Quản lý Lịch công tác",
        detailItem: { Id: 0, Ids: '', Title: '', IntroText: '', FullText: '', ShowWeek: 0, ShowYear: 0, PublishUp: 0, PublishUpShow:0, Status: 1, Ordering:1 },        
        SearchItems: { CurrentPage: 1, ItemsPerPage: 20, Keyword: '', TotalItems: 0, PageCount: 0, Status: -1, ShowYear: 0, ShowWeek:0},
        flagLoading: false,
        flagLoadingChild: false,
        headers: {},
        ListErr: [],
        ListItems: [],
        msgInfo: '',
        msgErrChild: '',
        headers: [],
        ListYear: [],
        ListWeek: [],
        now: new Date(),
    },
   
    mounted() {
        document.getElementById("app-workschedules").style.display = "block";
        var token = jQuery('input[name="__RequestVerificationToken"]').val();
        this.headers["RequestVerificationToken"] = token;
        this.SearchItems.ShowYear = this.now.getFullYear();        
        this.getListWeek();
        this.getItem();
        for (let i = 2019; i <= this.now.getFullYear(); i++) {
            this.ListYear.push(i);
        }
    },
    methods: {
                
        getItem: function () {
            this.flagLoadingChild = true;
            $.ajax({
                type: "POST",
                url: '/WorkSchedules/GetItem',
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingChild = false;
                    if (resul.Success) {
                        if (resul.Data != null) {
                            this.detailItem = resul.Data;
                        }
                        
                    }
                },
                error: function () {

                }
            });
        },
        getListWeek: function () {
            this.flagLoading = true;
            $.ajax({
                type: "POST",
                url: '/WorkSchedules/GetListWeek',
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoading = false;
                    if (resul.Success) {
                        this.ListWeek = resul.Data.ListWeek;
                        this.SearchItems.ShowWeek = resul.Data.WeekOfYear;                
                    }
                },
                error: function () {

                }
            });
        },
        
        SearchChangeYear() {            
            this.getListWeek();
        },
       
        SearchChange() {
            this.getItem();
        },
    }
});
