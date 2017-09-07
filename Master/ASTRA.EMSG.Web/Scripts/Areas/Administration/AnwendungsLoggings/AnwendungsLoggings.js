var index = function () {

    function init() {
        common.hookOnNotAjaxForm(function () {
            window.location = $("#parameters").data("cancel-url");
        });
    }

    return {
        init: init
    };

} ();

$(function () {
    index.init();
});