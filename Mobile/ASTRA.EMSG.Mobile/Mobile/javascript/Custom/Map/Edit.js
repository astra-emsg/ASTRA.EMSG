(function () {
    var Map = Emsg.Map;
    var $super = Map.prototype;

    Map.Edit = $.inherit(Map, {
        zoomToCurrentControl: true
    , currentinspectionroute: null

    , constructor: function (app, mode) {
        Map.apply(this, arguments);
        this.geoData = null;
        this.mode = new mode(this);

        var that = this;
        this.app.events.form.onSaveSuccess = function () { that.onSaveSuccess.apply(that, arguments); };
        this.app.events.form.onDataLoaded = function () { that.onDataLoaded.apply(that, arguments); };
    }

    , beforeBaseLayers: function () {
        $super.beforeBaseLayers.call(this);
        this.mode.beforeBaseLayers();
    }

    , afterBaseLayers: function () {
        $super.afterBaseLayers.call(this);
        this.mode.afterBaseLayers();
    }

    , beforeOverlays: function () {
        $super.beforeOverlays.call(this);
        this.mode.beforeOverlays();
    }
    , afterOverlays: function () {
        $super.afterOverlays.call(this);
        this.mode.afterOverlays();
    }

    , init: function (args) {
        $super.init.apply(this, arguments);

        var config = { projection: this.map.projection, maxExtent: this.map.maxExtent, displayInLayerSwitcher: false };
        config.styleMap = this.mode.parentStyleMap;
        this.parentLayer = new OpenLayers.Layer.Vector("Parent Layer", config);

        //config = { projection: config.projection, maxExtent: config.maxExtent, displayInLayerSwitcher: false };
        config.styleMap = Emsg.StyleMaps.MAIN_SEGMENT;
        this.editLayer = new OpenLayers.Layer.Vector("Edit Layer", config);
        this.dirs = new OpenLayers.Control.ShowDirection(this.editLayer, { autoActivate: true, drawBoth: true }); 
        this.endpoints = new OpenLayers.Control.ShowDirection(this.zabs, { autoActivate: true, drawBoth: true, styleMap: new OpenLayers.StyleMap({ 'fillColor': '#000000', 'strokeWidth': 3, 'strokeColor': '#000000', graphicName: "rect", rotation: "${angle}", pointRadius: 5 }) });
        this.endpointstrott = new OpenLayers.Control.ShowDirection(this.zabtrott, { autoActivate: true, drawBoth: true, styleMap: new OpenLayers.StyleMap({ 'fillColor': '#000000', 'strokeWidth': 3, 'strokeColor': '#000000', graphicName: "rect", rotation: "${angle}", pointRadius: 5 }) });

        this.parentLayer.events.register("beforefeatureadded", this, this.mode.convertParent);
        this.editLayer.events.register("beforefeatureadded", this, this.mode.convertChild);
        this.editLayer.events.register("featureadded", this, this.updateGeoData);
        this.editLayer.events.register("featuremodified", this, this.updateGeoData);
        this.editLayer.events.register("featureremoved", this, this.updateGeoData);
        this.editLayer.events.register("sketchmodified", this, this.updateGeoData);

        this.mode.init();
        this.map.addControls([this.mode.getPanel(), this.dirs, this.endpoints, this.endpointstrott]);
        this.map.addLayers([this.parentLayer, this.editLayer]);

        this.addControls();
        this.setCurrentInspektionsRoute(args.ActiveInspectionRouteId);
        this.events.initialized();
        this.mode.clear();
        this.mode.activateSelect();
    }
    , setCurrentInspektionsRoute: function (routeId) {
        if (routeId != this.currentinspectionroute) {
            this.currentinspectionroute = routeId;
            var strabsfeatures = this.strabs.features;
            var strabslength = strabsfeatures.length;

            for (var i = 0; i < strabslength; i++) {
                if (strabsfeatures[i].attributes["InspektionsrouteID"] == this.currentinspectionroute) {
                    //use layer style
                    strabsfeatures[i].style = null;
                } else {
                    strabsfeatures[i].style = { visibility: 'hidden' };
                }
            }
            this.strabs.redraw();
            var zabsfeatures = this.zabs.features
            var zabslength = zabsfeatures.length;
            var strabsid = null;
            var strabsfeature = null;
            for (var i = 0; i < zabslength; i++) {
                strabsid = zabsfeatures[i].attributes["StrassenabschnittsID"];
                strabsfeature = this.strabs.getFeatureByFid(strabsid);
                if (strabsfeature.attributes["InspektionsrouteID"] == this.currentinspectionroute) {
                    //use layer style
                    zabsfeatures[i].style = null;
                } else {
                    zabsfeatures[i].style = { visibility: 'hidden' };
                }
            }
            this.zabs.redraw();
            this.executeCancel();
            emsg.form.executeCancel();
            this.readExtent();
            this.zoomToDefault();

        }
    }
    , readExtent: function () {
        if (this.strabs && this.currentinspectionroute) {
            var vectors = this.strabs.getFeaturesByAttribute("InspektionsrouteID", this.currentinspectionroute);
            var bounds = undefined;
            var length = vectors.length;
            for (var i = 0; i < length; i++) {
                if (bounds) {
                    bounds.extend(vectors[i].geometry.getBounds());
                } else {
                    bounds = vectors[i].geometry.getBounds();
                }
            }
            if (bounds) {
                this.extent = bounds;
            } else {
                this.extent = this.map.getMaxExtent();
            }

        } else {
            this.extent = this.map.getMaxExtent();
        }
    }
    , onSaveSuccess: function () {
        this.editLayer.removeAllFeatures();
        this.parentLayer.removeAllFeatures();
        this.mode.clear();
    }

    , onCreated: function () { this.mode.create(); }
    , onSelected: function (id) {
        this.mode.open(id);
        this.mode.activateEdit();
    }
    , onDeleted: function (id) {
        this.deleteZustandsabschnitt(id);
        this.onSaveSuccess();
    }
    , onCanceled: function () { this.onSaveSuccess(); }
    , onDataLoaded: function () {
        if (this.mode.isDirty) {
            this.updateGeoData(true);
        };
        this.stopZoom();
        this.zoomToCurrent();
    }
    , updateGeoData: function (emitEvent) { this.mode.updateGeoData(emitEvent); }
    , getGeoData: function () { return this.mode.getGeoData(); }
    , getGeoLength: function () { return this.mode.getGeoLength(); }
    , prepareCancel: function () {
        // maybe highlight unsaved changes
        return { unsavedChanges: false, message: null };
    }
    , executeCancel: function () { this.onCanceled(); }
    });
})();
