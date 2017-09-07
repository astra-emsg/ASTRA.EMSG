(function () {
    var OL = OpenLayers;
    var Bounds = OL.Bounds;
    var Control = OL.Control;
    var Path = OL.Handler.Path;
    var Polygon = OL.Handler.Polygon;
    var Layer = OL.Layer;
    var Map = OL.Map;
    var Projection = OL.Projection;

    Emsg.Map = $.inherit(Object, {
        /**
        * @property {Boolean} true to add legend control
        */
        legend: false
        /**
        * @property {Boolean} true to add pdf download control for reports only
        */
   , pdfDownload: false
        /**
        * @property {Boolean} true to add zoom to current feature control
        */
   , zoomToCurrentControl: false
        /**
        * @property {OpenLAyers.Control} control to zoom to current feature
        */
   , zoom_current: null

        /**
        * @property {Emsg.App} the application this map runs in
        */
   , app: null

        /**
        * @property {Emsg.Events.Map} map event callback functions
        */
   , events: null

        /**
        * @property {OpenLayers.Bounds} the default extent for this mandant
        */
   , extent: null

        /**
        * @property {OpenLayers.Control.Panel} top left panel
        */
   , panel_: null
        /**
        *@property {OpenLayers.Control.ScaleLine} scale line in bottom left corner
        */
   , scale_line: null

        /**
        * some arcgis layers
        */
   , axis: null
   , strabs: null
   , zabs: null
   , measure: null
   , realisiert: null
   , suggestion: null
   , route: null
   , konflikte: null

   , constructor: function (app) {
       this.app = app;
       this.events = app.events.map;
   }

   , createLayer: function (className, key, textKey, params, options) {
       var layerClass = Layer[className];
       var c = this.app.config;
       var name = "";
       if (c.has("TEXT.LAYER." + textKey)) {
           name = c.get("TEXT.LAYER." + textKey);
       };
       if (c.has("TEXT.LAYER." + key + "." + textKey)) {
           name = c.get("TEXT.LAYER." + key + "." + textKey);
       };
       if (c.has("WMS." + key + ".PARAMS")) {
           var cfg_params = c.get("WMS." + key + ".PARAMS") || {};
           params = $.extend(cfg_params, params);
       };
       var url = c.get("WMS." + key + ".URL");
       if ($.isArray(url) && url.length == 1) {
           url = url[0];
       }
       if (className == "WMTS") {
           var config = $.extend({ name: name, url: url, params: params }, options);
           return new layerClass(config);
       };
       return new layerClass(name, url, params, options);
   }

   , createBaseLayers: function (key, params, options, zoomLevels) {
       var className = this.app.config.get("WMS." + key + ".TYPE");
       if (typeof (Layer[className]) != 'function') {
           return [];
       }
       var c = this.app.config;
       zoomLevels = typeof zoomLevels !== 'undefined' ? zoomLevels : 29;
       var serverres = [4000, 3750, 3500, 3250, 3000, 2750, 2500, 2250, 2000, 1750, 1500, 1250, 1000, 750, 650, 500, 250, 100, 50, 20, 10, 5, 2.5, 2, 1.5, 1, 0.5, 0.25, 0.1];
       var serverresSliced = serverres.slice(0, zoomLevels);
       var opt = options = $.extend({ transitionEffect: 'resize' }, options);
       var p, layers = [], layer_names = this.app.config.get("WMS." + key + ".LAYERS");
       if (!$.isArray(layer_names)) {
           layer_names = [layer_names];
       };
       for (var i = 0, ii = layer_names.length; i < ii; ++i) {
           var layer = layer_names[i];
           if (c.has("WMS." + key + ".PARAMS")) {
               var cfg_params = c.get("WMS." + key + ".PARAMS") || p;
               p = cfg_params;
           } else {
               p = params;
           }
           if (className == "WMTS") {
               var wmts = {
                   matrixSet: 21781
          , requestEncoding: "REST"
          , format: "image/png"
          , formatSuffix: "png"
          , dimensions: ["TIME"]
          , serverResolutions: serverresSliced
          , maxExtent: new Bounds(420000, 30000, 900000, 350000)
          , layer: layer
          , style: "default"
               };
               opt = $.extend(wmts, options);
           }
           else {
               p = $.extend({ layers: layer, format: "image/PNG" }, params);
           };
           layers.push(this.createLayer(className, key, layer, p, opt));
       };
       return layers;
   }

   , createSubLayer: function (className, key, subKey, params, options) {
       params = $.extend({ layers: this.app.config.get("WMS." + key + ".LAYERS." + subKey), transparent: true, format: "PNG", dummy: new Date().getTime() }, params);
       options = $.extend({ singleTile: false, 'buffer': 0, tileSize: new OpenLayers.Size(1024, 1024), transitionEffect: 'resize' }, options);
       return this.createLayer(className, key, subKey, params, options);
   }

   , beforeBaseLayers: function () { }
   , addBaseLayers: function () {
       this.map.addLayers(this.createBaseLayers("TILES", {}, { format: "image/jpeg", formatSuffix: "jpeg" }, 27));
       this.map.addLayers(this.createBaseLayers("BACKGROUND", { TIME: 20120809 }, { buffer: 0, format: "image/jpeg", formatSuffix: "jpeg" }, 28));
       this.map.addLayers(this.createBaseLayers("BACKGROUND_AV", { TIME: 20121201 }, 29));
       this.map.addLayers(this.createBaseLayers("BACKGROUND2", {}, { buffer: 0 }, 29));
   }
   , afterBaseLayers: function () { }

   , beforeOverlays: function () { }
   , addOverlays: function () {

       var axis = this.axis = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "ACHSENSEGMENTE", {}, { visibility: false })
       , strabs = this.strabs = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "STRASSENABSCHNITTE", {}, { visibility: false })
       , zabs = this.zabs = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "ZUSTANDSABSCHNITTE", {}, { visibility: false })
       , zabstrot = this.zabstrot = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "ZUSTANDSABSCHNITTETROTTOIR", {}, { visibility: false })
       , koordinierte = this.koordinierte = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "MASSNAHMEN", {}, { visibility: false })
       , realisiert = this.realisiert = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "REALISIERT", {}, { visibility: false })
       , teilsysteme = this.teilsysteme = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "VORSCHLAG", {}, { visibility: false })
       , vorschlaege = this.vorschlaege = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "VORSCHLAEGE", {}, { visibility: false })
       , vorschlaegetrot = this.vorschlaegetrot = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "VORSCHLAEGETROTTOIR", {}, { visibility: false })
       , inspektion = this.route = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "INSPEKTIONSROUTE", {}, { visibility: false })
       , konflikte = this.konflikte = this.createSubLayer("ArcGIS93Rest", "ARCGIS", "ACHSEN_UPDATE_KONFLIKTE", {}, { visibility: false });

       this.map.addLayers([axis, realisiert, koordinierte, teilsysteme, strabs, inspektion, vorschlaegetrot, vorschlaege, konflikte, zabstrot, zabs]);

       var av_group = this.app.config.get("TEXT.AV_LAYERGROUP");
       this.map.addLayers(this.createBaseLayers("OVERLAY", { format: "image/png", transparent: true }, { visibility: false, group: av_group }));
       this.map.addLayers(this.createBaseLayers("OVERLAY_AV", { format: "image/png", transparent: true, TIME: 20101109 }, { visibility: false, group: av_group, isBaseLayer: false }, 27));
       this.map.addLayers(this.createBaseLayers("OVERLAY_AV_GWVK", { format: "image/png", transparent: true, TIME: 20070101 }, { visibility: false, group: av_group, isBaseLayer: false }, 27));
       this.map.addLayers(this.createBaseLayers("OVERLAY_AV_GWVUL", { format: "image/png", transparent: true, TIME: 20070914 }, { visibility: false, group: av_group, isBaseLayer: false }, 27));
       this.map.addLayers(this.createBaseLayers("OVERLAY_AV_KBS", { format: "image/png", transparent: true }, { visibility: false, group: av_group }));
   }
   , afterOverlays: function () { }

   , init: function () {
       OL.DOTS_PER_INCH = 96;
       var config = {};
       config.projection = new Projection(this.app.config.get("GIS.PROJECTION"));
       config.units = 'm';
       config.maxExtent = new Bounds(485869.5728, 76443.1884, 837076.5648, 299941.7864);
       config.scales = this.app.config.get("GIS.SCALES").split(",");
       config.controls = [];

       this.map = new Map('map', config);
       this.beforeBaseLayers();
       this.addBaseLayers();
       this.afterBaseLayers();
       this.beforeOverlays();
       this.addOverlays();
       this.afterOverlays();

       // for remote, disableling topo layer
       if (this.app.debug) this.map.layers[0].visibility = false;
   }

   , addControls: function () {
       var map = this;
       var nav, ctrl = [], panel = this.panel_ = new Control.Panel({ displayClass: "panelTopLeft", allowDepress: true });
       this.tool_nav_ = new Control({ displayClass: "DummyPan", title: this.app.config.get("TEXT.TOOLTIP.TOOL_PAN") });
       this.tool_zoom_ = new Control.ZoomBox({ title: this.app.config.get("TEXT.TOOLTIP.TOOL_ZOOM") });
       ctrl.push(this.tool_nav_);
       ctrl.push(this.tool_zoom_);
       var trigger = function () { map.zoomToDefault(); };
       ctrl.push(new Control.Button({ displayClass: "FullExtent", trigger: trigger, title: this.app.config.get("TEXT.TOOLTIP.TOOL_ZOOM_TO_FULL_EXTENT") }));
       if (this.zoomToCurrentControl) {
           trigger = function () { map.zoomToCurrent(); };
           ctrl.push(this.zoom_current = new Control.Button({ displayClass: "Current", trigger: trigger, title: this.app.config.get("TEXT.TOOLTIP.TOOL_ZOOM_TO_CURRENT") }));
           var l = this.editLayer;
           if (l) {
               l.events.register("featureadded", this, this.hasFeature);
               l.events.register("featuresadded", this, this.hasFeature);
               l.events.register("featureremoved", this, this.hasFeature);
               l.events.register("featuresremoved", this, this.hasFeature);
           };
       };
       this.history = new Control.NavigationHistory({
           previousOptions: { displayClass: "Prev", title: this.app.config.get("TEXT.TOOLTIP.TOOL_PREVIOUS_EXTENT") }
     , nextOptions: { displayClass: "Next", title: this.app.config.get("TEXT.TOOLTIP.TOOL_NEXT_EXTENT") }
       });
       ctrl.push(this.history.previous);
       ctrl.push(this.history.next);

       ctrl.push(new Control.MeasureEx(Path, { displayClass: "MeasureLine", title: this.app.config.get("TEXT.TOOLTIP.TOOL_MEASURE_LINE") }));
       ctrl.push(new Control.MeasureEx(Polygon, { displayClass: "MeasureArea", title: this.app.config.get("TEXT.TOOLTIP.TOOL_MEASURE_AREA"), numDigits: 0 }));

       if (this.legend) {
           trigger = function () {
               var legend = emsg.legend;
               if (legend) {
                   legend.open();
               };
           };
           ctrl.push(new Control.Button({ displayClass: "Legend", trigger: trigger, title: this.app.config.get("TEXT.TOOLTIP.TOOL_LEGEND") }));
       };
       if (this.pdfDownload) {
           trigger = function () {
               emsg.form.generateGisMapPdfReport();
           };
           ctrl.push(new Control.Button({ displayClass: "PdfDownload",
               trigger: function () {
                   if (map.hasVisibleReportLayer()) {
                       emsg.form.previewGisMapPdfReport();
                   } else {
                       alert(map.app.config.get("TEXT.ERROR.NO_REPORTLAYER"));
                   }
               }, title: this.app.config.get("TEXT.TOOLTIP.TOOL_PDF_PREVIEW")
           }));
       };
       panel.addControls(ctrl);
       ctrl = [];
       ctrl.push(new Control.Navigation());
       ctrl.push(this.history);
       ctrl.push(panel);
       ctrl.push(new Control.PanZoomBar());
       ctrl.push(this.scale_line = new Control.ScaleLine());
       ctrl.push(new Control.LayerSwitcher.Grouping());
       var str_format = this.app.config.get("TEXT.MOUSE_POS.FORMAT");
       var format = function (pos) {
           var r = Emsg.Util.round;
           pos = { lon: r(pos.lon, 1), lat: r(pos.lat, 1) };
           return OL.String.format(str_format, pos);
       };
       ctrl.push(new Control.MousePosition({ formatOutput: format }));

       this.map.addControls(ctrl);
       this.activateNav();
   }

   , activateNav: function () {
       this.panel_.activateControl(this.tool_nav_);
   }

   , updateSize: function () {
       if (this.map) this.map.updateSize.apply(this.map, arguments);
   }

   , zoomToDefault: function () {
       if (this.extent) this.map.zoomToExtent(this.extent);
   }

   , zoomToCurrent: function () {
       var l = this.editLayer;
       l = l && l.getDataExtent();
       l && this.map.zoomToExtent(l);
   }


   , readExtent: function (url) {
       var that = this;
       $.get(url, { salt: new Date().getTime() }, function () { that.setExtent.apply(that, arguments); }, 'json');
   }

   , setExtent: function (result) {
       var e = this.extent = OpenLayers.Bounds.fromArray(result);
       if (this.map) {
           if (e.getWidth() == -1 || e.getHeight() == -1) {
               this.map.zoomTo(1);
               this.extent = this.map.getMaxExtent();
           }
           else {
               this.map.zoomToExtent(this.extent);
           }
       };
   }

   , readExtentWithoutZoom: function (url) {
       var that = this;
       $.get(url, { salt: new Date().getTime() }, function () { that.setExtentWithoutZoom.apply(that, arguments); }, 'json');
   }

   , setExtentWithoutZoom: function (result) {
       this.extent = OpenLayers.Bounds.fromArray(result);
   }

   , stopZoom: function () {
       this.activateNav();
   }

   , hasFeature: function (has) {
       var c = this.zoom_current;
       if (c == null) return;
       var l = this.editLayer;
       if (l && l.getDataExtent()) {
           c.activate();
       }
       else {
           c.deactivate();
       };
   }
    });
})();
