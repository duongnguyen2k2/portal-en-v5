requirejs.config({
    paths: {
        'axios': '../js/axios.min'      
    }
});


requirejs(['axios'], function (axios) {

    var vmManagerFile = Vue.createApp(
        {
            data() {
                return {
                    title: 'Danh sách file',
                    detailItem: { CurrentFolder: "", Lang: '', Type: '', CurrentFolder: '', Hash: '', CurrentPage: 0, ItemsPerPage: 0, Keyword:'' },
                    searchItem: { CurrentFolder: "", Lang: '', Type: '', CurrentFolder: '', Hash: '', CurrentPage: 0, ItemsPerPage: 0, Keyword:'' ,ParentId:0},
                    detailItemFolder: { Acl: 0, Name: '', HasChildren: true, Url: '', Path: '/uploads/', PathParent: '', Img: '', Alias: '', Icon: '', Id: 0, Description: '', Ids: '', CreatedBy: 0, ModifiedBy: 0, ParentId: 0 },
                    detailItemFile: {Id:0,Ids:'',Name:'',Size:0,StrSize:''},
                    detailItemsPermission: { Id: 0, Ids: '', Title: '', Status:0},
                    flagLoading: false,
                    flagLoadingChild: false,
                    flagPermissionAll: false,                    
                    ListFolders: [],
                    ListFiles: [],
                    ListErrChild: [],
                    ListBreadcrumb: [],
                    ListFilesUpload: [],
                    ListSelectBox: [],
                    ListGroups: [],
                    ListSkeleton: [1,2,3,4],
                    myModal: null,
                    token: "",
                }
            },
            created() {
                               
                this.token = jQuery('input[name="__RequestVerificationToken"]').val();                
                document.getElementById("backend_manager_folder").style.display = "block";
                this.GetListItems(-2);

                
            },
            methods: {
                UploadFiles: function () {

                    this.ListErrChild = [];
                    var formData = new FormData();
                    for (var i = 0; i < $('#UploadFileFolder')[0].files.length; ++i) {
                        formData.append('file', $('#UploadFileFolder')[0].files[i]);
                    }
                    
                    formData.append("CurrentFolder", this.detailItemFolder.Alias);
                    formData.append("Ids", this.detailItemFolder.Ids);

                    this.flagLoadingChild = true;

                    axios({
                        method: 'post',
                        url: '/Admin/ManagerFolder/UploadFiles',
                        headers: { RequestVerificationToken: this.token },
                        processData: false,
                        contentType: false,                        
                        data: formData
                    }).then(function (resul) {
                        console.log(resul);
                        vmManagerFile.flagLoadingChild = false;
                        if (resul.data.Success) {
                            vmManagerFile.ListFilesUpload = resul.data.Data;
                            vmManagerFile.GetListItems(-1);

                        } else {
                            vmManagerFile.ListErrChild.push(resul.data.Msg);
                        }
                    }).catch(function (error) {
                        console.log(error);
                    }); 
                },
                DeleteFile: function () {
                    this.flagLoadingChild = true;

                    axios({
                        method: 'post',
                        url: '/Admin/ManagerFolder/DeleteFile',
                        headers: { RequestVerificationToken: this.token },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: this.detailItemFile
                    }).then(function (resul) {
                        console.log(resul);
                        vmManagerFile.flagLoadingChild = false;
                        $('#DeleteFileModal').modal('hide');
                        if (resul.data.Success) {
                            
                            if (resul.data.Data == null) {
                                vmManagerFile.GetListItems(-2);
                            } else {
                                vmManagerFile.GetListItems(-3);
                            }

                        } else {
                            vmManagerFile.ListErrChild.push(resul.data.Msg);
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
                },
                ResetItems() {

                },
                UpdateFile: function () {
                    this.flagLoadingChild = true;    
                    this.ListErrChild = [];
                    axios({
                        method: 'post',
                        url: '/Admin/ManagerFolder/UpdateFile',
                        headers: { RequestVerificationToken: this.token },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: this.detailItemFile
                    }).then(function (resul) {
                        console.log(resul);
                        vmManagerFile.flagLoadingChild = false;
                        if (resul.data.Success) {
                            $('#UpdateFileModal').modal('hide');                            
                            vmManagerFile.GetListItems(-3);

                        } else {
                            vmManagerFile.ListErrChild.push(resul.data.Msg);
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });                    
                },
                SaveItemFolder: function () {
                    this.flagLoadingChild = true;
                    this.ListErrChild = [];
                    this.detailItemFolder.Description = $('textarea#FolderDescription').tinymce().getContent();
                    this.detailItemFolder.Img = $('#Item_ImagesFull_Article').val();
                    axios({
                        method: 'post',
                        url: '/Admin/ManagerFolder/SaveFolder',
                        headers: { RequestVerificationToken: this.token },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: this.detailItemFolder
                    }).then(function (resul) {
                        console.log(resul);
                        vmManagerFile.flagLoadingChild = false;
                        if (resul.data.Success) {
                            $('#FolderModal').modal('hide');          
                            //vmManagerFile.detailItemFolder = resul.data.Data;
                            vmManagerFile.GetListItems(-3);
                            
                        } else {
                            vmManagerFile.ListErrChild.push(resul.data.Msg);
                        }
                    }).catch(function (error) {                            
                            console.log(error);
                    });                    
                },

                DeleteFolder: function () {
                    this.flagLoadingChild = true;

                    axios({
                        method: 'post',
                        url: '/Admin/ManagerFolder/DeleteFolder',
                        headers: { RequestVerificationToken: this.token },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: this.detailItemFolder
                    }).then(function (resul) {
                        console.log(resul);
                        vmManagerFile.flagLoadingChild = false;
                        if (resul.data.Success) {
                            $('#DeleteFolderModal').modal('hide');                            
                            if (resul.data.Data == null) {
                                vmManagerFile.GetListItems(-2);
                            } else {                                
                                vmManagerFile.GetListItems(-3);
                            }
                            
                            
                        } else {
                            vmManagerFile.ListErrChild.push(resul.data.Msg);
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
                },
                MoveFolder: function () {
                    this.flagLoadingChild = true;
                    this.ListErrChild = [];
                    axios({
                        method: 'post',
                        url: '/Admin/ManagerFolder/MoveFolder',
                        headers: { RequestVerificationToken: this.token },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        data: this.detailItemFolder
                    }).then(function (resul) {
                        console.log(resul);
                        vmManagerFile.flagLoadingChild = false;
                        if (resul.data.Success) {
                            $('#MoveFolderModal').modal('hide');
                            if (resul.data.Data == null) {
                                vmManagerFile.GetListItems(-2);
                            } else {                                
                                vmManagerFile.GetListItems(-3);
                            }
                            
                            
                        } else {
                            vmManagerFile.ListErrChild.push(resul.data.Msg);
                        }
                    }).catch(function (error) {
                        console.log(error);
                    });
                },

                GetListSelectBox(Id) {
                    this.ListSelectBox = [];                    
                    axios.post('/Admin/ManagerFolder/GetListSelectBox/' + Id)
                        .then(response => {
                            // handle success
                            console.log(response);
                            this.ListSelectBox = response.data.Data;                           
                        })
                        .catch(function (error) {

                        });
                },
                
                GetListItems(i = -1) {
                    if (i == -3) {

                    }
                    else if (i == -2) {
                        this.searchItem.CurrentFolder = '/uploads/';
                        this.searchItem.ParentId = 0;
                        this.searchItem.ParentIds ="";
                    } else if (i == -1) {
                        this.searchItem.CurrentFolder = this.detailItemFolder.Path;
                        this.searchItem.ParentId = this.detailItemFolder.Id;
                        this.searchItem.ParentIds = this.detailItemFolder.Ids;
                    }else if (i > -1) {
                        this.searchItem.ParentId = this.ListFolders[i].Id;
                        this.searchItem.ParentIds = this.ListFolders[i].Ids;
                        this.searchItem.CurrentFolder = this.ListFolders[i].Path;
                        console.log('search >-1', this.searchItem);
                    }

                    this.flagLoading = true;

                    axios.post('/Admin/ManagerFolder/GetListItems', this.searchItem)
                        .then(response => {                            
                            this.flagLoading = false;
                            this.ListFolders = response.data.Data.ListFolders;
                            this.ListFiles = response.data.Data.ListFiles;
                            this.ListBreadcrumb = response.data.Data.ListBreadcrumb;
                        })
                        .catch(function (error) {

                            //window.location.href = "/Home/Privacy";
                        });
                },
                getItem(ids) {                    
                    $('textarea#FolderDescription').html('');
                    this.GetListSelectBox();
                    axios.post('/Admin/ManagerFolder/GetItem/' + ids)
                        .then(response => {                            
                            this.detailItemFolder = response.data.Data;      
                            this.flagLoadingChild = false;                            
                            $('textarea#FolderDescription').html(response.data.Data.Description);
                        })
                        .catch(error => {
                            this.flagLoadingChild = false;                            
                        });
                },
                SaveManagerFolderGroup() {

                    axios.post('/Admin/ManagerFolder/SaveManagerFolderGroup/', this.ListGroups)
                        .then(function (response) {                          
                            vmManagerFile.flagLoadingChild = false;
                            $('#PermissionModal').modal('hide');
                            vmManagerFile.GetListItems(-3);
                        })
                        .catch(function (error) {
                            vmManagerFile.flagLoadingChild = false;
                        });
                },
                getListGroup(ids) {

                    axios.post('/Admin/ManagerFolder/GetListGroupsByFolder/' + ids)
                        .then(function (response) {
                            vmManagerFile.ListGroups = response.data.Data;
                            vmManagerFile.flagLoadingChild = false;
                        })
                        .catch(function (error) {
                            vmManagerFile.flagLoadingChild = false;
                        });
                },
                DownloadFile(i) {
                    
                    var title = this.ListFiles[i].Name;
                    axios.post('/Admin/ManagerFolder/DownloadFile', this.ListFiles[i], { responseType: 'arraybuffer' })
                        .then(function (response) {
                            const url = window.URL.createObjectURL(new Blob([response.data]))
                            const link = document.createElement('a')
                            link.href = url
                            link.setAttribute('download', title)
                            document.body.appendChild(link)
                            link.click()
                        })
                        .catch(function (error) {
                            // handle error
                            console.log(error);
                        });


                },
                forceFileDownload(response, title) {
                    console.log(title)
                    const url = window.URL.createObjectURL(new Blob([response.data]))
                    const link = document.createElement('a')
                    link.href = url
                    link.setAttribute('download', title)
                    document.body.appendChild(link)
                    link.click()
                },
                downloadWithAxios(url, title) {
                    axios({
                        method: 'get',
                        url,
                        responseType: 'arraybuffer',
                    }).then((response) => {
                        this.forceFileDownload(response, title)
                    }).catch(() => console.log('error occured'));
                },
                showUpdateFile(i) {
                    this.flagLoadingChild = false;
                    this.ListErrChild = [];
                    this.detailItemFile = this.ListFiles[i];

                    axios.post('/Admin/ManagerFolder/GetItemFile/' + this.ListFiles[i].Ids, this.ListFiles[i])
                        .then(function (response) {
                            vmManagerFile.detailItemFile = response.data.Data;     
                            $('#UpdateFileModal').modal('show');
                        })
                        .catch(function (error) {
                            // handle error
                            console.log(error);
                        });

                    
                },
                GetListBreadcrumb(i) {
                    
                    console.log('ListBreadcrumb', this.ListBreadcrumb[i]);
                    this.detailItemFolder = this.ListBreadcrumb[i];
                    this.GetListItems(-1);
                },
                SearchChange() {
                    this.GetListSearch();
                },
                ResetItems() {
                    this.GetListItems(-2);
                },
                showAddNewFile() {
                    
                    this.flagLoadingChild = false;
                    this.ListErrChild = [];
                    this.ListFilesUpload = [];
                    this.getItem(this.searchItem.ParentIds);
                    console.log("showAddNewFile", this.searchItem);
                    document.getElementById("FormFileFolder").reset();
                    $('#UploadFileModal').modal('show');
                },
                showAddNewFolder(i) {
                    console.log('showAddNewFolder');
                    $('textarea#FolderDescription').tinymce({});
                    this.flagLoadingChild = false;
                    this.ListErrChild = [];
                    if (i < 0) {
                        this.detailItemFolder = { Acl: 0, Name: '', HasChildren: true, Url: '', Path: this.searchItem.CurrentFolder, PathParent: this.searchItem.CurrentFolder, Img: '', Alias: '', Icon: '', Id: 0, Description: '', Ids: '', CreatedBy: 0, ModifiedBy: 0, ParentId: this.searchItem.ParentId, ParentIds: this.searchItem.ParentIds };
                    } else {
                        this.getItem(this.ListFolders[i].Ids);
                    }
                    $('#FolderModal').modal('show');
                },
                showDeleteFolder(i) {
                    this.flagLoadingChild = false;
                    this.ListErrChild = [];
                    this.getItem(this.ListFolders[i].Ids);
                    $('#DeleteFolderModal').modal('show');
                },
                showDeleteFile(i) {
                    this.flagLoadingChild = false;
                    this.ListErrChild = [];
                    this.detailItemFile = this.ListFiles[i];
                    $('#DeleteFileModal').modal('show');
                },

                showMoveFolder(i) {
                    this.flagLoadingChild = false;
                    this.ListErrChild = [];
                    this.detailItemFolder = this.ListFolders[i];

                    this.GetListSelectBox(this.ListFolders[i].Id);                   
                    $('#MoveFolderModal').modal('show');
                },
                showPermission(i) {

                    this.ListErrChild = [];
                    this.getListGroup(this.ListFolders[i].Ids);
                    this.detailItemFolder = this.ListFolders[i];
                    $('#PermissionModal').modal('show');
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

                }
            }
        }


    ).mount('#backend_manager_folder');


});


