function ZustandsabschnittCancelled(sender, args) {
    emsg.form.executeCancel();
    emsg.map.executeCancel();
}

function ZustandsabschnittSaved(sender, args) {
    var argsobject = JSON.parse(args);
    emsg.map.updateZustandsabschnitt(argsobject.Id, argsobject.Zustandsindex, argsobject.GeoJson);
    emsg.map.onSaveSuccess();
}

function ZustandsabschnittApplySave(sender, args) {
    var argsobject = JSON.parse(args);
    emsg.map.updateZustandsabschnitt(argsobject.Id, argsobject.Zustandsindex, argsobject.GeoJson);
    emsg.map.mode.isDirty = false;
}


function ZustandsabschnittDeleted(sender, args) {
    var argsobject = JSON.parse(args);    
    emsg.events.form.onZustandsabschnittDeleted(argsobject.Id);
}



function GettingFormHasChanges(sender, args) {
    var argsobject = JSON.parse(args);
    emsg.map.mode.isDirty = args.HasFormChanges;
}

function InspektionsRouteChanged(sender, args) {
    var argsobject = JSON.parse(args);
    if (window.emsg) {        
        window.emsg.map.setCurrentInspektionsRoute(argsobject.inspektionsRouteId);
    }
}

function DataChanged(sender, args) {

    var argsobject = JSON.parse(args);
    if (window.emsg) {
        window.emsg.destroy();
    }
    window.emsg = new Emsg.App();

    window.emsg.localization = argsobject.MobileLocalization;    
    window.emsg.map = new Emsg.Map.Edit(window.emsg, Emsg.MapMode.ConditionIndex);    
    window.emsg.map.init(argsobject);
    window.emsg.map.readExtent();
    window.emsg.map.zoomToDefault();
    window.emsg.map.setCurrentInspektionsRoute(argsobject.ActiveInspectionRouteId);

    window.emsg.form.prepareCancel = function () {
        var isDirty = window.external.HasFormChanges();
        return { unsavedChanges: isDirty, message: argsobject.MobileLocalization.DiscardChanges };
    };
    window.emsg.events.map.onZustandsabschnittSelected = function (fid) {
        window.external.CallZustandsabschnittSelected(fid);
        window.emsg.events.map.onZustandsabschnittChanged = function (geoData, geoLength) { window.external.CallZustandsabschnittChanged(geoData, geoLength); };
    };
    window.emsg.form.executeCancel = function () {
        emsg.events.map.onZustandsabschnittChanged = function (geoData, geoLength) { };
        window.external.CallZustandsabschnittCancelled();
    };
    window.emsg.events.map.onZustandsabschnittCreated = function (strabsid) {
        window.external.CallZustandsabschnittCreated(strabsid);
        window.emsg.events.map.onZustandsabschnittChanged = function (geoData, geoLength) { window.external.CallZustandsabschnittChanged(geoData, geoLength); };
        window.emsg.map.updateGeoData();
        window.emsg.events.map.onZustandsabschnittChanged(emsg.map.getGeoData(), emsg.map.getGeoLength());
    }

}
function onSelect(object) {
    
    var hasChanges = window.external.HasFormChanges();
    if (!hasChanges || confirm("Etwaige Änderungen werden verworfen, wollen Sie fortfahren?")) {
        editlayer.removeAllFeatures();
        var clone = object.clone();
        clone.style = undefined;
        editlayer.addFeatures([clone]);
        window.external.CallZustandsabschnittSelected(object.fid);
    }
}
function addControls(map) {
    var OL = OpenLayers;
    var Control = OL.Control;
    var Path = OL.Handler.Path;
    var Polygon = OL.Handler.Polygon;

    var nav, ctrl = [], panel = new Control.Panel({ displayClass: "panelTopLeft", allowDepress: true });
    tool_nav_ = new Control({ displayClass: "DummyPan", title: "Pan" });
    tool_zoom_ = new Control.ZoomBox({ title: "Zoom" });
    ctrl.push(this.tool_nav_);
    ctrl.push(this.tool_zoom_);
   
    var history = new Control.NavigationHistory({
        previousOptions: { displayClass: "Prev" }
     , nextOptions: { displayClass: "Next" }
    });
    ctrl.push(history.previous);
    ctrl.push(history.next);

    ctrl.push(new Control.MeasureEx(Path, { displayClass: "MeasureLine", title: "Masure Line" }));
    ctrl.push(new Control.MeasureEx(Polygon, { displayClass: "MeasureArea", title: "Measure Area", numDigits: 0 }));

    panel.addControls(ctrl);
    ctrl = [];
    ctrl.push(new Control.Navigation());
    ctrl.push(history);
    ctrl.push(panel);
    ctrl.push(new Control.PanZoomBar());
    ctrl.push(new Control.ScaleLine());
  

    var str_format = "X: ${lon}, Y: ${lat}";
    var format = function (pos) {
        var r = Emsg.Util.round;
        pos = { lon: r(pos.lon, 1), lat: r(pos.lat, 1) };
        return OL.String.format(str_format, pos);
    };
    ctrl.push(new Control.MousePosition({ formatOutput: format }));

    map.addControls(ctrl);
    panel.activateControl(tool_nav_);
}
 



