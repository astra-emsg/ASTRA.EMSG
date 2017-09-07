var index = function () {

    function hookOnReportButtons() {
        $(".emsg-excel-report-button").click(generateReport);
        $(".emsg-pdf-report-button").click(generateReport);
    }

    function generateReport() {
        var generateReportUrl = $(this).data("generate-report-action");
        var getLastGeneratedReportUrl = $(this).data("get-last-generated-report-action");
        var belagsTyp = $(this).data("belags-typ");
        var outputFormat = $(this).data("output-format");

        var reportParameter = {
            outputFormat: outputFormat,
            belagsTyp: belagsTyp
        };

        $.post(generateReportUrl, reportParameter, function (data) { $.download(getLastGeneratedReportUrl, {}); });
    }

    function init() {
        hookOnReportButtons();
    }

    return {
        init: init
    };
} ();

$(index.init);