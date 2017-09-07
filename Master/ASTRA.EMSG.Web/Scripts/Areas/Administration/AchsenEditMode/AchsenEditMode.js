var index = function () {

    function init() {
        common.hookOnNotAjaxForm(function () {
            window.location = $("#parameters").data("cancel-url");
        }, function() {
                return confirm($("#form").data("confirmation"));
            }
        );
    }

    return {
        init: init
    };

} ();

$(function () {
    index.init();
});