var index = function () {

    function getFilteredData(e) {
        return {
            ProjektnameFilter: $("#ProjektnameFilter").val()
        };
    }

    function onComplete(e) {
        if (e.name == "delete") {
            refreshGrid();
        }
    }

    function openRealisierteMassnahmeEditWindow(id) {
        var gird = $('#RealisierteMassnahmeGrid');
        var editUrl = gird.data('editurl');
        var windowTitle = gird.data('editwindowtitle');
        openEditWindow(id, editUrl, 'realisierteMassnahmeFormDiv', 'RealisierteMassnahmeWindow', windowTitle, edit.init);
    }

    function openRealisierteMassnahmeCreateWindow() {
        var gird = $('#RealisierteMassnahmeGrid');
        var createUrl = gird.data('createurl');
        var windowTitle = gird.data('createwindowtitle');
        openGeneralWindow(createUrl, 'realisierteMassnahmeFormDiv', 'RealisierteMassnahmeWindow', windowTitle, edit.init);
    }

    function onClose() { refreshGrid(true); }

    function refreshGrid(keepCurrentPage) {
        common.refreshGrid("#RealisierteMassnahmeGrid", keepCurrentPage);
    }

    function init() {
        common.hookOnFilter(function () { refreshGrid(); });
    }

    return {
        getFilteredData: getFilteredData,
        onComplete: onComplete,
        openRealisierteMassnahmeEditWindow: openRealisierteMassnahmeEditWindow,
        openRealisierteMassnahmeCreateWindow: openRealisierteMassnahmeCreateWindow,
        onCloseRealisierteMassnahmeWindow: onClose,
        init: init
    };

} ();

var edit = function () {
    function init() {
        common.hookOnForm(init, closeEditWindow, function () {
            closeEditWindow();
            alert($('#RealisierteMassnahmeWindow').data('savedwarning'));
        }, undefined, undefined, function () {
            alert($('#RealisierteMassnahmeWindow').data('savedwarning'));
        });
    }

    function closeEditWindow() {
        $('#RealisierteMassnahmeWindow').data('kendoWindow').close();
    }

    return {
        init: init
    };
} ();


$(function() {
    index.init();
});
