
const vmSurvey = Vue.createApp(
    {
        data() {
            return {
                message: 'Khảo sát',
                detailItem: { SurveyId: 0, SurveyIds: '', Id: 0, Ids: '', IdCoQuan:0,Title:'' },
                flagLoading: false,
                flagLoadingChild: false,
                ListItems: [],
                checkQuestion: 0,
                flagQuestion: 0,
                msgErr:'',
                msgSuccess:'',
            }
        },
        created() {

            localStorage.setItem("flagQuestion", "dsd");
            this.getListSurvey();
        },
        methods: {
            SendQuestion(Id,Ids) {
                this.flagLoadingChild = true;
                this.msgErr = '';
                this.msgSuccess = '';
                var token = jQuery('input[name="__RequestVerificationToken"]').val();
                
                var headers = {};
                headers["RequestVerificationToken"] = token;

                this.detailItem.SurveyId = Id;
                this.detailItem.SurveyIds = Ids;
                this.detailItem.Id = this.checkQuestion;

              

                grecaptcha.ready(function () {
                    grecaptcha.execute('6Lc9Mz4aAAAAAIcKI4xnbI2BWZc5zaBkRhXotoUq', { action: 'submit' }).then(function (token) {
                        //$("#token_recaptcha").val(token);
                        console.log(token);
                       
                        vmSurvey.detailItem.Token = token;
                        return $.ajax({
                            type: "POST",
                            url: '/Home/SendQuestion',
                            headers: headers,
                            data: JSON.stringify(vmSurvey.detailItem),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (result) {
                                vmSurvey.flagLoadingChild = false;
                                if (result.Success == true) {

                                    vmSurvey.msgSuccess = result.Msg;
                                    vmSurvey.flagQuestion = 1;
                                    localStorage.setItem("flagQuestion", vmSurvey.detailItem.SurveyId);

                                } else {
                                    vmSurvey.msgErr = result.Msg;
                                }

                            },
                            error: function () {
                                vmSurvey.flagLoadingChild = false;

                            }
                        });
                    });
                });


                
            },
            ViewQuestion() {
                this.flagLoadingChild = true;
                this.flagLoadingChild = false;
            },
            getListSurvey() {
                var token = jQuery('input[name="__RequestVerificationToken"]').val();
                var headers = {};
                headers["RequestVerificationToken"] = token;

                this.flagLoading = true;
                return $.ajax({
                    type: "POST",
                    url: '/Home/GetListSurvey',
                    headers: headers,
                    data: JSON.stringify(this.detailItem),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        vmSurvey.flagLoading = false;
                        if (result.Success == true) {

                            vmSurvey.ListItems = result.Data;
                            
                            if (localStorage.getItem("flagQuestion") != null) {
                                if (localStorage.getItem("flagQuestion") == vmSurvey.ListItems[0].Id) {

                                    vmSurvey.flagQuestion = 1;
                                } else {
                                    vmSurvey.flagQuestion = 0;
                                }
                            }

                        } else {

                        }

                    },
                    error: function () {
                        vmSurvey.flagLoading = false;

                    }
                });
            }
        }
    }
).mount('#vm_survey');
