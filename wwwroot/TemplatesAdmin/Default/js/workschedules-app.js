

var vmWorkSchedules  = new Vue({
    el: '#app-workschedules',
    data: {
        title: "Quản lý Lịch công tác",
        detailItem: { Id: 0, Ids: '', Title: '', IntroText: '', FullText: '', ShowWeek: 0, ShowYear: 0, PublishUp: 0, PublishUpShow:0, Status: 1, Ordering:1 },
        deleteItem: { Id: 0, Ids: '', Title: '',  IntroText: '', FullText: '', ShowWeek: 0, ShowYear: 0, PublishUp: 0, PublishUpShow:0, Status: 1, Ordering:1 },
        SearchItems: { CurrentPage: 1, ItemsPerPage: 20, Keyword: '', TotalItems: 0, PageCount: 0, Status: -1, ShowYear: 0, ShowWeek:0},
        flagLoading: false,
        flagLoadingChild: false,
        headers: {},
        ListErr: [],
        ListItems: [],
        msgInfo: '',
        msgErrChild: '',         
        validDetailItem: { Title: "", Code: "", Ordering:"" },
        headers: [],
        ListSkeleton: [1, 2, 3, 4,5,6,7,8,9,10],
        ListYear: [],
        ListWeek: [],
        now: new Date(),
    },
   
    mounted() {
        document.getElementById("app-workschedules").style.display = "block";
        var token = jQuery('input[name="__RequestVerificationToken"]').val();
        this.headers["RequestVerificationToken"] = token;
        this.SearchItems.ShowYear = this.now.getFullYear();
        this.getListJson();
        this.getListWeek();
        for (let i = 2019; i <= this.now.getFullYear()+1; i++) {
            this.ListYear.push(i);
        }
    },
    methods: {
        
        checkForm: function (count) {
            this.resetFormDetailValid();
           

            return true;
            
        },
        showAdd: function () {                     
            this.resetFormDetail();
            $('#detail-modal').modal('show');
            $("#DetailItem_Title").focus();
            tinymce.get("full-textarea").setContent("");
            //tinymce.activeEditor.setContent("");
            
            
        },
        changeStatus: function (i) {
            
            var Status = false;
            if (this.ListItems[i].Status == false) {
                Status = true;
            }

            return $.ajax({
                type: "GET",
                url: '/Admin/workschedules/UpdateStatus?Ids=' + this.ListItems[i].Ids + "&Status=" + Status,
                headers: this.headers,                
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    
                    if (!resul.Success) {                        
                        $.toast({
                            heading: resul.Msg,
                            icon: 'error',                            
                        });
                    } else {
                        this.ListItems[i].Status = Status;                       
                        $.toast({
                            heading: `Cập nhật trạng thái <strong>` + this.ListItems[i].Title + `</strong> thành công`,                            
                            icon: 'success',                            
                            stack: false                            
                        });                      
                    }
                },
                error: function () {


                }
            });

        },

        deletedItem: function () {            
            return $.ajax({
                type: "POST",
                url: '/Admin/workschedules/DeleteItem',
                headers: this.headers,
                data: JSON.stringify(this.deleteItem),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.getListJson();                    
                    $('#delete-modal').modal('hide');
                    if (!resul.Success) {
                        $.toast({
                            heading: resul.Msg,
                            icon: 'error',
                        });
                    } else {                        
                        $.toast({
                            heading: `Xóa <strong>` + this.deleteItem.Title + `</strong> thành công`,
                            icon: 'success',
                            stack: false
                        });
                    }
                },
                error: function () {


                }
            });

        },

        saveItem: function () {
            this.detailItem.FullText = tinymce.get("full-textarea").getContent();
            if (this.checkForm()) {
                this.detailItem.Status = true;
                this.flagLoadingChild = true;
                return $.ajax({
                    type: "POST",
                    url: '/Admin/workschedules/SaveItem/',
                    headers: this.headers,
                    data: JSON.stringify(this.detailItem),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resul => {
                        this.flagLoadingChild = false;
                        if (resul.Success) {                            
                            $('#detail-modal').modal('hide');
                            this.getListJson();                            
                            $.toast({
                                heading: resul.Msg,
                                icon: 'success',
                                stack: false
                            });
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
            }
        },
        getItem: function () {
            this.flagLoadingChild = true;
            $.ajax({
                type: "POST",
                url: '/Admin/WorkSchedules/GetItem?Id=' + this.detailItem.Ids,
                headers: this.headers,
                data: JSON.stringify(this.detailItem.Ids),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingChild = false;
                    if (resul.Success) {
                        this.detailItem = resul.Data;
                        tinymce.get("full-textarea").setContent(this.detailItem.FullText);
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
        getListJson: function () {
            this.flagLoading = true;
            $.ajax({
                type: "POST",
                url: '/Admin/WorkSchedules/GetListJson',
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

        SearchChangeYear() {            
            this.getListJson();
        },
        showEdit: function (i) {
            this.resetFormDetail();            
            this.detailItem = Object.assign({}, this.ListItems[i]);          
            $('#detail-modal').modal('show');
            this.getItem();
        },

        showDeleted: function (i) {            
            this.resetFormDetail();
            this.deleteItem = Object.assign({}, this.ListItems[i]);
            $('#delete-modal').modal('show');
        },
        resetFormDetail: function () {
            this.ListErr = [];
            this.msgInfo = "";
            this.msgErrChild = "";
            this.detailItem = { Id: 0, Ids: '', Title: '',  IntroText: '', FullText: '', ShowWeek: this.SearchItems.ShowWeek, ShowYear: this.SearchItems.ShowYear, PublishUp: 0, PublishUpShow: 0, Status: 1, Ordering: 1 };
//            $('#WorkSchedulesForm')[0].reset();
        },
        resetFormDetailValid() {
            this.validDetailItem = { Title: "", Code: "", Ordering:"" };
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


$('#detail-modal').on('hidden.bs.modal', function (event) {
    document.getElementById("DetailItem_Title").focus();
})