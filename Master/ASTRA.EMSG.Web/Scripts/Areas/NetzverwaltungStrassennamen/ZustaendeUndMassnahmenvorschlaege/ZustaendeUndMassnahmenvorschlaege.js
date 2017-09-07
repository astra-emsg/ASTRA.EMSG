var index = function () {
    function init() {
    }

    function openZustandsabschnittWindow(id) {
        var gird = getGrid();
        var editUrl = gird.data('edit-url');
        openFullScreenEditWindow(id, editUrl, "zustandsabschnittEditFormDiv", "ZustandsabschnittWindow", '', function () { edit.init('edit-window-title', false); });
    }

    function openCreateZustandsabschnittWindow(id) {
        var gird = getGrid();
        var createUrl = gird.data('create-url');
        openFullScreenEditWindow(id, createUrl, "zustandsabschnittEditFormDiv", "ZustandsabschnittWindow", '', function () { edit.init('create-window-title', true); });
    }

    function openImportWindow() {
        var gird = getGrid();
        var importUrl = gird.data('import-url');
        openGeneralWindow(importUrl, "importFormDiv", "ImportWindow");
        var w = $('#ImportWindow').data('kendoWindow');
        w.setOptions({
            width: 950,
            height: 640
        });
        w.center();
    }

    function onClose(e) {
        if (monsterEditor.isDirty() && !confirm($('#zustandsabschnittEditForm').data("cancel-confirmation")))
        {
            e.preventDefault();
            return;
        }

        $('zustandsabschnittEditFormDiv').empty();
        $('importFormDiv').empty();
        common.refreshGrid("#ZustandsabschnittenGrid", true);
    }

    function importWindowOnClose(e) {
        var importParameters = $("#importParameters");
        if (importParameters.length == 1 && !confirm(importParameters.data("import-widow-close-confirmation"))) {
            e.preventDefault();
            return;
        }

        $('importFormDiv').empty();
        common.refreshGrid("#ZustandsabschnittenGrid");
    }

    function getGrid() {
        return $('#ZustandsabschnittenGrid');
    }

    function onTabSelect(e) {
        var idx = 0;
        $.each($("#TabStrip").children('.k-tabstrip-items').find('a.k-link'), function (i, a) {
            if ($(a).text() == $(e.item).text()) {
                idx = i;
                return false;
            }
        });
        $("#SelectedTabIndex").val(idx);
    }
    
    function onRowDataBound(e) {
        var rows = e.sender.tbody.children();
        for (var i = 0; i < rows.length; i++) {
            if (rows[i].innerHTML.indexOf('javascript:index.openZustandsabschnittWindow') != -1) {
                var dataItem = e.sender.dataItem(rows[i]);
                var anchor = $(rows[i]).find(".emsg-zustandsabschnitt-white-edit-button");
                if (anchor.length > 0) {
                    anchor.toggle(!dataItem.IsAufnahmedatumInAktuellenErfassungsperiod);
                }
                anchor = $(rows[i]).find(".emsg-zustandsabschnitt-edit-button");
                if (anchor.length > 0) {
                    anchor.toggle(dataItem.IsAufnahmedatumInAktuellenErfassungsperiod);
                }
            }
        }
    }

    return {
        init: init,
        openZustandsabschnittWindow: openZustandsabschnittWindow,
        openCreateZustandsabschnittWindow: openCreateZustandsabschnittWindow,
        openImportWindow: openImportWindow,
        onTabSelect: onTabSelect,
        onClose: onClose,
        importWindowOnClose: importWindowOnClose,
        onRowDataBound: onRowDataBound
    };
}();

var edit = function () {

    var self = this;

    function getForm() {
        return $('#zustandsabschnittEditForm');
    }

    function init(windowTitleKey) {
        $('#ZustandsabschnittWindow').data('kendoWindow').title(getForm().data(windowTitleKey));
        setTimeout(function () { reMaximizeWindow($('#ZustandsabschnittWindow')); }, 1000);

        startZustandDirtyTrancking();
        common.hookOnForm(function() { init(windowTitleKey); }, null, function() {
            monsterEditor.resetIsDirty();
            closeEditWindow();
        });

        $("#Stammdaten_BezeichnungBis").keydown(function (e) {
            var tabKeyCode = 9;
            if (e.keyCode == tabKeyCode) {
                var activeTabId = $(".k-content.k-state-active")[0].id;
                focusFirstInput(activeTabId);

                if (e.preventDefault) {
                    e.preventDefault();
                }
                return false;
            }
        });
    }

    function closeEditWindow() {
        $('#ZustandsabschnittWindow').data('kendoWindow').close();
    }

    function getMassnahmenvorschlagKosten(event) {
        var target = event.sender.value();
        var targetSpanId = '#' + $(this.element).data('rechte-trottoir');
        $.ajax({
            url: getForm().data('kosten-url'),
            type: "POST",
            data: { massnahmenvorschlagKatalogId: target },
            success: function (data) {
                $(targetSpanId).empty().append(data);
            }
        });
    }

    function getErfassungForm(zustandsErfassungsmodus, id, url) {
        $.ajax({
            url: url,
            type: 'POST',
            data: { zustandsErfassungsmodus: zustandsErfassungsmodus, id: id },
            success: function (data) {
                {
                    $('#erfassungFormDiv').empty().append(data);

                    //Restore the window to maximum. Otherwise the popup buttons won't displayed for the Detail form.
                    reMaximizeWindow($('#ZustandsabschnittWindow'));
                    
                    monsterEditor.init();
                }
            }
        });
    }
    
    function reMaximizeWindow(windowSelector) {
        var tWindow = windowSelector.data('kendoWindow');
        tWindow.restore();
        tWindow.center();
        tWindow.maximize();
        $('body').css('overflow', 'visible');
    }

    function startZustandDirtyTrancking() {

        monsterEditor.init();

        $("input[name$=Erfassungsmodus]:radio").change(function (eventObject) {
            var currentZustandsErfassungsmodus = $("#CurrentZustandsErfassungsmodus").val();
            if (currentZustandsErfassungsmodus != eventObject.target.value) {
                var form = getForm();
                if ((monsterEditor.isDirty() ||
                    ($("#Fahrbahn_Zustandsindex").length !== 0 && $("#Fahrbahn_Zustandsindex").data("kendoNumericTextBox").value()) ||
                    ($("#Fahrbahn_ZustandsindexCalculated").length !== 0 && parseFloat(($("#Fahrbahn_ZustandsindexCalculated").val())) > 0)
                    ) && !confirm(form.data('zustandserfassungsmodechanged-warning'))) {
                    selectRadioButton(form.data('erfassung-mode-id'), currentZustandsErfassungsmodus);
                    eventObject.preventDefault();
                    return false;
                } else {
                    if (form.data('is-new') === "True")
                        getErfassungForm(eventObject.target.value, $("#Stammdaten_Strassenabschnitt").val(), form.data('create-erfassung-form-url'));
                    else
                        getErfassungForm(eventObject.target.value, $('#Fahrbahn_Id').val(), getForm().data('get-erfassung-form-url'));

                    if (eventObject.target.value != "Manuel")
                        $('#Fahrbahn_Is' + eventObject.target.value + "Initializiert").val("True");

                    monsterEditor.resetIsDirty();
                    $("#zustandsabschnittFormDiv input").off("change");
                }
            }
        });
    }

    return {
        init: init,
        getMassnahmenvorschlagKosten: getMassnahmenvorschlagKosten
    };
} ();

var monsterEditor = function () {

    var _isDirty = false;

    function init() {
        _isDirty = false;
        $("#zustandsabschnittFormDiv input").change(function () {
            setIsDirty();
        });

        $("#zustandsabschnittFormDiv td").click(function (e) {
            var children = e.target.children;
            if (children.length > 0 && children[0].type == 'radio') {
                if (!children[0].checked) {
                    children[0].checked = 'checked';
                    monsterEditor.setIsDirty();
                }
            }

            highlightSelectedCells();
        });

        highlightSelectedCells();
    }

    function isDirty() {
        return _isDirty;
    }

    function setIsDirty() {
        return _isDirty = true;
    }

    function resetIsDirty() {
        _isDirty = false;
    }

    function highlightSelectedCells() {
        $.each($("#zustandsabschnittFormDiv td"), function (i, v) {
            if (v.children.length > 0 && v.children[0].type == 'radio') {
                $(v).toggleClass("emsg-selected-cell", v.children[0].checked);
            }
        });
    }

    highlightSelectedCells();

    return {
        init: init,
        resetIsDirty: resetIsDirty,
        setIsDirty: setIsDirty,
        isDirty: isDirty
    };
}();

var zustandsabschnittImport = function () {
    function init() {
    }

    function getData(dataName) {
        return $("#parameters").attr("data-" + dataName);
    }

    function getResultDiv() {
        return $("#result");
    }

    function onError(e) {
        var errorMessage;

        if (e.XMLHttpRequest.responseText.indexOf("Access is denied.") != -1) {
            errorMessage = getData("errorMessage");
        }
        else {
            errorMessage = e.XMLHttpRequest.responseText;
        }
        getResultDiv().empty().append(errorMessage);

        e.preventDefault();
    }

    function onSuccess(e) {
        $.post(getData("uploadResultUrl"), null, function (data) {
            $("#importFormDiv").empty().append(data);

            //If there was no error we got the ImportResult (Commit) site
            if ($("#importParameters").length > 0)
                zustandsabschnittCommit.init();
        });
    }

    function onSelect(e) {
        var files = e.files;
        $.each(files, function () {
            if (this.extension.toLowerCase() != ".xlsx") {
                alert(getData("fileExtensionErrorMessage"));
                e.preventDefault();
                return false;
            }
        });

        getResultDiv().empty();
    }

    function onUpload(e) {
        getResultDiv().empty().append(getData("progressText"));
    }

    function closeImportWindow() {
        $('#ImportWindow').data('kendoWindow').close();
    }

    return {
        onUpload: onUpload,
        onSelect: onSelect,
        onSuccess: onSuccess,
        onError: onError,
        closeImportWindow: closeImportWindow,
        init: init
    };
}();

var zustandsabschnittCommit = function () {

    function init() {
        common.hookOnButton(".emsg-commit-import-button", commitImport);
        common.hookOnButton(".emsg-cancel-import-button", cancelImport);
    }

    function commitImport(that) {
        var url = $(that).data("commit-url");
        $.ajax({
            url: url,
            type: 'POST',
            success: function (data) {
                $('#importFormDiv').empty().append(data);

                var w = $('#ImportWindow').data('kendoWindow');
                w.setOptions({
                    width: 400,
                    height: 200
                });
                w.center();
            }
        });
    }

    function cancelImport(that) {
        $('#ImportWindow').data('kendoWindow').close();
    }

    return {
        init: init
    };
}();