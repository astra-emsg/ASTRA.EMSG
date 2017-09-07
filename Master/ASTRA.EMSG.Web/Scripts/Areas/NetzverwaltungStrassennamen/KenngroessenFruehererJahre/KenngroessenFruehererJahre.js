var index = function () {

    function openKenngroessenFruehererJahreEditWindow(id) {
        var gird = $('#KenngroessenFruehererJahreGrid');
        var editUrl = gird.data('editurl');
        var windowTitle = gird.data('editwindowtitle');
        openEditWindow(id, editUrl, 'kenngroessenFruehererJahreFormDiv', 'KenngroessenFruehererJahreWindow', windowTitle, edit.init);
    }

    function openKenngroessenFruehererJahreCreateWindow() {
        var gird = $('#KenngroessenFruehererJahreGrid');
        var createUrl = gird.data('createurl');
        var windowTitle = gird.data('createwindowtitle');
        openGeneralWindow(createUrl, 'kenngroessenFruehererJahreFormDiv', 'KenngroessenFruehererJahreWindow', windowTitle, edit.init);
    }

    function onClose() { refreshGrid(true); }

    function refreshGrid(keepCurrentPage) {
        common.refreshGrid("#KenngroessenFruehererJahreGrid", keepCurrentPage);
    }

    function init() {
        common.hookOnFilter(function() { refreshGrid(); });
    }

    return {
        openKenngroessenFruehererJahreEditWindow: openKenngroessenFruehererJahreEditWindow,
        openKenngroessenFruehererJahreCreateWindow: openKenngroessenFruehererJahreCreateWindow,
        onCloseKenngroessenFruehererJahreWindow: onClose,
        init: init
    };

} ();

var edit = function () {
    function init() {
        common.hookOnForm(init,closeEditWindow, function () {
            closeEditWindow();
        });

        $("#Jahr").keydown(handleDecimalSeparator);
    }

    function closeEditWindow() {
        $('#KenngroessenFruehererJahreWindow').data('kendoWindow').close();
    }

    return {
        init: init
    };
} ();


$(function() {
    index.init();
});
