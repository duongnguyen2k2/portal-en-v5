

var vmImport = new Vue({
    el: '#app-import-articles',
    data: {
        title: "Quản lý tour",
        detailItem: { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0 },
        deleteItem: { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0 },        
        SearchItems: { CurrentPage: 1, Cookie: "", ItemsPerPage: 30, PageStart: 1, PageEnd:1, Keyword: '', TotalItems: 0, PageCount: 0, Status: -1, CatId: -1, Link:"https://cumgar.daklak.gov.vn/web/guest/tin-hoat-dong",LinkRoot:"" },
        flagLoadingCat: false,
        flagLoading: false,
        flagLoadingChild: false,
        ListErr: [],
        ListItems: [],
        msgInfo: '',
        msgErrChild: '',
        detailModal: null,
        deleteModal: null,
        copyModal: null,
        validDetailItem: { Title: "", Sku: "", Ordering: "", TimeDay: "", PriceDeal: "", Price: "" },
        headers: [],
        ListSkeleton: [1, 2, 3, 4],
        IdGroupUser: 0,
        ListCat: [],

    },

    mounted() {
        document.getElementById("app-import-articles").style.display = "block";
        var token = jQuery('input[name="__RequestVerificationToken"]').val();
        this.headers["RequestVerificationToken"] = token;
        //this.getListCat();
    },
    methods: {

        checkForm: function (count) {
            this.ListErr = [];
            if (this.SearchItems.LinkRoot == null || this.SearchItems.LinkRoot == "") {
                this.ListErr.push("Tên không được để trống");
            }

            if (this.SearchItems.CatId < 0) {
                this.ListErr.push("Loại tin tức đổ không được để trống");
            }
            

            if (this.ListErr.length == 0) {
                return true;
            } else {
                return false;
            }

        },
        ImportDB: function () {
            if (this.checkForm()) {
                this.flagLoading = true;
                
                if (this.SearchItems.PageEnd >= this.SearchItems.PageStart) {
                    for (let i = this.SearchItems.PageStart; i <= this.SearchItems.PageEnd; i++) {                        
                        this.importDatabase(i);                        
                    }
                } else {
                    this.flagLoading = false;
                }
                //var ListId = [6, 7, 20, 21, 24, 25, 57, 58, 81, 82, 83, 84, 85, 87, 98, 104, 167, 197];
                /*
                var ListId = [58, 81, 83, 85,  98, 167, 197];
                for (let i = 0; i < ListId.length; i++)
                {
                    //console.log(ListId[i]);
                    this.importDatabase(ListId[i], "DOLAI",i);
                }*/
                //
                /*
                $.ajax({
                    type: "POST",
                    url: '/Import/Index',
                    headers: this.headers,
                    data: JSON.stringify(this.SearchItems),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result => {
                        this.flagLoading = false;
                        if (result.Success) {
                            this.ListItems = result.Data.ListItems;
                            console.log("thành công", this.ListItems);
                        }

                    },
                    error: function () {

                    }
                });*/
            } else {
                alert(this.ListErr);
                console.log("Looi");
            }
            
        },

        
        importDatabase: function (i, flag = "",n) {
            let time = i * 10000;
            if (flag == "DOLAI") {
                time = n * 30000;
            }
            setTimeout(function (SearchItems, i, headers) {
                var Cookie = "";
                SearchItems.Link = SearchItems.LinkRoot + '?p_p_id=101_INSTANCE_BwxVxSF2Cj3T&_101_INSTANCE_BwxVxSF2Cj3T_cur=' + i;                
                console.log("Lần:" + i + "- Time:" + new Date().toLocaleString() + "- Thời Gian:" + i * 5000, SearchItems);  

                $.ajax({
                    type: "POST",
                    url: '/Import/Index',
                    headers: headers,
                    data: JSON.stringify(SearchItems),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result => {
                        SearchItems.Cookie = result.Data.SearchData.Cookie;
                    },
                    error: function () {

                    }
                });

            }, time, this.SearchItems, i, this.headers);
                       
        },

        getListCat: function () {
            this.flagLoadingCat = true;
            $.ajax({
                type: "POST",
                url: '/Api/GetListCatArticleImport',
                headers: this.headers,
                data: JSON.stringify(this.SearchItems),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: resul => {
                    this.flagLoadingCat = false;
                    this.flagLoading = true;

                    if (resul.Success) {
                        this.ListCat = resul.Data;
                        if (this.ListCat != null && this.ListCat.length > 0) {
                            for (let i = 0; i < this.ListCat.length; i++) {
                                let time = i * 410000;
                                console.log("I nhé" + i + ";time:" + time);
                                setTimeout(function ($this, time, ItemCat, n) {
                                    console.log("Loại Tin:" + time + ";n Cat thứ:" + n + "; IdCat:" + ItemCat.Id, ItemCat);                                    
                                    if (ItemCat.PageEnd >= ItemCat.PageStart) {
                                        for (let j = ItemCat.PageStart; j <= ItemCat.PageEnd; j++) {
                                           $this.importDatabase2(j, ItemCat);
                                        }
                                    } 
                                }, time,this, time, this.ListCat[i], i);

                               
                            }// End For cat
                        }
                    }
                },
                error: function () {

                }
            });
        },

        importDatabase2: function (i, ItemCat) {
            let time = i * 15000;
            
            setTimeout(function (time, ItemCat, i, headers) {
                console.log("Time import:" + time);
                if (i > 1) {
                    ItemCat.Link = ItemCat.LinkSiteRoot + i;
                } else {
                    ItemCat.Link = ItemCat.LinkSiteRoot;
                }
                
                console.log("Lần:" + i + "- Time:" + new Date().toLocaleString() + "- Thời Gian:" + i * 1000 + ";Link:" + ItemCat.Link);
                //console.log(ItemCat.Link);

                let SearchItems = {
                    CurrentPage: 1, Cookie: "", ItemsPerPage: 30,
                    PageStart: ItemCat.PageStart, PageEnd: ItemCat.PageEnd,
                    Link : ItemCat.Link,
                    Keyword: '', TotalItems: 0,
                    PageCount: 0, Status: -1, CatId: ItemCat.Id,
                    //
                };
                
                $.ajax({
                    type: "POST",
                    url: '/Import/Index',
                    headers: headers,
                    data: JSON.stringify(SearchItems),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result => {
                        
                    },
                    error: function () {

                    }
                });

            }, time, time, ItemCat, i, this.headers);

        },

      
        pageChanged() {
            this.getListJson();
        },

        ChangeKeyWord: function (event) {
            console.log(event);
            if (event.which == 13) {
                //this.SearchChange();
            }
        },
        SearchChange() {
            //this.getListJson();
        },
    }
});



/*
 ## Văn Hóa Xã Hội
 Lần: 6,7,20,21,24,25,57,58,81,82,83,84,85,87,98,104,167,197
*/
 