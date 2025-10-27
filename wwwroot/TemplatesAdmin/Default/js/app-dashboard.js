
const vmDB = Vue.createApp(
    {
        data() {
            return {
                title: 'DashBoard',                
                detailItem: { ThangDonVi: 0, ThangHuyen: 0, ThangTinh: 0, ThangTatCa: 0 },
                SearchItems: { ShowStartDate: '', ShowEndDate :'',Nam:2010},
                flagLoading: false,                
                flagLoadingExcel: false,                
                flagLoadingVisit: false,                
                flagLoadingCountAr: false,                
                ListItems: [],              
                ListArticles: [],              
                ListVisit: [],              
                ListYear: [],               
            }
        },
        created() {
            $("#app-dashboard").show();
            var date = new Date();   
            for (let i = 2010; i <= date.getFullYear()+1; i++) {
                this.ListYear.push(i);
            }
            this.SearchItems.Nam = date.getFullYear();
            this.SearchItems.ShowStartDate = moment().startOf('month').format('DD/MM/YYYY');
            this.SearchItems.ShowEndDate = moment().endOf('month').format('DD/MM/YYYY');

            this.GetReportMonth();
            this.GetListCountArticles();
            this.GetListCountVisit();
            this.GetBieuDoCot();
            this.BieuDoLuotTruyCapCot();
        },
        methods: {
            SearchChangeYear() {
                this.GetBieuDoCot();
                this.BieuDoLuotTruyCapCot();
            },
            GetBieuDoCot() {
                return $.ajax({
                    type: "GET",
                    url: '/Admin/Home/BieuDoCot?Nam=' + this.SearchItems.Nam,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result => {

                        if (result.Success == true) {

                            am5.ready(function () {

                                am5.array.each(am5.registry.rootElements, function (root) {
                                    console.log("root=", root);
                                    if (root != undefined && root.dom.id == "chartBieuDoCot") {
                                        root.dispose();
                                    }
                                });

                                // Create root element
                                // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                                var root = am5.Root.new("chartBieuDoCot");

                                // Set themes
                                // https://www.amcharts.com/docs/v5/concepts/themes/
                                root.setThemes([
                                    am5themes_Animated.new(root)
                                ]);

                                // Create chart
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                                    panX: true,
                                    panY: true,
                                    wheelX: "panX",
                                    wheelY: "zoomX",
                                    pinchZoomX: true,
                                    paddingLeft: 0,
                                    paddingRight: 1
                                }));

                                // Add cursor
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/cursor/
                                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
                                cursor.lineY.set("visible", false);


                                // Create axes
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                                var xRenderer = am5xy.AxisRendererX.new(root, {
                                    minGridDistance: 30,
                                    minorGridEnabled: true
                                });

                                xRenderer.labels.template.setAll({
                                    rotation: -90,
                                    centerY: am5.p50,
                                    centerX: am5.p100,
                                    paddingRight: 15
                                });
                                /*
                                xRenderer.grid.template.setAll({
                                    location: 1
                                })*/

                                xRenderer.grid.template.set("visible", false);

                                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                                    maxDeviation: 0.3,
                                    categoryField: "Thang",
                                    renderer: xRenderer,
                                    tooltip: am5.Tooltip.new(root, {})
                                }));

                                var yRenderer = am5xy.AxisRendererY.new(root, {
                                    strokeOpacity: 0.1
                                })

                                yRenderer.labels.template.setAll({
                                    rotation: -30,        // Remove rotation for better readability
                                    centerY: am5.p50,
                                    centerX: am5.p100,
                                    paddingRight: 10,   // Adjust padding as needed
                                    visible: true       // Ensure labels are visible
                                });

                                // Optional: Customize Y-axis grid lines
                                yRenderer.grid.template.setAll({
                                    strokeDasharray: [2, 2],  // Dashed grid lines
                                    strokeOpacity: 0.3
                                });

                                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                                    maxDeviation: 0.3,
                                    renderer: yRenderer,                                 
                                    min: 0,
                                    numberFormat: "#,###"  // This will format numbers with commas
                                    //renderer: am5xy.AxisRendererY.new(root, {})
                                }));

                               

                                // Create series
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                                var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                                    name: "Series 1",
                                    xAxis: xAxis,
                                    yAxis: yAxis,                                                                        
                                    valueYField: "SoTinBai",
                                    sequencedInterpolation: true,
                                    categoryXField: "Thang",
                                    tooltip: am5.Tooltip.new(root, {
                                        labelText: "Số tin bài: {valueY}" 
                                    })
                                }));

                                series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
                                series.columns.template.adapters.add("fill", function (fill, target) {
                                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                                });

                                series.columns.template.adapters.add("stroke", function (stroke, target) {
                                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                                });

                               
                                // Add Label bullet
                                series.bullets.push(function () {
                                    return am5.Bullet.new(root, {
                                        locationY: 1,
                                        sprite: am5.Label.new(root, {
                                            text: "{valueYWorking.formatNumber('#.')}",
                                            fill: root.interfaceColors.get("alternativeText"),
                                            centerY: 0,
                                            centerX: am5.p50,
                                            populateText: true
                                        })
                                    });
                                });


                                var data = result.Data;

                                xAxis.data.setAll(data);
                                series.data.setAll(data);


                                // Make stuff animate on load
                                // https://www.amcharts.com/docs/v5/concepts/animations/
                                series.appear(1000);
                                chart.appear(1000, 100);

                            }); // end am5.ready()

                        }

                    },
                    error: function () {
                        this.flagLoadingVisit = false;

                    }
                });
            },
            BieuDoLuotTruyCapCot() {
                return $.ajax({
                    type: "GET",
                    url: '/Admin/Home/BieuDoLuotTruyCapCot?Nam=' + this.SearchItems.Nam,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result => {

                        if (result.Success == true) {

                            am5.ready(function () {

                                am5.array.each(am5.registry.rootElements, function (root) {
                                    console.log("root=", root);
                                    if (root != undefined && root.dom.id == "chartBieuDoLuotTruyCapCot") {
                                        root.dispose();
                                    }
                                });

                                // Create root element
                                // https://www.amcharts.com/docs/v5/getting-started/#Root_element
                                var root = am5.Root.new("chartBieuDoLuotTruyCapCot");

                                // Set themes
                                // https://www.amcharts.com/docs/v5/concepts/themes/
                                root.setThemes([
                                    am5themes_Animated.new(root)
                                ]);

                                // Create chart
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/
                                var chart = root.container.children.push(am5xy.XYChart.new(root, {
                                    panX: true,
                                    panY: true,
                                    wheelX: "panX",
                                    wheelY: "zoomX",
                                    pinchZoomX: true,
                                    paddingLeft: 0,
                                    paddingRight: 1
                                }));

                                // Add cursor
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/cursor/
                                var cursor = chart.set("cursor", am5xy.XYCursor.new(root, {}));
                                cursor.lineY.set("visible", false);


                                // Create axes
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/axes/
                                var xRenderer = am5xy.AxisRendererX.new(root, {
                                    minGridDistance: 30,
                                    minorGridEnabled: true
                                });

                                xRenderer.labels.template.setAll({
                                    rotation: -90,
                                    centerY: am5.p50,
                                    centerX: am5.p100,
                                    paddingRight: 15
                                });

                                xRenderer.grid.template.setAll({
                                    location: 1
                                })

                                var xAxis = chart.xAxes.push(am5xy.CategoryAxis.new(root, {
                                    maxDeviation: 0.3,
                                    categoryField: "Thang",
                                    renderer: xRenderer,
                                    tooltip: am5.Tooltip.new(root, {})
                                }));

                                var yRenderer = am5xy.AxisRendererY.new(root, {
                                    strokeOpacity: 0.1
                                })

                                var yAxis = chart.yAxes.push(am5xy.ValueAxis.new(root, {
                                    maxDeviation: 0.3,
                                    renderer: yRenderer
                                }));

                                // Create series
                                // https://www.amcharts.com/docs/v5/charts/xy-chart/series/
                                var series = chart.series.push(am5xy.ColumnSeries.new(root, {
                                    name: "Series 1",
                                    xAxis: xAxis,
                                    yAxis: yAxis,
                                    valueYField: "SoLuotTruyCap",
                                    sequencedInterpolation: true,
                                    categoryXField: "Thang",
                                    tooltip: am5.Tooltip.new(root, {
                                        labelText: "Lượt truy cập: {valueY}"
                                    })
                                }));

                                series.columns.template.setAll({ cornerRadiusTL: 5, cornerRadiusTR: 5, strokeOpacity: 0 });
                                series.columns.template.adapters.add("fill", function (fill, target) {
                                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                                });

                                series.columns.template.adapters.add("stroke", function (stroke, target) {
                                    return chart.get("colors").getIndex(series.columns.indexOf(target));
                                });

                                // Add Label bullet
                                series.bullets.push(function () {
                                    return am5.Bullet.new(root, {
                                        locationY: 1,
                                        sprite: am5.Label.new(root, {
                                            text: "{valueYWorking.formatNumber('#.')}",
                                            fill: root.interfaceColors.get("alternativeText"),
                                            centerY: 0,
                                            centerX: am5.p50,
                                            populateText: true
                                        })
                                    });
                                });

                                var data = result.Data;

                                xAxis.data.setAll(data);
                                series.data.setAll(data);


                                // Make stuff animate on load
                                // https://www.amcharts.com/docs/v5/concepts/animations/
                                series.appear(1000);
                                chart.appear(1000, 100);

                            }); // end am5.ready()

                        }

                    },
                    error: function () {
                        this.flagLoadingVisit = false;

                    }
                });
            },
            Report() {
                this.SearchItems.ShowStartDate = $("#ShowShowStartDate").val();
                this.SearchItems.ShowEndDate = $("#ShowShowEndDate").val();

                this.GetListCountArticles();
                this.GetListCountVisit();
            },

            ExportExcel() {
                this.SearchItems.ShowStartDate = $("#ShowShowStartDate").val();
                this.SearchItems.ShowEndDate = $("#ShowShowEndDate").val();

                var token = jQuery('input[name="__RequestVerificationToken"]').val();
                var headers = {};
                headers["RequestVerificationToken"] = token;
                this.flagLoadingExcel = true;

                return $.ajax({
                    type: "POST",
                    url: '/Admin/Home/ExportCountArticles',
                    data: JSON.stringify(this.SearchItems),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",                  
                    success: result => {
                        
                        setTimeout(result => { this.flagLoadingExcel = false; }, 1500);

                        if (result.Success == true) {
                            DownLoadFileByUrl(headers, result.Data, "thong-ke-bai-viet.xlsx");
                        } 
                    },
                    error: function () {
                        this.flagLoadingExcel = false;

                    }
                });
                
                
            },
           
            GetReportMonth() {
                var token = jQuery('input[name="__RequestVerificationToken"]').val();
                var headers = {};
                headers["RequestVerificationToken"] = token;

                this.flagLoading = true;
                return $.ajax({
                    type: "GET",
                    url: '/Admin/Home/GetReportArticle',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result=> {
                        this.flagLoading = false;
                        if (result.Success == true) {

                            this.detailItem = result.Data;
                            
                        } else {

                        }

                    },
                    error: function () {
                        this.flagLoading = false;

                    }
                });
            },
            GetListCountArticles() {
                var token = jQuery('input[name="__RequestVerificationToken"]').val();
                var headers = {};
                headers["RequestVerificationToken"] = token;

                this.flagLoadingCountAr = true;
                this.ListArticles = [];
                return $.ajax({
                    type: "POST",
                    url: '/Admin/Home/GetListCountArticles',
                    data: JSON.stringify(this.SearchItems),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result=> {
                        this.flagLoadingCountAr = false;
                        if (result.Success == true) {
                            this.ListArticles = result.Data;
                        }
                    },
                    error: function () {
                        this.flagLoadingCountAr = false;

                    }
                });
            },
            GetListCountVisit() {
                var token = jQuery('input[name="__RequestVerificationToken"]').val();
                var headers = {};
                headers["RequestVerificationToken"] = token;
                this.ListVisit = [];
                this.flagLoadingVisit = true;
                return $.ajax({
                    type: "POST",
                    url: '/Admin/Home/GetListCountVisit',
                    data: JSON.stringify(this.SearchItems),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: result=> {
                        this.flagLoadingVisit = false;
                        if (result.Success == true) {

                            this.ListVisit = result.Data;

                        } 

                    },
                    error: function () {
                        this.flagLoadingVisit = false;

                    }
                });
            },
            ExportCountArticles: function () {

            },
        }
    }
).mount('#app-dashboard');



