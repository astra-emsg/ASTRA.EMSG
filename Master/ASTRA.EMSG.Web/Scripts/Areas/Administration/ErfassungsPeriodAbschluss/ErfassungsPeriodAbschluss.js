var index = function () {

    function init() {
        common.hookOnSubmit(init);
    }

    return {
        init: init
    };

} ();


$(function () {
    index.init();
});