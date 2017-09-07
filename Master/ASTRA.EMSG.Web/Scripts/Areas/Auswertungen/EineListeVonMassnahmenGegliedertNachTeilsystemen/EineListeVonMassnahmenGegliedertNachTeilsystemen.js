$(function () {
    gisReport.init({
        gridDivId: '#EineListeVonMassnahmenGegliedertNachTeilsystemen',
        getFormFilterParametersForMap: function () {
            return {
                Projektname: $("#Projektname").val(),
                Status: $("#Status").val(),
                Dringlichkeit: $("#Dringlichkeit").val(),
                Teilsystem: $("#Teilsystem").val()
            };
        },
        setFormParameters: function (parameter) {
            parameter.Projektname = $("#Projektname").val();
            parameter.Status = $("#Status").val();
            parameter.Dringlichkeit = $("#Dringlichkeit").val();
            parameter.Teilsystem = $("#Teilsystem").val();
        }
    });
});