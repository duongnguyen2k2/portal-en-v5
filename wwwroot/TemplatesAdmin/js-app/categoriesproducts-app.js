

var vmCategoriesProducts = new Vue({
    el: '#app-categoriesproducts',
    data: {
        title: "Loại sản phẩm",
        detailItem: { Id: 0, Ids: '', Title: '', Code: '', Description: '', Status: 1, Ordering:1 },
        deleteItem: { Id: 0, Ids: '', Title: '', Code: '', Description: '', Status: 1, Ordering:1},
        SearchItems: { CurrentPage: 1, ItemsPerPage: 100, Keyword: '', TotalItems: 0, PageCount:0 ,Status:-1},
        flagLoading: false,
        flagLoadingChild: false,        
        flagPermissionAll: false,
        ListErr: [],
        ListItems: [],
        msgInfo: '',
        msgErrChild: '',
        detailModal: null,
        deleteModal: null,        
        detailPermission: null,
        validDetailItem: { Title: "", Code: "", Ordering:"" },
        headers: [],
        ListSkeleton: [1, 2, 3, 4],
        ListGroups: [],
        detailItemsPermission: { Id: 0, Ids: '', Title: '', Status: 0 },
        IdGroupUser:0
    },
   
    mounted() {
        document.getElementById("app-categoriesproducts").style.display = "block";
        var token = jQuery('input[name="__RequestVerificationToken"]').val();
        this.headers["RequestVerificationToken"] = token;

        this.getListJson();
        this.detailModal = new bootstrap.Modal(document.getElementById('detail-modal'), {});
        this.deleteModal = new bootstrap.Modal(document.getElementById('delete-modal'), {});
        this.detailPermission = new bootstrap.Modal(document.getElementById('permission-modal'), {});
        this.IdGroupUser = $("#User_IdGroup").val();
    },
    methods: {
        
        checkForm: function (count) {
            this.resetFormDetailValid();
            if (this.detailItem.Title == null || this.detailItem.Title == "") {
                this.validDetailItem.Title = "Tên không được để trống";
            }

            if (this.detailItem.Ordering < 0) {
                this.validDetailItem.Ordering = "Số thứ tự không hợp lệ";
            }

            if (this.validDetailItem.Title == "") {
                return true;
            } else {
                return false;
            }
            
        },
        showAdd: function () {                     
            this.resetFormDetail();
            this.detailModal.show();           
            $("#DetailItem_Title").focus();
        },
        changeStatus: function (i) {
            
            var Status = false;
            if (this.ListItems[i].Status == false) {
                Status = true;
            }

            return $.ajax({
                type: "GET",
                url: '/Admin/CategoriesProducts/UpdateStatus?Ids=' + this.ListItems[i].Ids + "&Status=" + Status,
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
                    this.getListJson();
                },
                error: function () {


                }
            });

        },
		changeFeatured: function (i) {
            
            var Featured = false;
            if (this.ListItems[i].Featured == false) {
                Featured = true;
            }

            return $.ajax({
                type: "GET",
                url: '/Admin/CategoriesProducts/UpdateFeatured?Ids=' + this.ListItems[i].Ids + "&Featured=" + Featured,
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
                        this.ListItems[i].Featured = Featured;                       
                        $.toast({
                            heading: `Cập nhật Nổi bật <strong>` + this.ListItems[i].Title + `</strong> thành công`,                            
                            icon: 'success',                            
                            stack: false                            
                        });                      
                    }
                    this.getListJson();
                },
                error: function () {


                }
            });

        },
        deletedItem: function () {
            
            return $.ajax({
                type: "POST",
                url: '/Admin/CategoriesProducts/DeleteItem',
                headers: this.headers,
                data: JSON.stringify(this.deleteItem),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.getListJson();
                    this.deleteModal.hide();
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
                    this.getListJson();
                },
                error: function () {


                }
            });

        },

        saveItem: function () {
            if (this.checkForm()) {
                this.flagLoadingChild = true;
                return $.ajax({
                    type: "POST",
                    url: '/Admin/CategoriesProducts/SaveItem/',
                    headers: this.headers,
                    data: JSON.stringify(this.detailItem),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: resul => {
                        this.flagLoadingChild = false;
                        if (resul.Success) {
                            this.detailModal.hide();
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
        getListJson: function () {
            this.flagLoading = true;
            $.ajax({
                type: "POST",
                url: '/Admin/CategoriesProducts/GetListJson',
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
                    $.toast({
                        heading: "Lỗi khi lấy dữ liệu",
                        icon: 'error',
                    });
                }
            });
        },

        SaveManagerCatGroup() {
            this.flagLoadingChild = true;
            return $.ajax({
                type: "POST",
                url: '/Admin/CategoriesProducts/SaveManagerCatGroup',
                headers: this.headers,
                data: JSON.stringify(this.ListGroups),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingChild = false;
                    this.detailPermission.hide();
                    $.toast({
                        heading: resul.Msg,
                        icon: 'success',
                        stack: false
                    });
                    this.getListJson();
                },
                error: function () {

                    this.flagLoadingChild = false;
                    $.toast({
                        heading: "Lỗi khi Lưu dữ liệu",
                        icon: 'error',
                    });
                }
            });
        },

        getListGroup(ids) {
            this.flagLoadingChild = true;
            return $.ajax({
                type: "GET",
                url: '/Admin/CategoriesProducts/GetListGroupsByCat?Ids=' + ids,
                headers: this.headers,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingChild = false;
                    this.ListGroups = resul.Data;
                },
                error: function () {

                    this.flagLoadingChild = false;
                    $.toast({
                        heading: "Lỗi khi lấy dữ liệu",
                        icon: 'error',
                    });
                }
            });
        },
        showPermission(i) {
            this.ListErrChild = [];
            this.getListGroup(this.ListItems[i].Ids);
            this.detailItem = this.ListItems[i];
            this.detailPermission.show();
        },
        showAddPermission: function (i) {
            this.detailItemsPermission = this.ListGroups[i];
        },
        CheckAllPermission: function () {
            console.log(this.flagPermissionAll);
            if (this.ListGroups != null && this.ListGroups.length > 0) {
                for (let i = 0; i < this.ListGroups.length; i++) {
                    this.ListGroups[i].Selected = !this.flagPermissionAll;
                }
            }

        },
        showEdit: function (i) {
            this.resetFormDetail();            
            this.detailItem = Object.assign({}, this.ListItems[i]);
            this.detailModal.show();
        },

        showDeleted: function (i) {            
            this.resetFormDetail();
            this.deleteItem = Object.assign({}, this.ListItems[i]);
            this.deleteModal.show();
        },
        resetFormDetail: function () {
            this.ListErr = [];
            this.msgInfo = "";
            this.msgErrChild = "";
            this.detailItem = { Id: 0, Ids: '', Title: '', Code: '', Description: '', Status: 1, Ordering:1 };            
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


var detailModal = document.getElementById('detail-modal');
detailModal.addEventListener('show.bs.modal', function (event) {
    document.getElementById("DetailItem_Title").focus();
})

