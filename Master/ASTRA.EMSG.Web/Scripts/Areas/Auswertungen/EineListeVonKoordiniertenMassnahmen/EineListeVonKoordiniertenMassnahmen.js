$(function () {
    gisReport.init({
        gridDivId: '#EineListeVonKoordiniertenMassnahmen',
        getFormFilterParametersForMap: function () {
            return {
                Projektname: $("#Projektname").val(),
                Status: $("#Status").val(),
                AusfuehrungsanfangVon: $("#AusfuehrungsanfangVon").val(),
                AusfuehrungsanfangBis: $("#AusfuehrungsanfangBis").val()
            };
        },
        setFormParameters: function (parameter) {
            parameter.Projektname = $("#Projektname").val();
            parameter.Status = $("#Status").val();
            parameter.AusfuehrungsanfangVon = $("#AusfuehrungsanfangVon").val();
            parameter.AusfuehrungsanfangBis = $("#AusfuehrungsanfangBis").val();
        }
    }
    );
});