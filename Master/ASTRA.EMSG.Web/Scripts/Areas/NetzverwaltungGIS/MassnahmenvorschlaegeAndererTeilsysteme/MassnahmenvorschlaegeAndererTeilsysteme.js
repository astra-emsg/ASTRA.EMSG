var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

var overview = new GisMode.OverviewModel(index, '#MassnahmenvorschlagTeilsystemeGrid',
    {
        deleted: function () { return emsg.events.form.onMassnahmenvorschlagTeilsystemeDeleted; },
        created: function () { return emsg.events.form.onMassnahmenvorschlagTeilsystemeCreated; },
        selected: function () { return emsg.events.form.onMassnahmenvorschlagTeilsystemeSelected; }
    });

index.getFilteredData = function () {
    return { ProjektnameFilter: $("#ProjektnameFilter").val() };
}

var detailsViewModel = new GisMode.Details();

var details = function () {

    var dirtyTracker = new GisMode.DirtyTracker('#massnahmenvorschlagTeilsystemeEditFormDiv');

    emsg.events.map.onMassnahmenvorschlagTeilsystemeCreated = massnahmenvorschlagTeilsystemeCreated;
    emsg.events.map.onMassnahmenvorschlagTeilsystemeSelected = massnahmenvorschlagTeilsystemeSelected;
    emsg.form.executeCancel = onCloseForm;

    function massnahmenvorschlagTeilsystemeChanged(geoData, geoLength) {
        $("#FeatureGeoJSONString").val(emsg.map.getGeoData()).change();
        $("#LaengeDiv").html(DecimalToShortGrouppedString(emsg.map.getGeoLength()));
        $("#Laenge").val(DecimalToShortString(emsg.map.getGeoLength())).change();
    }

    function massnahmenvorschlagTeilsystemeCreated() {
        var formDiv = $('#massnahmenvorschlagTeilsystemeEditFormDiv');
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

    function massnahmenvorschlagTeilsystemeSelected(id) {
        var formDiv = $('#massnahmenvorschlagTeilsystemeEditFormDiv');
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

    function init() {
        detailsViewModel.showForm();
        common.hookOnSubmit(init, function () {
            onCloseForm();
            emsg.events.form.onSaveSuccess();
        });
        common.hookOnApply(init);
        hookOnCancel();
        hookOnDelete();
        emsg.events.map.onMassnahmenvorschlagTeilsystemeChanged = massnahmenvorschlagTeilsystemeChanged;
        
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
            var id = $('#massnahmenvorschlagTeilsystemeEditFormDiv #Id').val();
            var comfirmationMessage = $(this).data('delete-comfirmation-message');
            if (!comfirmationMessage || confirm(comfirmationMessage)) {
                $.post($(this).data('delete-action') + '/' + id, function () {
                    onCloseForm();
                    emsg.events.form.onMassnahmenvorschlagTeilsystemeDeleted(id);
                });
            }
        });
    }

    function onCloseForm() {
        $("#massnahmenvorschlagTeilsystemeEditFormDiv input").off("change");
        $("#massnahmenvorschlagTeilsystemeEditFormDiv").empty();
        emsg.events.map.onMassnahmenvorschlagTeilsystemeChanged = function (geoData, geoLength) { };
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