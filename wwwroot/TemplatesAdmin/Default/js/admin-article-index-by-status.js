

    $('document').ready(function () {

       

        $("#ArticleStatusItemTrue").click(function () {                
            var form = $('#AjaxDeleteForm');
            var token = jQuery('input[name="__RequestVerificationToken"]', form).val();
            var ids = jQuery("#ArticleStatusItemTrue").attr("data-ids");
            var url = jQuery("#ArticleStatusItemTrue").attr("data-url");
            var headers = {};
            headers["RequestVerificationToken"] = token;
            return $.ajax({
                type: "POST",
                url: url + 'ChangeArticlesStatusId/' + ids,
                headers: headers,
                data: JSON.stringify({ "Ids": ids }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    window.location.reload();
                },
                error: function () {
                    window.location.reload();
                }
            });
        });


    });