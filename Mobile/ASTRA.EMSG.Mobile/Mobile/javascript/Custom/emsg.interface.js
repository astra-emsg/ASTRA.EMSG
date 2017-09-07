/* FormInterface */
Emsg.FormInterface = function () { };
Emsg.FormInterface.prototype.prepareCancel = function () {
  // maybe highlight unsaved changes
  return { unsavedChanges: false, message: null };
};
Emsg.FormInterface.prototype.executeCancel = function () { };
Emsg.FormInterface.prototype.getFilterParameters = function () { return {}; };
Emsg.FormInterface.prototype.setFormParameters = function (parameter) { return {}; };
Emsg.FormInterface.prototype.refreshGrid = function () { return {}; };

/* MapInterface */
Emsg.MapInterface = function () { };
Emsg.MapInterface.prototype.init = function () { };
Emsg.MapInterface.prototype.getGeoData = function () { return null; };
Emsg.MapInterface.prototype.getGeoLength = function () { return null; };
Emsg.MapInterface.prototype.prepareCancel = function () {
    // maybe highlight unsaved changes
    return {unsavedChanges: false, message : null};
};
Emsg.MapInterface.prototype.executeCancel = function () { };

/* AnalysisMapInterface */
Emsg.AnalysisMapInterface = function () { };
Emsg.AnalysisMapInterface.prototype.init = function () { };
Emsg.AnalysisMapInterface.prototype.getFilterParameters = function () { return {}; };
Emsg.AnalysisMapInterface.prototype.updateSetting = function (settings) {
  // name value pairs wher the name/key is the column available in arcgis
  // settings = {"ABSCHLUSSJAHR" : "2008", "BELASTUNGSKATEGORI": "IV"}
};

/* InspectionMapInterface */
Emsg.InspectionMapInterface = function () { };
Emsg.InspectionMapInterface.prototype.init = function () { };
Emsg.InspectionMapInterface.prototype.clear = function () { };
Emsg.InspectionMapInterface.prototype.load = function (inspectionRouteId) { };
Emsg.InspectionMapInterface.prototype.redraw = function () { };


/* Events.Form */
Emsg.Events = {};
Emsg.Events.Form = function () { };
Emsg.Events.Form.prototype.onSaveSuccess = function () { };
Emsg.Events.Form.prototype.onDataLoaded = function () { };

//Strassenabschnitt
Emsg.Events.Form.prototype.onStrassenabschnittCreated = function () { }; // Scroll/zoom the current Mandant center into view on the MAP
Emsg.Events.Form.prototype.onStrassenabschnittSelected = function (strassenabschnittId) { }; // Scroll/zoom the selected Strassenabschnitt into view on the MAP
Emsg.Events.Form.prototype.onStrassenabschnittDeleted = function (strassenabschnittId) { };
Emsg.Events.Form.prototype.onStrassenabschnittDeselected = function (strassenabschnittId) { };
Emsg.Events.Form.prototype.onStrassenabschnittSplitted = function(strassenabschnittId, count) { };

//Zustandsabschnitt
Emsg.Events.Form.prototype.onZustandsabschnittCreated = function (strassenabschnittId) { }; // Activate the create tool on the MAP
Emsg.Events.Form.prototype.onZustandsabschnittSelected = function (zustandsabschnittId) { }; // Scroll/zoom the selected Zustandsabschnitt into view on the MAP
Emsg.Events.Form.prototype.onZustandsabschnittDeleted = function (zustandsabschnittId) { };

//Inspektionsroute
Emsg.Events.Form.prototype.onInspektionsrouteCreated = function () { };
Emsg.Events.Form.prototype.onInspektionsrouteSelected = function (inspektionsrouteId) { };
Emsg.Events.Form.prototype.onInspektionsrouteDeleted = function (inspektionsrouteId) { };

//MassnahmenDerTeilsysteme
Emsg.Events.Form.prototype.onMassnahmenvorschlagTeilsystemeCreated = function () { };
Emsg.Events.Form.prototype.onMassnahmenvorschlagTeilsystemeSelected = function (massnahmenvorschlagTeilsystemeId) { };
Emsg.Events.Form.prototype.onMassnahmenvorschlagTeilsystemeDeleted = function (massnahmenvorschlagTeilsystemeId) { };

//KoordinierteMassnahme
Emsg.Events.Form.prototype.onKoordinierteMassnahmeCreated = function () { };
Emsg.Events.Form.prototype.onKoordinierteMassnahmeSelected = function (koordinierteMassnahmeId) { };
Emsg.Events.Form.prototype.onKoordinierteMassnahmeDeleted = function (koordinierteMassnahmeId) { };

//RealisierteMassnahme
Emsg.Events.Form.prototype.onRealisierteMassnahmeCreated = function () { };
Emsg.Events.Form.prototype.onRealisierteMassnahmeSelected = function (realisierteMassnahmeId) { };
Emsg.Events.Form.prototype.onRealisierteMassnahmeDeleted = function (realisierteMassnahmeId) { };

/* Events.Map */
Emsg.Events.Map = function () { };
Emsg.Events.Map.prototype.initialized = function () { };
Emsg.Events.Map.prototype.onDataLoaded = function () { };

//Strassenabschnitt
Emsg.Events.Map.prototype.onStrassenabschnittCreated = function () { };
Emsg.Events.Map.prototype.onStrassenabschnittChanged = function (geoData, geoLength) { };
Emsg.Events.Map.prototype.onStrassenabschnittSelected = function (strassenabschnittId) { };

//Zustandsabschnitt
Emsg.Events.Map.prototype.onZustandsabschnittCreated = function (strassenabschnittId) { };
Emsg.Events.Map.prototype.onZustandsabschnittChanged = function (geoData, geoLength) { };
Emsg.Events.Map.prototype.onZustandsabschnittSelected = function (zustandsabschnittId) { };
// when trying to put data on a different Strassenabschnitt as the currently used one
Emsg.Events.Map.prototype.onZustandsabschnittDifferentParent = function () { };

//Inspektionsroute
Emsg.Events.Map.prototype.onStrassenabschnittDeselected = function (strassenabschnittId) { };
Emsg.Events.Map.prototype.onInspektionsrouteCreated = function () { };
Emsg.Events.Map.prototype.onInspektionsrouteSelected = function (inspektionsrouteId) { };

//MassnahmenvorschlagTeilsysteme
Emsg.Events.Map.prototype.onMassnahmenvorschlagTeilsystemeCreated = function () { };
Emsg.Events.Map.prototype.onMassnahmenvorschlagTeilsystemeSelected = function (massnahmenvorschlagTeilsystemeId) { };
Emsg.Events.Map.prototype.onMassnahmenvorschlagTeilsystemeChanged = function (geoData, geoLength) { };

//KoordinierteMassnahme
Emsg.Events.Map.prototype.onKoordinierteMassnahmeCreated = function () { };
Emsg.Events.Map.prototype.onKoordinierteMassnahmeSelected = function (koordinierteMassnahmeId) { };
Emsg.Events.Map.prototype.onKoordinierteMassnahmeChanged = function (geoData, geoLength) { };

//RealisierteMassnahme
Emsg.Events.Map.prototype.onRealisierteMassnahmeCreated = function () { };
Emsg.Events.Map.prototype.onRealisierteMassnahmeSelected = function (realisierteMassnahmeId) { };
Emsg.Events.Map.prototype.onRealisierteMassnahmeChanged = function (geoData, geoLength) { };

//Filtering
Emsg.Events.Map.prototype.onFilterChanged = function() {};
Emsg.Events.Form.prototype.onFilterChanged = function () { return {}; };

//GisReporting
Emsg.FormInterface.prototype.generateGisMapPdfReport = function () { };
