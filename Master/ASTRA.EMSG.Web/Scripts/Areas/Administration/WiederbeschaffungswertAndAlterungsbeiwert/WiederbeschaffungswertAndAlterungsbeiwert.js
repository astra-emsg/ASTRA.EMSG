var index = function () {

    function init() {
    }
    
    function openWiederbeschaffungswertAndAlterungsbeiwertWindow(id) {
        var url = getGrid().data('edit-url');
        var title = getGrid().data('edit-window-title');
        openEditWindow(id, url, 'wiederbeschaffungswertAndAlterungsbeiwertWindowDiv', 'WiederbeschaffungswertAndAlterungsbeiwertWindow', title, edit.init);
    }

    function openÇreateWiederbeschaffungswertAndAlterungsbeiwertWindow() {
        var url = getGrid().data('create-url');
        var title = getGrid().data('create-window-title');
        openCreateWindow(url, 'wiederbeschaffungswertAndAlterungsbeiwertWindowDiv', 'WiederbeschaffungswertAndAlterungsbeiwertWindow', title, edit.init);
    }
    
    function getGrid() {
        return $('#WiederbeschaffungswertAndAlterungsbeiwertGrid');
    }

    return {
        init: init,
        getGrid: getGrid,
        openWiederbeschaffungswertAndAlterungsbeiwertWindow: openWiederbeschaffungswertAndAlterungsbeiwertWindow,
        openÇreateWiederbeschaffungswertAndAlterungsbeiwertWindow: openÇreateWiederbeschaffungswertAndAlterungsbeiwertWindow
    };
    
} ();


var edit = function () {

    function init() {
        common.hookOnSubmit(init, closeEditWindow);
        common.hookOnCancel(closeEditWindow);
    }

    function onClose() {
        common.refreshGrid('#WiederbeschaffungswertAndAlterungsbeiwertGrid', true);
    }

    function closeEditWindow() {
        $('#WiederbeschaffungswertAndAlterungsbeiwertWindow').data('kendoWindow').close();
    }

    function onChange(e) {
        var url = index.getGrid().data('load-default-url');
        common.destroyErrorDialogs();
        $.post(url, $('#Belastungskategorie').closest('form').serialize(),
            function (data) {
                $('#wiederbeschaffungswertAndAlterungsbeiwertWindowDiv').empty().append(data);
                init();
            });
    }

    return {
        init: init,
        onClose: onClose,
        onChange: onChange
    };

} ();

$(index.init);