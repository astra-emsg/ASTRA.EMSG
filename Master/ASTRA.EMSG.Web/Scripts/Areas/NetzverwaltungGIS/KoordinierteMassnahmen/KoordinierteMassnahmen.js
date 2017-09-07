 var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

 var overview = new GisMode.OverviewModel(index, '#KoordinierteMassnahmenGrid',
    {
        deleted: function () { return emsg.events.form.onKoordinierteMassnahmeDeleted; },
        created: function () { return emsg.events.form.onKoordinierteMassnahmeCreated; },
        selected: function () { return emsg.events.form.onKoordinierteMassnahmeSelected; }
    });

 index.getFilteredData = function () {
     return { ProjektnameFilter: $("#ProjektnameFilter").val() };
 }

var detailsViewModel = new GisMode.Details();
    
var details = function () {

        var dirtyTracker = new GisMode.DirtyTracker('#koordinierteMassnahmeEditFormDiv');

        emsg.events.map.onKoordinierteMassnahmeCreated = koordinierteMassnahmeCreated;
        emsg.events.map.onKoordinierteMassnahmeSelected = koordinierteMassnahmeSelected;
        emsg.form.executeCancel = onCloseForm;

        function massnahmenvorschlagTeilsystemeChanged(geoData, geoLength) {
            $("#FeatureGeoJSONString").val(emsg.map.getGeoData()).change();
            $("#LaengeDiv").html(DecimalToShortGrouppedString(emsg.map.getGeoLength()));
            $("#Laenge").val(DecimalToShortString(emsg.map.getGeoLength())).change();
        }

        function koordinierteMassnahmeCreated() {
            var formDiv = $('#koordinierteMassnahmeEditFormDiv');
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

        function koordinierteMassnahmeSelected(koordinierteMassnahmeId) {
            var formDiv = $('#koordinierteMassnahmeEditFormDiv');
            $.ajax({
                url: formDiv.data('edit-action') + '/' + koordinierteMassnahmeId,
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
            emsg.events.map.onKoordinierteMassnahmeChanged = massnahmenvorschlagTeilsystemeChanged;
            
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
                var id = $('#koordinierteMassnahmeEditFormDiv #Id').val();
                var comfirmationMessage = $(this).data('delete-comfirmation-message');
                if (!comfirmationMessage || confirm(comfirmationMessage)) {
                    $.post($(this).data('delete-action') + '/' + id, function () {
                        onCloseForm();
                        emsg.events.form.onKoordinierteMassnahmeDeleted(id);
                    });
                }
            });
        }

        function onCloseForm() {
            $("#koordinierteMassnahmeEditFormDiv").empty();
            emsg.events.map.onKoordinierteMassnahmeChanged = function (geoData, geoLength) { };
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