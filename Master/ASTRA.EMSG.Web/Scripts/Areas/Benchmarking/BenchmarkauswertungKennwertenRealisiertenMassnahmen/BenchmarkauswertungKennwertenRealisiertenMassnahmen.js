var index = function () {

    function init() {
        common.hookOnSubmit(init);
    }



    return {
        init: init
    };

} ();

$(index.init);
