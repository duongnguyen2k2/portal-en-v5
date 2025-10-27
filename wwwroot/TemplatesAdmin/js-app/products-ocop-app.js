

var vmProducts = new Vue({
	el: '#app-products-ocop',
	data: {
		title: "Quản lý tour",
		detailItem: { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0 },
		deleteItem: { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0 },
		SearchItems: { CurrentPage: 1, ItemsPerPage: 10, Keyword: '', TotalItems: 0, PageCount: 0, Status: -1, CatId: -1 },
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
		document.getElementById("app-products-ocop").style.display = "block";
		var token = jQuery('input[name="__RequestVerificationToken"]').val();
		this.headers["RequestVerificationToken"] = token;

		this.getListJson();

		this.deleteModal = new bootstrap.Modal(document.getElementById('delete-modal'), {});
		this.copyModal = new bootstrap.Modal(document.getElementById('copy-modal'), {});

		this.IdGroupUser = $("#User_IdGroup").val();

		this.getListCat();
	},
	methods: {

		checkForm: function (count) {
			this.resetFormDetailValid();
			if (this.detailItem.Title == null || this.detailItem.Title == "") {
				this.validDetailItem.Title = "Tên không được để trống";
			}

			if (this.detailItem.Price < 0) {
				this.validDetailItem.Price = "Giá Tour không được để trống";
			}

			if (this.detailItem.PriceDeal < 0) {
				this.validDetailItem.PriceDeal = "Giá gốc Tour không được để trống";
			}

			if (this.detailItem.TimeDay < 0) {
				this.validDetailItem.TimeDay = "Số ngày không hợp lệ";
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
				url: '/Admin/ProductsOCOP/UpdateStatus?Ids=' + this.ListItems[i].Ids + "&Status=" + Status,
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

		deletedItem: function () {

			return $.ajax({
				type: "POST",
				url: '/Admin/ProductsOCOP/DeleteItem',
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

				},
				error: function () {


				}
			});

		},

		copyItem: function () {
			this.flagLoadingChild = true;
			return $.ajax({
				type: "POST",
				url: '/Admin/ProductsOCOP/CopyItem',
				headers: this.headers,
				data: JSON.stringify(this.detailItem),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: resul => {
					this.getListJson();
					this.flagLoadingChild = false;

					this.copyModal.hide();

					if (!resul.Success) {
						$.toast({
							heading: resul.Msg,
							icon: 'error',
						});
					} else {
						$.toast({
							heading: `Copy dữ liệu <strong>` + this.deleteItem.Title + `</strong> thành công`,
							icon: 'success',
							stack: false
						});
					}

				},
				error: function () {
					this.flagLoadingChild = false;
					$.toast({
						heading: `Lỗi khi copy dữ liệu`,
						icon: 'error',
						stack: false
					});

				}
			});

		},

		saveItem: function () {
			console.log("saveItem111");

			if (this.checkForm()) {
				this.flagLoadingChild = true;
				return $.ajax({
					type: "POST",
					url: '/Admin/ProductsOCOP/SaveItemJson/',
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
			console.log("getListJson")
			this.flagLoading = true;
			$.ajax({
				type: "POST",
				url: '/Admin/ProductsOCOP/GetListJson',
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

		getListCat: function () {
			this.flagLoadingCat = true;
			$.ajax({
				type: "POST",
				url: '/Admin/CategoriesProducts/GetListAll',
				headers: this.headers,
				data: JSON.stringify(this.SearchItems),
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: resul => {
					this.flagLoadingCat = false;
					if (resul.Success) {
						this.ListCat = resul.Data;
					}
				},
				error: function () {

				}
			});
		},

		showEdit: function (i) {
			this.resetFormDetail();
			this.detailItem = Object.assign({}, this.ListItems[i]);
			this.detailModal.show();
		},

		showDeleted: function (i) {
			console.log("showDeleted");
			this.resetFormDetail();
			this.deleteItem = Object.assign({}, this.ListItems[i]);
			this.deleteModal.show();
		},
		showCopy: function (i) {
			this.resetFormDetail();
			this.detailItem = Object.assign({}, this.ListItems[i]);
			this.copyModal.show();
		},
		resetFormDetail: function () {
			//this.ListErr = [];
			//this.msgInfo = "";
			//this.msgErrChild = "";
			//this.detailItem = { Id: 0, Ids: '', Title: '', Sku: '', Description: '', Status: 1, Ordering: 1, TimeDay: 0, PriceDeal: 0, Price: 0 };
			//$('#ProductsForm')[0].reset();
		},
		resetFormDetailValid() {
			this.validDetailItem = { Title: "", Sku: "", Ordering: "", TimeDay: "", PriceDeal: "", Price: "" };
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


