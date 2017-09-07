$(function () {
    gisReport.init({
        gridDivId: '#MassnahmenvorschlagProZustandsabschnitt',
        getFormFilterParametersForMap: function () {
            return {
                ErfassungsPeriodId: convertGuidToHexa($("#ErfassungsPeriod").val()),
                Eigentuemer: $("#Eigentuemer").val(),
                Dringlichkeit: $("#Dringlichkeit").val(),
                Strassenname: $("#Strassenname").val(),
                ZustandsindexVon: $("#ZustandsindexVon").val(),
                ZustandsindexBis: $("#ZustandsindexBis").val(),
                Ortsbezeichnung: $("#Ortsbezeichnung").val()
            };
        },
        setFormParameters: function (parameter) {
            parameter.Eigentuemer = $("#Eigentuemer").val();
            parameter.Dringlichkeit = $("#Dringlichkeit").val();
            parameter.Strassenname = $("#Strassenname").val();
            parameter.ZustandsindexVon = $("#ZustandsindexVon").val();
            parameter.ZustandsindexBis = $("#ZustandsindexBis").val();
            parameter.Ortsbezeichnung = $("#Ortsbezeichnung").val();
        }
    }
    );
});