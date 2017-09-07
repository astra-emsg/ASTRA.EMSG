$(function () {
    gisReport.init({
        gridDivId: '#StrassenabschnitteListe',
        getFormFilterParametersForMap: function () {
            return {
                Eigentuemer: $("#Eigentuemer").val(),
                Belastungskategorie: convertGuidToHexa($("#Belastungskategorie").val()),
                ErfassungsPeriodId: convertGuidToHexa($("#ErfassungsPeriod").val()),
                Ortsbezeichnung: $("#Ortsbezeichnung").val()
            };
        },
        setFormParameters: function (parameter) {
            parameter.Eigentuemer = $("#Eigentuemer").val();
            parameter.Belastungskategorie = $("#Belastungskategorie").val();
            parameter.Ortsbezeichnung = $("#Ortsbezeichnung").val();
        }
    }
    );
});