var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

var overview = new GisMode.OverviewModel(index, '#RealisierteMassnahmenGrid',
    {
        deleted: function () { return emsg.events.form.onRealisierteMassnahmeDeleted; },
        created: function () { return emsg.events.form.onRealisierteMassnahmeCreated; },
        selected: function () { return emsg.events.form.onRealisierteMassnahmeSelected; }
    },
    function () {
        return { ProjektnameFilter: $("#ProjektnameFilter").val() };
    });

index.getFilteredData = function () {
  return { ProjektnameFilter: $("#ProjektnameFilter").val() };
}

var detailsViewModel = new GisMode.Details();

var details = function () {

    var dirtyTracker = new GisMode.DirtyTracker('#realisierteMassnahmeEditFormDiv');

    emsg.events.map.onRealisierteMassnahmeCreated = realisierteMassnahmeCreated;
    emsg.events.map.onRealisierteMassnahmeSelected = realisierteMassnahmeSelected;
    emsg.form.executeCancel = onCloseForm;

    function massnahmenvorschlagTeilsystemeChanged(geoData, geoLength) {
        $("#FeatureGeoJSONString").val(emsg.map.getGeoData()).change();
        $("#LaengeDiv").html(DecimalToShortGrouppedString(emsg.map.getGeoLength()));
        $("#Laenge").val(DecimalToShortString(emsg.map.getGeoLength())).change();
    }

    function realisierteMassnahmeCreated() {
        var formDiv = $('#realisierteMassnahmeEditFormDiv');
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

    function realisierteMassnahmeSelected(realisierteMassnahmeId) {
        var formDiv = $('#realisierteMassnahmeEditFormDiv');
        $.ajax({
            url: formDiv.data('edit-action') + '/' + realisierteMassnahmeId,
            type: 'POST',
            success: function (data) {
                formDiv.empty().append(data);
                emsg.events.form.onDataLoaded();
                index.disableTab(1);
                init();
            }
        });
    }

    function init() {
        detailsViewModel.showForm();
        common.hookOnSubmit(init, function () {
            onCloseForm();
            alert($('#realisierteMassnahmeEditFormDiv').data('saved-warning'));
            emsg.events.form.onSaveSuccess();
        });
        common.hookOnApply(init);
        hookOnCancel();
        hookOnDelete();
        emsg.events.map.onRealisierteMassnahmeChanged = massnahmenvorschlagTeilsystemeChanged;

        if ($("#IsValid").val().toLowerCase() === 'true')
            dirtyTracker.resetIsDirty();
    }

    function hookOnCancel() {
        $(".emsg-cancel-button").click(function () {
            emsg.cancel();
        });
    }

    function hookOnDelete() {
        $(".emsg-delete-button").click(function () {
            var id = $('#realisierteMassnahmeEditFormDiv #Id').val();
            var comfirmationMessage = $(this).data('delete-comfirmation-message');
            if (!comfirmationMessage || confirm(comfirmationMessage)) {
                $.post($(this).data('delete-action') + '/' + id, function () {
                    onCloseForm();
                    emsg.events.form.onRealisierteMassnahmeDeleted(id);
                });
            }
        });
    }

    function onCloseForm() {
        $("#realisierteMassnahmeEditFormDiv").empty();
        emsg.events.map.onRealisierteMassnahmeChanged = function (geoData, geoLength) { };
        index.enableTab(1);
        detailsViewModel.hideForm();
        dirtyTracker.resetIsDirty();
    }

    return {
        trackChanges: dirtyTracker.trackChanges,
        onCloseForm: onCloseForm,
        dropDownChanged: dirtyTracker.setIsDirty
    };
} ();

$(function () {
    overview.init();
    details.trackChanges();
});