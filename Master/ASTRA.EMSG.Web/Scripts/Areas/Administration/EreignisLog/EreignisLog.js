var index = function () {

    function clearLog() {
        var confirmationMesage = $('#EreignisLogGrid').data("confirmation-mesage");
        var clearlogUrl = $('#EreignisLogGrid').data("clearlog-url");

        if (confirm(confirmationMesage)) {
            $.ajax({
                url: clearlogUrl,
                type: 'POST',
                success: function (data) {
                    refreshGrid();
                }
            });
        }
    }
    
    function refreshGrid() {
        common.refreshGrid('#EreignisLogGrid');
    }

    function getFilteredData(e) {
        return {
            EreignisTyp: $("#EreignisTyp").val(),
            Benutzer: $("#Benutzer").val(),
            Mandant: $("#Mandant").val(),
            ZeitVon: $("#ZeitVon").val(),
            ZeitBis: $("#ZeitBis").val()
        };
    }

    return {
        getFilteredData: getFilteredData,
        clearLog: clearLog,
        refreshGrid: refreshGrid
    };

} ();