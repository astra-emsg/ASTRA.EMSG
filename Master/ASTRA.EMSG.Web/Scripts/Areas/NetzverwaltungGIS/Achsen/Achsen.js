var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

var overview = new GisMode.OverviewModel(index, '#AchsenGrid',
    {
        deleted: function () { return emsg.events.form.onAchseDeleted; },
        created: function () { return emsg.events.form.onAchseCreated; },
        selected: function () { return emsg.events.form.onAchseSelected; }
    });

index.getFilteredData = function () {
    return {
        AchsenName: $("#AchsenName").val()
    };
}

var detailsViewModel = new GisMode.Details();

var details = function () {

    var dirtyTracker = new GisMode.DirtyTracker('#acheEditFormDiv');

    emsg.events.map.onAchseCreated = achseCreated;
    emsg.events.map.onAchseSelected = achseSelected;
    emsg.form.executeCancel = onCloseForm;

    function achneChanged(geoData, geoLength) {
        $("#FeatureGeoJSONString").val(emsg.map.getGeoData()).change();
    }

    function achseCreated() {
        var formDiv = $('#acheEditFormDiv');
        $.ajax({
            url: formDiv.data('create-action'),
            type: 'POST',
            success: function (data) {
                formDiv.empty().append(data);
                index.disableTab(1);
                init();
                emsg.events.form.onDataLoaded();
                dirtyTracker.setIsDirty();
            }
        });
    }

    function achseSelected(id) {
        var formDiv = $('#acheEditFormDiv');
        $.ajax({
            url: formDiv.data('edit-action') + '/' + id,
            type: 'POST',
            success: function (data) {
                formDiv.empty().append(data);
                emsg.events.form.onDataLoaded();
                index.disableTab(1);
                init();
            }
        });
    }

    function setFeatureGeoJSONString(data) {
         if(!data.FeatureGeoJSONString)
                data.FeatureGeoJSONString = emsg.map.getGeoData();
    }

    function init() {
        detailsViewModel.showForm();

        common.subscribeOnButtonEvents($(".emsg-submit-button"), function (that) {
            showWarningDialog(that, "submit");
        });

        common.subscribeOnButtonEvents($(".emsg-apply-button"), function (that) {
            showWarningDialog(that, "apply");
        });
        
        hookOnCancel();
        hookOnDelete();
        emsg.events.map.onAchseChanged = achneChanged;

        if ($("#IsValid").val().toLowerCase() === 'true')
            dirtyTracker.resetIsDirty();
    }

    function showWarningDialog(that, action) {
        if ($(that).closest('form').attr('action').indexOf("Update") > -1) {
            $.ajax({
                url: $('#acheEditFormDiv').data('search-action'),
                data: { geoJson: emsg.map.getGeoData() },
                type: 'POST',
                success: function (data) {
                    if (data.length > 0) {
                        var tWindow = $("#WarningWindow").data('kendoWindow');
                        $("#WarningWindow").data('action', action);
                        tWindow.center().open();
                    } else {
                        if (action == "submit")
                            save();
                        else {
                            applySave();
                        }
                    }
                }
            });
        } else {
            if (action == "submit")
                save();
            else {
                applySave();
            }
        }
    }

    function hookOnCancel() {
        $(".emsg-cancel-button").click(function () {
            emsg.cancel();
        });
    }

    function applySave(callback) {
        common.applyForm($(".emsg-apply-button"), init, setFeatureGeoJSONString);
        if (callback)
            callback();
    }

    function save(callback) {
        common.submitForm($(".emsg-submit-button"), init, setFeatureGeoJSONString, function () {
            onCloseForm();
            emsg.events.form.onSaveSuccess();
            if (callback)
                callback();
        });
    }

    function hookOnDelete() {
        $(".emsg-delete-button").click(function () {
            var id = $('#acheEditFormDiv #Id').val();
            var comfirmationMessage = $(this).data('delete-comfirmation-message');
            if (!comfirmationMessage || confirm(comfirmationMessage)) {
                $.post($(this).data('delete-action') + '/' + id, function () {
                    onCloseForm();
                    emsg.events.form.onAchseDeleted(id);
                });
            }
        });
    }

    function onCloseForm() {
        $("#acheEditFormDiv input").off("change");
        $("#acheEditFormDiv").empty();
        emsg.events.map.onAchseChanged = function (geoData, geoLength) { };
        index.enableTab(1);
        detailsViewModel.hideForm();
        dirtyTracker.resetIsDirty();
    }

    return {
        save: save,
        applySave: applySave,
        trackChanges: dirtyTracker.trackChanges,
        onCloseForm: onCloseForm,
        dropDownChanged: dirtyTracker.setIsDirty
    };
}();

$(function () {
    overview.init();
    var div = $("#modificationWarning");
    div.append("<p>" + $("#acheEditFormDiv").data("modify-warning") + "</p>");
    div.append('<input class="ExportDialogButton k-button cancel" type="button" value="' + $("#acheEditFormDiv").data("button-cancel") + '"/>');
    div.append('<input class="ExportDialogButton k-button delete" type="button" value="' + $("#acheEditFormDiv").data("button-delete") + '"/>');
    div.append('<input class="ExportDialogButton k-button change" type="button" value="' + $("#acheEditFormDiv").data("button-change") + '"/>');

    $(".ExportDialogButton.cancel").click(function () {
        $("#WarningWindow").data('kendoWindow').close();
    });

    function executeSave() {
        if ($("#WarningWindow").data('action') == "submit") {
            details.save(function () {
                $("#WarningWindow").data('kendoWindow').close();
            });
        } else {
            details.applySave(function () {
                $("#WarningWindow").data('kendoWindow').close();
            });
        }
    }

    $(".ExportDialogButton.delete").click(function () {
        $("#ModificationAction").val("Delete");
        executeSave();
    });
    $(".ExportDialogButton.change").click(function () {
        $("#ModificationAction").val("Change");
        executeSave();
    });
    details.trackChanges();
});