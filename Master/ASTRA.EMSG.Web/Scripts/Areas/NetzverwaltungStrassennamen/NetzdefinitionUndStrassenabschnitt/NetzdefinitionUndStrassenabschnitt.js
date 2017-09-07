var strassenabschnittPage = (function() {

    var index = function() {
        function init() {
        }

        function openStrassenabschnittWindow(id) {
            var gird = getGrid();
            var editUrl = gird.data('editurl');
            var windowTitle = gird.data('editwindowtitle');
            openEditWindow(id, editUrl, 'strassenabschnittWindowDiv', 'StrassenabschnittWindow', windowTitle, edit.init);
        }

        function openÇreateStrassenabschnittWindow() {
            var gird = getGrid();
            var createUrl = gird.data('createurl');
            var windowTitle = gird.data('createwindowtitle');
            openGeneralWindow(createUrl, 'strassenabschnittWindowDiv', 'StrassenabschnittWindow', windowTitle, edit.init);
        }

        function openSplitStrassenabschnittWindow(id) {
            var gird = getGrid();
            var splitUrl = gird.data('split-url');
            openGeneralWindowWithPostedData(splitUrl, { id: id }, "splitStrassenabschnittFormDiv", "SplitStrassenabschnittWindow", null, split.init);
        }

        function getGrid() {
            return $('#StrassenabschnittenGrid');
        }

        return {
            init: init,
            openÇreateStrassenabschnittWindow: openÇreateStrassenabschnittWindow,
            openStrassenabschnittWindow: openStrassenabschnittWindow,
            openSplitStrassenabschnittWindow: openSplitStrassenabschnittWindow
        };
    }();

    var edit = function() {
        function init() {
            common.hookOnForm(init, closeEditWindow, function() {
                closeEditWindow();
            }, function() {
                $(".emsg-submit-button").data('submit-comfirmation-message', getComfirmationMessage());

                return isBelagChangedNotificationNeeded() || isBelagBelastungskategorieMismatchNotificationNeeded();
            }, getComfirmationMessage, getComfirmationMessage);
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
            var originalBelage = $('#StrassenabschnittWindow #OriginalBelag').val();
            var belage = $('#StrassenabschnittWindow #Belag').data('kendoDropDownList').text();
            var shouldCheckBelagChange = $('#StrassenabschnittWindow #ShouldCheckBelagChange').val();
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

        function closeEditWindow() {
            $('#StrassenabschnittWindow').data('kendoWindow').close();
        }

        return {
            init: init
        };
    }();

    var split = function() {
        function init() {
            common.hookOnForm(init, closeEditWindow, function() {
                $('#SplitStrassenabschnittWindow').data('kendoWindow').close();
            }, function() { return false; });
        }

        return {
            init: init
        };

        function closeEditWindow() {
            $('#SplitStrassenabschnittWindow').data('kendoWindow').close();
        }
    }();

    var strassenabschnittImport = function() {
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
            } else {
                errorMessage = e.XMLHttpRequest.responseText;
            }
            getResultDiv().empty().append(errorMessage);

            e.preventDefault();
        }

        function onSuccess(e) {
            $.post(getData("uploadResultUrl"), null, function(data) {
                $("#importFormDiv").empty().append(data);

                //If there was no error we got the ImportResult (Commit) site
                if ($("#importParameters").length > 0)
                    strassenabschnittCommit.init();
            });
        }

        function onSelect(e) {
            var files = e.files;
            $.each(files, function() {
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

    var strassenabschnittCommit = function() {

        function init() {
            common.hookOnButton(".emsg-commit-import-button", commitImport);
            common.hookOnButton(".emsg-cancel-import-button", cancelImport);
        }

        function commitImport(that) {
            var url = $(that).data("commit-url");
            $.ajax({
                url: url,
                type: 'POST',
                success: function(data) {
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

    return {
        index: index,
        edit: edit,
        split: split,
        strassenabschnittImport: strassenabschnittImport,
        strassenabschnittCommit: strassenabschnittCommit
    }
})();
$(function () {
    strassenabschnittPage.index.init();
});