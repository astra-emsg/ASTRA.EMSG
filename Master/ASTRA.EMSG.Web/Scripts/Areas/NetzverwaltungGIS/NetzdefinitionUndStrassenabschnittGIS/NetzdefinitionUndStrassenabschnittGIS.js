var index = new GisMode.MapOverviewSwitcher("#TabStrip", function () { overview.refreshGrid(); }, function () { window.setTimeout("emsg.map.updateSize()", 410); });

var overview = new GisMode.OverviewModel(index, '#StrassenabschnittenGrid',
    {
        deleted: function () { return emsg.events.form.onStrassenabschnittDeleted; },
        created: function () { return emsg.events.form.onStrassenabschnittCreated; },
        selected: function () { return emsg.events.form.onStrassenabschnittSelected; }
    },

    function (e) {
        if (e.dataItem.BelongsToInspektionsroute) {
            alert($("#StrassenabschnittenGrid").data("strassenabschnitt-in-inspektionsroute-warning"));
            e.preventDefault();
        }
    }
);

index.getFilteredData = function () {
    return {
        strassennameFilter: $("#StrassennameFilter").val(), ortsbezeichnung: $("#Ortsbezeichnung").val()
    };
}

var detailsViewModel = new GisMode.Details();

var details = function () {
    
        var dirtyTracker = new GisMode.DirtyTracker('#strassenabschnittEditFormDiv');

        emsg.events.map.onStrassenabschnittCreated = strassenabschnittCreated;
        emsg.events.map.onStrassenabschnittSelected = strassenabschnittSelected;
        emsg.form.executeCancel = onCloseForm;

        /// StrassenabschnittGIS ///

        function strassenabschnittCreated() {
            var formDiv = $('#strassenabschnittEditFormDiv');
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

        function strassenabschnittSelected(strassenabschnittId) {
            var formDiv = $('#strassenabschnittEditFormDiv');
            $.ajax({
                url: formDiv.data('edit-action') + '/' + strassenabschnittId,
                type: 'POST',
                success: function (data) {
                    formDiv.empty().append(data);
                    emsg.events.form.onDataLoaded();
                    index.disableTab(1);
                    init();
                }
            });
        }

        function isBrowserIsIE() {
            // Internet Explorer 6-11
            return /*@cc_on!@*/false || !!document.documentMode;
        };

        function refreshStrassenabschnittName() {

            var geoNames = emsg.map.getGeoNames();
            var strassennamen = "";
            for (var i = 0; i < geoNames.length; i++) {
                if (i == 0) {
                    strassennamen += geoNames[i].AchsenName;
                }
                else
                {
                    strassennamen += ", " + geoNames[i].AchsenName;
                }
            }
            $("#Strassenname").val(strassennamen);
        return false;
        }


    function strassenabschnittChanged(geoData, geoLength, achsenNames) {

            $("#FeatureGeoJSONString").val(emsg.map.getGeoData()).change();
            $("#LaengeDiv").html(DecimalToShortGrouppedString(emsg.map.getGeoLength()));
            $("#Laenge").val(DecimalToShortString(emsg.map.getGeoLength())).change();

            if ($("#Strassenname").val().length == 0)
            {
                var geoNames = emsg.map.getGeoNames();
                var strassennamen = "";
                for (var i = 0; i < geoNames.length; i++) {
                    if (i == 0) {
                        strassennamen += geoNames[i].AchsenName;
                    }
                    else {
                        strassennamen += ", " + geoNames[i].AchsenName;
                    }
                }
                $("#Strassenname").val(strassennamen).change();
            }
        }

        function init() {
            detailsViewModel.showForm();

            emsg.events.map.onStrassenabschnittChanged = strassenabschnittChanged;
            
            common.hookOnSubmit(init, function () {
                onCloseForm();
                emsg.events.form.onSaveSuccess();
            }, null, needsComfirmation, getComfirmationMessage);
            common.hookOnApply(function () {
                strassenabschnittChanged();
                init();
            }, null, needsComfirmation, getComfirmationMessage);
            hookOnCancel();
            hookOnDelete();
            hookOnSplit();
            
            if ($("#IsValid").val().toLowerCase() === 'true')
                dirtyTracker.resetIsDirty();
        }

        function hookOnCancel() {
            $(".emsg-cancel-button").click(function () {
                emsg.cancel();
            });
        }

        function hookOnSplit() {
            $(".emsg-split-button").click(function () {
                var form = $('#strassenabschnittEditForm');
                var id = form.find('#Id').val();
                var splitUrl = form.data('split-url');
                openGeneralWindowWithPostedData(splitUrl, { id: id }, "splitStrassenabschnittGISFormDiv", "SplitStrassenabschnittGISWindow", null, split.init);
            });
        }

        function hookOnDelete() {
            $(".emsg-delete-button").click(function () {
                var belongsToInspektionsroute = $('#strassenabschnittEditFormDiv #BelongsToInspektionsroute').val().toLowerCase() === 'true';
                if (belongsToInspektionsroute) {
                    alert($('#strassenabschnittEditForm').data('inspektionsroute-notification'));
                    return;
                }
                var strassenabschnittId = $('#strassenabschnittEditFormDiv #Id').val();
                var comfirmationMessage = $(this).data('delete-comfirmation-message');
                if (!comfirmationMessage || confirm(comfirmationMessage)) {
                    $.post($(this).data('delete-action') + '/' + strassenabschnittId, function () {
                        onCloseForm();
                        emsg.events.form.onStrassenabschnittDeleted(strassenabschnittId);
                    });
                }
            });
        }

        function onCloseForm() {
            $("#strassenabschnittEditFormDiv").empty();
            emsg.events.map.onStrassenabschnittChanged = function (geoData, geoLength) { };
            index.enableTab(1);
            detailsViewModel.hideForm();
            dirtyTracker.resetIsDirty();
        }
        
        function needsComfirmation() {
            $(".emsg-submit-button").data('submit-comfirmation-message', getComfirmationMessage());

            return isBelagChangedNotificationNeeded() || isBelagBelastungskategorieMismatchNotificationNeeded();
        }

        function getComfirmationMessage() {
            var showBelagChangedNotificationNeeded = isBelagChangedNotificationNeeded();
            var showBelagBelastungskategorieMismatchNotificationNeeded = isBelagBelastungskategorieMismatchNotificationNeeded();

            var notificationMessage = "";

            if (showBelagChangedNotificationNeeded)
                notificationMessage += $("#notificationMessages").data('belag-change-confirmation');

            if (showBelagBelastungskategorieMismatchNotificationNeeded)
                notificationMessage += "\n" + $("#notificationMessages").data('belag-belastungskategorie-mismatch-confirmation');

            return notificationMessage;
        }

        function isBelagChangedNotificationNeeded() {
            var originalBelage = $('#OriginalBelag').val();
            var belage = $('#Belag').data('kendoDropDownList').text();
            var shouldCheckBelagChange = $('#ShouldCheckBelagChange').val();
            if (shouldCheckBelagChange != 'True')
                return false;

            return originalBelage !== belage;
        }

        function isBelagBelastungskategorieMismatchNotificationNeeded() {
            var selectedBelastungskategorie = $("#Belastungskategorie").val();
            var selectedBelagsTyp = $("#Belag").data('kendoDropDownList').text();

            if (selectedBelastungskategorie != null && selectedBelastungskategorie != "") {
                return -1 >= $.inArray(selectedBelagsTyp, belastungskategorienDictionary[selectedBelastungskategorie].AllowedBelagList);
            }

            return false;
        }

        return {
            trackChanges: dirtyTracker.trackChanges,
            onCloseForm: onCloseForm,
            dropDownChanged: dirtyTracker.setIsDirty,
            refreshStrassenabschnittName: refreshStrassenabschnittName,
            isBrowserIsIE: isBrowserIsIE
        };
    } ();

var split = function () {
    function init() {
        common.hookOnForm(init, closeEditWindow, function () {
            emsg.events.form.onStrassenabschnittSplitted($('#StrassenabschnittId').val(), $('#Count').val());
            emsg.events.form.onSaveSuccess();
            details.onCloseForm();
            closeEditWindow();
        });
    }

    function onClose() {

    }


    return {
        init: init,
        onClose: onClose
    };

    function closeEditWindow() {
        $('#SplitStrassenabschnittGISWindow').data('kendoWindow').close();
    }
}();

$(function () {
    details.trackChanges();
});