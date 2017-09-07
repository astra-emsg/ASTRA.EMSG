var index = function () {

    function init() {
        common.hookOnFilter(refreshGrid);
    }

    function refreshGrid(keepCurrentPage) {
        common.refreshGrid("#RealisierteMassnahmeSummarsichGrid", keepCurrentPage);
    }

    function openRealisierteMassnahmeSummarsichCreateWindow() {
        var url = getGrid().data('create-url');
        var title = getGrid().data('create-window-title');
        openCreateWindow(url, 'realisierteMassnahmeSummarsichFormDiv', 'RealisierteMassnahmeSummarsichWindow', title, edit.init);
    }

    function openRealisierteMassnahmeSummarsichEditWindow(id) {
        var url = getGrid().data('edit-url');
        var title = getGrid().data('edit-window-title');
        openEditWindow(id, url, 'realisierteMassnahmeSummarsichFormDiv', 'RealisierteMassnahmeSummarsichWindow', title, edit.init);
    }

    function getFilteredData(e) {
        return {
            Projektname: $("#Projektname").val()
        };
    }

    function getGrid() {
        return $('#RealisierteMassnahmeSummarsichGrid');
    }

    return {
        init: init,
        getFilteredData: getFilteredData,
        openRealisierteMassnahmeSummarsichEditWindow: openRealisierteMassnahmeSummarsichEditWindow,
        openRealisierteMassnahmeSummarsichCreateWindow: openRealisierteMassnahmeSummarsichCreateWindow,
        getGrid: getGrid
    };

} ();


var edit = function () {

    function init() {
        common.hookOnSubmit(init, function () {
            closeEditWindow();
            var warning = index.getGrid().data("saved-warning");
            alert(warning);
        });
        common.hookOnCancel(closeEditWindow);
    }

    function closeEditWindow() {
        $("#RealisierteMassnahmeSummarsichWindow").data('kendoWindow').close();
    }

    function onClose() {
        common.refreshGrid("#RealisierteMassnahmeSummarsichGrid", true);
    }

    return {
        init: init,
        onClose: onClose
    };

} ();


$(function () {
    index.init();
});