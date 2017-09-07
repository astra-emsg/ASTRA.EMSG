var preview = function () {
    var canGenerateReportField = false;

    function onRequestEnd(e) {
        canGenerateReportField = e.response.CanGenerateReport;
    }

    function canGenerateReport() {
        if (canGenerateReportField)
            return true;

        var msg = $("#AusgefuellteErfassungsformulareFuerOberflaechenschaeden").data('no-detallierte-schadenerfassungsformular-message');
        alert(msg);
        return false;
    }

    return {
        onRequestEnd: onRequestEnd,
        canGenerateReport: canGenerateReport
    };
} ();