var index = function () {

    function init() {
        common.hookOnButton(".emsg-edit-button", function (that) {
            var id = $(that).data("id");
            openMittleresErhebungsjahrEditWindow(id);
        });
    }

    function openStrassenmengeUndZustandEditWindow(id) {
        var url = $('#NetzSummarischGrid').data('editnetzsummarischdetailmodel-url');
        openEditWindow(id, url, 'strassenmengeUndZustandFormDiv', 'StrassenmengeUndZustandEditWindow', null, editNetzSummarischDetailModel.init);
    }

    function openMittleresErhebungsjahrEditWindow(id) {
        var url = $('#NetzSummarischGrid').data('editmittlereserhebungsjahr-url');
        openEditWindow(id, url, "mittleresErhebungsjahrFormDiv", "MittleresErhebungsjahrEditEditWindow", null, editMittleresErhebungsjahr.init);
    }

    return {
        init: init,
        openStrassenmengeUndZustandEditWindow: openStrassenmengeUndZustandEditWindow
    };

} ();


var editNetzSummarischDetailModel = function () {

    function init() {
        common.hookOnSubmit(init, closeEditWindow);
        common.hookOnCancel(closeEditWindow);
    }

    function closeEditWindow() {
        $('#StrassenmengeUndZustandEditWindow').data('kendoWindow').close();
    }

    function onStrassenmengeUndZustandFormClose() {
        $('strassenmengeUndZustandFormDiv').empty();
        common.refreshGrid("#NetzSummarischGrid");
    }

    return {
        init: init,
        onStrassenmengeUndZustandFormClose: onStrassenmengeUndZustandFormClose
    };

} ();


var editMittleresErhebungsjahr = function () {

    function init() {
        common.hookOnSubmit(init, closeEditWindow);
        common.hookOnCancel(closeEditWindow);
    }

    function closeEditWindow() {
        $('#MittleresErhebungsjahrEditEditWindow').data('kendoWindow').close();
    }

    function onMittleresErhebungsjahrFormClose() {
        var url = $("#NetzSummarischGrid").data("getmittlereserhebungsjahr-url");
        replaceTextContent(url, 'MittleresErhebungsjahr');
    }

    return {
        init: init,
        onMittleresErhebungsjahrFormClose: onMittleresErhebungsjahrFormClose
    };

} ();


$(function () {
    index.init();
});