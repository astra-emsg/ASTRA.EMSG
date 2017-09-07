var index = function () {

    function init() {
    }
    
    function onRowDataBound(e) {
        var rows = e.sender.tbody.children();        
        for (var i = 0; i < rows.length; i++) {
            var dataItem = e.sender.dataItem(rows[i]);
            if (dataItem.IsUsed) {
                var cellChild = $(rows[i]).find(".k-button.k-grid-delete");
                if (cellChild.length > 0) {
                    cellChild.addClass("k-state-disabled");
                    cellChild.removeClass("k-grid-delete");
                    cellChild.attr("style", "background: none repeat scroll 0 0 lightgray;");
                    cellChild.attr("title", $("#MassnahmenvorschlagGrid").data("is-used-message"));
                }
            }
        }
    }

    function openMassnahmenvorschlagKatalogWindow(typ) {
        var url = getGrid().data('edit-url');
        var title = getGrid().data('edit-window-title');
        openGeneralWindowWithPostedData(url, { typ: typ }, 'massnahmenvorschlagWindowDiv', 'MassnahmenvorschlagWindow', title, edit.init);
    }

    function openCreateMassnahmenvorschlagKatalogWindow() {
        var url = getGrid().data('create-url');
        var title = getGrid().data('create-window-title');
        openCreateWindow(url, "massnahmenvorschlagWindowDiv", "MassnahmenvorschlagWindow", title, edit.init);
    }
    
    function getGrid() {
        return $('#MassnahmenvorschlagGrid');
    }

    return {
        init: init,
        getGrid: getGrid,
        onRowDataBound: onRowDataBound,
        openMassnahmenvorschlagKatalogWindow: openMassnahmenvorschlagKatalogWindow,
        openCreateMassnahmenvorschlagKatalogWindow: openCreateMassnahmenvorschlagKatalogWindow
    };
    
} ();


var edit = function () {

    function init() {
        common.hookOnSubmit(init, closeEditWindow);
        common.hookOnCancel(closeEditWindow);
    }

    function onClose() {
        common.refreshGrid('#MassnahmenvorschlagGrid', true);
    }

    function closeEditWindow() {
        $('#MassnahmenvorschlagWindow').data('kendoWindow').close();
    }

    function getGrid() {
        return $('#MassnahmenvorschlagGrid');
    }

    function onChange(e) {
        var url = getGrid().data('load-default-url');
        common.destroyErrorDialogs();
        $.post(url, $('#Typ').closest('form').serialize(), 
            function (data) {
                $('#massnahmenvorschlagWindowDiv').empty().append(data);
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