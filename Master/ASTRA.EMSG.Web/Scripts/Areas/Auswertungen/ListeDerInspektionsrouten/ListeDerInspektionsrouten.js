$(function () {
    gisReport.init({
        gridDivId: '#ListeDerInspektionsroutenGrid',
        getFormFilterParametersForMap: function () {
            return {
                Eigentuemer: $("#Eigentuemer").val(),
                Inspektionsroutename: $("#Inspektionsroutename").val(),
                Strassenname: $("#Strassenname").val(),
                InspektionsrouteInInspektionBei: $("#InspektionsrouteInInspektionBei").val(),
                InspektionsrouteInInspektionBisVon: $("#InspektionsrouteInInspektionBisVon").val(),
                InspektionsrouteInInspektionBisBis: $("#InspektionsrouteInInspektionBisBis").val(),

                ErfassungsPeriodId: convertGuidToHexa($("#ErfassungsPeriod").val())
            };
        },
        setFormParameters: function (parameter) {
            parameter.Eigentuemer = $("#Eigentuemer").val();
            parameter.Inspektionsroutename = $("#Inspektionsroutename").val();
            parameter.Strassenname = $("#Strassenname").val();
            parameter.InspektionsrouteInInspektionBei = $("#InspektionsrouteInInspektionBei").val();
            parameter.InspektionsrouteInInspektionBisVon = $("#InspektionsrouteInInspektionBisVon").val();
            parameter.InspektionsrouteInInspektionBisBis = $("#InspektionsrouteInInspektionBisBis").val();
        }
    }
    );
});