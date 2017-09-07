var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

var detailsViewModel = new GisMode.Details();

var overview = new GisMode.OverviewModel(index, '#ZustandsabschnittenGrid',
    {
        deleted: function () { return emsg.events.form.onZustandsabschnittDeleted; },
        created: function () { return emsg.events.form.onZustandsabschnittCreated; },
        selected: function () { return emsg.events.form.onZustandsabschnittSelected; }
    },
    function (e) {
        if (e.dataItem.IsLocked) {
            alert($("#ZustandsabschnittenGrid").data("inspektionsroute-locked-warning"));
            e.preventDefault();
        }
    }
);

index.getFilteredData = function () {
    return { strassennameFilter: $("#StrassennameFilter").val() };
}

overview.onRowDataBound = function (e) {
    var rows = e.sender.tbody.children();
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].innerHTML.indexOf('javascript:overview.edit') != -1) {
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
};

var details = function () {
    
    var dirtyTracker = new GisMode.DirtyTracker('#zustandsabschnittEditFormDiv');

    emsg.events.map.onZustandsabschnittCreated = zustandsabschnittCreated;
    emsg.events.map.onZustandsabschnittSelected = zustandsabschnittSelected;
    emsg.form.executeCancel = closeForm;

    function zustandsabschnittCreated(strassenabschnittId) {
        var formDiv = $('#zustandsabschnittEditFormDiv');
        $.ajax({
            url: formDiv.data('create-action') + '/' + strassenabschnittId,
            type: 'POST',
            success: function (data) {
                $('#zustandsabschnittEditFormDiv').empty().append(data);
                index.disableTab(1);
                init();
                emsg.events.form.onDataLoaded();
                dirtyTracker.setIsDirty();
            }
        });
    };

    function zustandsabschnittSelected(zustandsabschnittId) {
        var formDiv = $('#zustandsabschnittEditFormDiv');
        $.ajax({
            url: formDiv.data('edit-action') + '/' + zustandsabschnittId,
            type: 'POST',
            success: function (data) {
                $('#zustandsabschnittEditFormDiv').empty().append(data);
                emsg.events.form.onDataLoaded();
                index.disableTab(1);
                init();
            }
        });
    };

    var selectedZustandsErfassungsmodus;

    function init() {
        selectedZustandsErfassungsmodus = $("#CurrentZustandGISsErfassungsmodus").val();

        detailsViewModel.showForm();
        common.hookOnSubmit(init, function () {
            closeForm();
            emsg.events.form.onSaveSuccess();
        });
        common.hookOnApply(init);
        common.hookOnButton(".emsg-cancel-button", hookOnCancel);
        common.hookOnButton(".emsg-delete-button", hookOnDelete);
        startZustandDirtyTrancking();

        $("#Stammdaten_BezeichnungBis").keydown(function (e) {
            var tabKeyCode = 9;
            if (e.keyCode == tabKeyCode) {
                var activeTabId = $("#ZustandTabStrip .k-content.k-state-active")[0].id;
                focusFirstInput(activeTabId);

                if (e.preventDefault) {
                    e.preventDefault();
                }
                return false;
            }
        });
        
        if ($("#IsValid").val().toLowerCase() === 'true') {
            dirtyTracker.resetIsDirty();
            monsterEditor.isSaved(true);
        }
    }

    function hookOnCancel(that) {
        emsg.cancel();
    }
    
    var zustandsdetailData;

    function hookOnClose() {
        common.hookOnButton("#zustandsabschnittFahrbahnDiv .emsg-submit-button", function () {
            isCloseButtonClick = false;
            $('#ZustandsabschnittFahrbahnWindow').data('kendoWindow').close();
            isCloseButtonClick = true;

            $('#zustandsabschnittFahrbahnDiv #Fahrbahn_Is' + selectedZustandsErfassungsmodus + "Initializiert").val("True");
            $('#zustandsabschnittFahrbahnHiddenDiv').empty().append($('#zustandsabschnittFahrbahnDiv').children().clone());

            zustandsdetailData = JSON.stringify(getForm().serializeObject());
            
            refreshSchadenerfassungsForm(zustandsdetailData,
                    function () {
                        recalculateZustandIndex();
                        $("#zustandsabschnittFahrbahnDiv .emsg-submit-button").unbind('click');
                        $("#zustandsabschnittFahrbahnDiv .emsg-submit-button").unbind('keypress');
                    }
                );
        });
        setTimeout(function () { reMaximizeWindow($('#ZustandsabschnittFahrbahnWindow')); }, 1000);
    }

    var isCloseButtonClick = true;

    function onZustandsabschnittFahrbahnWindowClose(e) {
        if (!isCloseButtonClick)
            return;

        var warning = getForm().data("cancel-warning");
        if (!monsterEditor.isDirty() || confirm(warning)) {
            cancelSchadenerfassung();
        } else {
            e.preventDefault();
            return;
        }
    };

    function cancelSchadenerfassung() {
        monsterEditor.isSaved(true);
        refreshSchadenerfassungsForm(zustandsdetailData,
                function () {
                    $('#zustandsabschnittFahrbahnHiddenDiv').empty().append($('#zustandsabschnittFahrbahnDiv').children().clone());

                    recalculateZustandIndex();
                    $("#zustandsabschnittFahrbahnDiv .emsg-submit-button").unbind('click');
                    $("#zustandsabschnittFahrbahnDiv .emsg-submit-button").unbind('keypress');
                }
            );
    }

    function recalculateZustandIndex() {
        $.ajax({
            url: getForm().data('recalulate-zustandsindex-url'),
            type: "POST",
            contentType: 'application/json',
            dataType: 'text',
            data: JSON.stringify(getForm().serializeObject()),
            success: function (data) {
                $('.' + selectedZustandsErfassungsmodus + 'ZustandsindexCalculated').text(data);
            }
        });
    }

    function getZustandIndex() {
        $.ajax({
            url: getForm().data('get-zustandsindex-url'),
            type: "POST",
            data: { id: $("#Stammdaten_Id").val() },
            success: function (data) {
                $("#Fahrbahn_Zustandsindex").data("kendoNumericTextBox").value(data);
            }
        });
    }

    function refreshSchadenerfassungsForm(formData, callBack) {
        var stringJsonData = JSON.stringify(getForm().serializeObject());
        if (formData)
            stringJsonData = formData;
        $.ajax({
            url: getForm().data('refresh-erfassungform-url'),
            type: "POST",
            contentType: 'application/json',
            data: stringJsonData,
            success: function (data) {
                $('#zustandsabschnittFahrbahnDiv').empty().append(data);
                monsterEditor.init();
                monsterEditor.resetIsDirty();
                hookOnClose();
                if (callBack)
                    callBack();
            }
        });
    }

    function reMaximizeWindow(windowSelector) {
        var tWindow = windowSelector.data('kendoWindow');
        tWindow.restore();
        tWindow.center();
        tWindow.maximize();
        $('html').css('overflow', 'visible');
        $('body').css('overflow', 'visible');
    }

    function hookOnDelete(that) {
        var zustandsabschnittId = $('#zustandsabschnittEditFormDiv #Stammdaten_Id').val();
        var comfirmationMessage = $(that).data('delete-comfirmation-message');
        if (!comfirmationMessage || confirm(comfirmationMessage)) {
            $.post($(that).data('delete-action') + '/' + zustandsabschnittId, function () {
                closeForm();
                emsg.events.form.onZustandsabschnittDeleted(zustandsabschnittId);
            });
        }
    }

    function getMassnahmenvorschlagKosten(event) {
        var target = event.sender.value();
        $.ajax({
            url: getForm().data('kosten-url'),
            type: "POST",
            data: { massnahmenvorschlagKatalogId: target },
            success: function (data) {
                $("#Fahrbahn_Kosten").empty().append(data);
                dirtyTracker.setIsDirty();
            }
        });
    }

    function getForm() {
        return $('#zustandsabschnittEditForm');
    }
    
    function hasZustandsindex() {
        if (selectedZustandsErfassungsmodus == "Manuel")
            return $("#Fahrbahn_Zustandsindex").data("kendoNumericTextBox").value();
        return $('.' + selectedZustandsErfassungsmodus + 'ZustandsindexCalculated').text();
    }

    function startZustandDirtyTrancking() {

        openZustandsabschnittFahrbahnWindow(selectedZustandsErfassungsmodus);

        $("input[name$=Erfassungsmodus]:radio").change(function (eventObject) {
            if (selectedZustandsErfassungsmodus != eventObject.target.value) {
                var form = getForm();
                if (hasZustandsindex() && !confirm(form.data('zustandserfassungsmodechanged-warning'))) {
                    selectRadioButton(form.data('erfassung-mode-id'), selectedZustandsErfassungsmodus);
                    eventObject.preventDefault();
                    return false;
                } else {
                    selectedZustandsErfassungsmodus = eventObject.target.value;
                    openZustandsabschnittFahrbahnWindow(eventObject.target.value,
                        function () {
                            $('#zustandsabschnittFahrbahnHiddenDiv').empty().append($('#zustandsabschnittFahrbahnDiv').children().clone());

                            var currentZustandsErfassungsmodus = $("#CurrentZustandGISsErfassungsmodus").val();
                            if (currentZustandsErfassungsmodus != eventObject.target.value) {
                                if (eventObject.target.value != "Manuel")
                                    $('.' + selectedZustandsErfassungsmodus + 'ZustandsindexCalculated').text('');
                                else
                                    $("#Fahrbahn_Zustandsindex").data("kendoNumericTextBox").value("");
                            } else {
                                if (eventObject.target.value == "Manuel") {
                                    $("#Fahrbahn_Zustandsindex").data("kendoNumericTextBox").value("");
                                } else {
                                    recalculateZustandIndex();
                                }
                            }
                        
                            monsterEditor.resetIsDirty();

                            $('#DetailButton').hide();
                            $('#DetailValue').hide();
                            $('#GrobButton').hide();
                            $('#GrobValue').hide();
                            $('#ManuelButton').hide();
                            $('#' + selectedZustandsErfassungsmodus + 'Button').css('display', 'inline-block');
                            $('#' + selectedZustandsErfassungsmodus + 'Value').css('display', 'inline-block');
                            
                            zustandsdetailData = undefined;
                        }
                    );
                }
            }
        });
    }

    function closeForm() {
        $("#zustandsabschnittEditFormDiv").empty();
        emsg.events.map.onZustandsabschnittChanged = function (geoData, geoLength) { };
        index.enableTab(1);
        detailsViewModel.hideForm();
        dirtyTracker.resetIsDirty();
    }

    function onTabSelect(e) {
        var idx = 0;
        $.each($("#ZustandTabStrip").children('.k-tabstrip-items').find('a.k-link'), function (i, a) {
            if ($(a).text() == $(e.item).text()) {
                idx = i;
                return false;
            }
        });
        $("#SelectedTabIndex").val(idx);
    }

    return {
        trackChanges: dirtyTracker.trackChanges,
        dropDownChanged: dirtyTracker.setIsDirty,
        hookOnClose: hookOnClose,
        closeForm: closeForm,
        onZustandsabschnittFahrbahnWindowClose: onZustandsabschnittFahrbahnWindowClose,
        onTabSelect: onTabSelect,
        getMassnahmenvorschlagKosten: getMassnahmenvorschlagKosten,
        recalculateZustandIndex: recalculateZustandIndex,
        refreshSchadenerfassungsForm: refreshSchadenerfassungsForm
    };
}();

var edit = new zustandsabschnittFahrbahnTrottoir.ZustandsabschnittFahrbahnTrottoirEditors('#ZustandsabschnittWindow');

var monsterEditor = function () {

    var _isDirty = false;
    var _isSaved = true;

    function init() {
        $("#zustandsabschnittFahrbahnDiv input").change(function () {
            setIsDirty();
        });
        
        $("#Fahrbahn_Zustandsindex").change(function () {
            setIsDirty();
        });

        $("#zustandsabschnittFahrbahnDiv td").click(function (e) {
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

        common.hookOnButton(".emsg-recalculate-button", hookOnReCalculate);
    }

    function hookOnReCalculate(that) {
        $('#zustandsabschnittFahrbahnHiddenDiv').empty().append($('#zustandsabschnittFahrbahnDiv').children().clone());
        details.refreshSchadenerfassungsForm();
    }

    function cancel() {
        $('#ZustandsabschnittFahrbahnWindow').data('kendoWindow').close();
    }

    function isDirty() {
        return _isDirty;
    }

    function setIsDirty() {
        _isSaved = false;
        return _isDirty = true;
    }
    
    function isSaved(value) {
        if (value == undefined)
            return _isSaved;
        return _isSaved = value;
    }

    function resetIsDirty() {
        _isDirty = false;
    }

    function highlightSelectedCells() {
        $.each($("#zustandsabschnittFahrbahnDiv td"), function (i, v) {
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
        isDirty: isDirty,
        isSaved: isSaved,
        cancel: cancel
    };
}();

$(function () {
    details.trackChanges();
});